using PowerUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public abstract class BasePostExPass : ScriptableRenderPass
    {
        public ScriptableRenderer Renderer { set; get; }

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

        public RenderTargetIdentifier sourceTex, targetTex;
        /// <summary>
        /// pass's order inject from PowerPostFeature.cs, used for sort
        /// </summary>
        public int order;


        public bool isNeedReleaseGlobal, isNeedInitGlobal;
        protected bool isCameraSwapTarget;

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
#if UNITY_2023_1_OR_NEWER
                return Renderer.cameraColorTargetHandle;
#else
                return urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;
#endif
            }
        }

        public RenderTargetIdentifier ColorTarget
        {
#if UNITY_2023_1_OR_NEWER
            get { return Renderer.cameraColorTargetHandle; }
#else
            get { return Renderer.cameraColorTarget; }
#endif
        }

        public BasePostExPass InitStatesForWrtieCameraTarget(int id, int count,bool needSwapTarget)
        {
            isCameraSwapTarget = needSwapTarget;
            // target is A , post target is B, otherwist swap
            var isOdd = (id % 2 != 0);
            var isSwap = needSwapTarget? !isOdd : isOdd;

            isNeedInitGlobal = id ==0;
            isNeedReleaseGlobal = (id ==count-1) && !isOdd;
            sourceTex = isSwap ? ShaderPropertyIds._CameraColorAttachmentB : ShaderPropertyIds._CameraColorAttachmentA;
            targetTex = isSwap ? ShaderPropertyIds._CameraColorAttachmentA : ShaderPropertyIds._CameraColorAttachmentB;
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
            
            Renderer = renderingData.cameraData.renderer;

            var cmd = CommandBufferUtils.Get(ref context, PassName);
            ref var cameraData = ref renderingData.cameraData;

            if (isNeedInitGlobal && settings.NeedWriteToTarget())
            {
                InitGlobal(cmd,ref cameraData);
            }

            OnExecute(context, ref renderingData, settings,cmd);

            if (isNeedReleaseGlobal && settings.NeedWriteToTarget())
            {
                ReleaseGlobal(cmd);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferUtils.ClearRelease(cmd);
        }

        public abstract void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData,T settings,CommandBuffer cmd);
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
            if (isCameraSwapTarget)
            {
                cmd.BlitColorDepth(ShaderPropertyIds._CameraColorAttachmentA, ShaderPropertyIds._CameraColorAttachmentB, ShaderPropertyIds._CameraColorAttachmentB, DefaultBlitMaterial);
            }
            else
            {
                cmd.BlitColorDepth(ShaderPropertyIds._CameraColorAttachmentB, ShaderPropertyIds._CameraColorAttachmentA, ShaderPropertyIds._CameraColorAttachmentA, DefaultBlitMaterial);
            }
        }

        void CopyTargetTextures(CommandBuffer cmd)
        {
            if (isCameraSwapTarget)
            {
                cmd.CopyTexture(ShaderPropertyIds._CameraColorAttachmentA, ShaderPropertyIds._CameraColorAttachmentB);
            }
            else
            {
                cmd.CopyTexture(ShaderPropertyIds._CameraColorAttachmentB, ShaderPropertyIds._CameraColorAttachmentA);
            }
        }
    }

}
