using PowerUtilities;
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
                    defaultMaterial = new Material(Shader.Find(POWER_POST_DEFAULT_SHADER));
                }
                return defaultMaterial;
            }
        }

        public RTHandle sourceTex, targetTex;
        /// <summary>
        /// pass's order inject from PowerPostFeature.cs, used for sort
        /// </summary>
        public int order;

        /// <summary>
        /// index of powerPost's rendering list
        /// </summary>
        public int renderingId;

        public bool isNeedReleaseGlobal, isNeedInitGlobal;

        public BasePostExPass()
        {
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public T GetSettings<T>() where T :BasePostExSettings
        {
            return VolumeManager.instance.stack.GetComponent<T>();
        }

        public RenderTargetIdentifier DepthTarget
        {
            get
            {
                var urpAsset = UniversalRenderPipeline.asset;
#if UNITY_2022_1_OR_NEWER
                return Renderer.cameraColorTargetHandle;
#else
                return urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;
#endif
            }
        }

        public RTHandle ColorTarget => Renderer.GetRTHandle(URPRTHandleNames.m_ActiveCameraColorAttachment);


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
                material = new Material(Shader.Find(shaderName));
            return material;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var settings = GetSettings<T>();
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

            OnExecute(context, ref renderingData, settings, cmd);

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
                var ca = r.GetRTHandle(URPRTHandleNames._CameraColorAttachmentA);
                var cb = r.GetRTHandle(URPRTHandleNames._CameraColorAttachmentB);
                var curTarget = r.GetRTHandle(URPRTHandleNames.m_ActiveCameraColorAttachment);

                sourceTex = ca;
                targetTex = cb;

                if (curTarget == cb)
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
            var ca = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentA);
            var cb = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentB);
            cmd.BlitColorDepth(cb, ca, ca, DefaultBlitMaterial);
        }

        void CopyTargetTextures(CommandBuffer cmd)
        {
            var ca = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentA);
            var cb = Renderer.GetRTHandle(URPRTHandleNames._CameraColorAttachmentB);
            cmd.CopyTexture(cb, ca);
        }
    }

}
