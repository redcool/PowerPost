﻿using PowerUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#if UNITY_2020
using UniversalRenderer = UnityEngine.Rendering.Universal.ForwardRenderer;
#endif

namespace PowerPost
{
    public abstract class BasePostExPass : ScriptableRenderPass
    {
        public UniversalRenderer Renderer { set; get; }

        public const string POWER_POST_DEFAULT_SHADER = "Hidden/PowerPost/DefaultBlit";
        Material defaultMaterial;
        public Material DefaultBlitMaterial
        {
            get
            {
                if (!defaultMaterial)
                {
                    var shader = Shader.Find(POWER_POST_DEFAULT_SHADER);
                    Debug.Assert(shader != null, $"shader ' {POWER_POST_DEFAULT_SHADER} ' not found");

                    defaultMaterial = new Material(shader);
                }
                return defaultMaterial;
            }
        }

        /// <summary>
        /// current soruceId,targetId,next post will change
        /// </summary>
        public RTHandle sourceTex, targetTex;

        /// <summary>
        /// pass's order inject from PowerPostFeature.cs
        /// 1 used for sort
        /// 2 used for change(AttachmentA,B)
        /// </summary>
        public int order;

        /// <summary>
        /// index of powerPost's rendering list
        /// </summary>
        public int renderingId;

        public bool isNeedReleaseGlobal, isNeedInitGlobal;

        /// <summary>
        /// inject by PowerPostFFeature
        /// </summary>
        public BasePostExSettings settings;

        public BasePostExPass()
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public RenderTargetIdentifier DepthTarget
        {
            get
            {
#if UNITY_2022_1_OR_NEWER
                return Renderer.cameraColorTargetHandle;
#else
                var urpAsset = UniversalRenderPipeline.asset;
                return urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;
#endif
            }
        }

        public RTHandle ColorTarget
            => Renderer.CameraColorTargetHandle();

        public BasePostExPass InitStatesForWrtieCameraTarget(int id, int count)
        {
            renderingId = id;

            isNeedInitGlobal = id == 0;

            var isOdd = (id % 2 != 0);
            isNeedReleaseGlobal = (id == count - 1) && !isOdd;

            return this;
        }
    }

    public abstract class BasePostExPass<T> : BasePostExPass where T : BasePostExSettings
    {
        Material material;


        public Material GetTargetMaterial(string shaderName)
        {
            if (!material)
            {
                var shader = Shader.Find(shaderName);
                Debug.Assert(shader != null, $"shader ' {shaderName} ' not found");

                material = new Material(shader);
            }
            return material;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings == null || !settings.IsActive())
                return;

            Renderer = (UniversalRenderer)renderingData.cameraData.renderer;
            SetupTargets(Renderer);

            var cmd = CommandBufferUtils.Get(ref context, PassName);
            ref var cameraData = ref renderingData.cameraData;

            if (isNeedInitGlobal && settings.NeedWriteToTarget())
            {
                InitGlobal(cmd, ref cameraData);
            }

            OnExecute(context, ref renderingData, (T)settings, cmd);

            if (isNeedReleaseGlobal && settings.NeedWriteToTarget())
            {
                ReleaseGlobal(cmd);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferUtils.ClearRelease(cmd);
        }

        private void SetupTargets(ScriptableRenderer renderer)
        {
            if (renderer is UniversalRenderer r)
            {
#if UNITY_2020
                var ca = RTHandles.Alloc(ShaderPropertyIds._CameraColorTexture);
                var cb = RTHandles.Alloc(ShaderPropertyIds._CameraColorAttachmentB);
#else
                var ca = r.GetCameraColorAttachmentA();
                var cb = r.GetCameraColorAttachmentB();
#endif

                sourceTex = ca;
                targetTex = cb;

                if (order % 2 == 1)
                {
                    sourceTex = cb;
                    targetTex = ca;
                }

            }
        }

        public abstract void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, T settings, CommandBuffer cmd);
        public abstract string PassName { get; }


        void InitGlobal(CommandBuffer cmd, ref CameraData cameraData)
        {
            cmd.GetTemporaryRT(ShaderPropertyIds._CameraColorAttachmentB, cameraData.cameraTargetDescriptor);
        }

        void ReleaseGlobal(CommandBuffer cmd)
        {
            if (SystemInfo.copyTextureSupport == CopyTextureSupport.Basic)
                CopyTargetTextures(cmd);
            else
                BlitTargetTextures(cmd);
            //cmd.ReleaseTemporaryRT(ShaderPropertyIds._CameraColorAttachmentB);
        }

        void BlitTargetTextures(CommandBuffer cmd)
        {
#if UNITY_2020
            var ca = RTHandles.Alloc(ShaderPropertyIds._CameraColorTexture);
            var cb = RTHandles.Alloc(ShaderPropertyIds._CameraColorAttachmentB);
#else
            var ca = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentA);
            var cb = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentB);
#endif
            cmd.BlitColorDepth(cb, ca, ca, DefaultBlitMaterial);
        }

        void CopyTargetTextures(CommandBuffer cmd)
        {
#if UNITY_2020
            var ca = RTHandles.Alloc(ShaderPropertyIds._CameraColorTexture);
            var cb = RTHandles.Alloc(ShaderPropertyIds._CameraColorAttachmentB);
#else
            var ca = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentA);
            var cb = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentB);
#endif
            cmd.CopyTexture(cb, ca);
        }
    }

}
