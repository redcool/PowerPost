using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost {
    public class SSSSPass : BasePostExPass<SSSSSettings>
    {
        const string DIFFUSE_PROFILE_SHADER = "Hidden/PowerPost/ScreenDiffuseProfile";

        int sceneColorRTId = Shader.PropertyToID("_SceneColorRT");
        int _Kernel = Shader.PropertyToID("_Kernel");
        int _BlurSize = Shader.PropertyToID("_BlurSize");
        int _StencilRef = Shader.PropertyToID("_StencilRef");

        Material mat;
        List<Vector4> kernels = new List<Vector4>();
        Color lastStrength, lastFalloff;

        void CalcSSSSKernel(SSSSSettings settings, List<Vector4> kernels)
        {
            var needUpdate = lastStrength != settings.strength.value || lastFalloff != settings.falloff.value;
            if (!needUpdate)
                return;

            lastStrength = settings.strength.value;
            lastFalloff = settings.falloff.value;
            SSSSKernel.CalculateKernel(kernels, 25, lastStrength, lastFalloff);
        }

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SSSSSettings settings)
        {
            //update kernel 
            CalcSSSSKernel(settings, kernels);

            // command
            var cmd = CommandBufferPool.Get();
            cmd.BeginSample(nameof(SSSSPass));

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            // update material
            if(!mat)
                mat = new Material(Shader.Find(DIFFUSE_PROFILE_SHADER));
            mat.SetVectorArray(_Kernel,kernels);
            mat.SetFloat(_BlurSize,settings.blurScale.value);
            mat.SetInt(_StencilRef, settings.stencilRef.value);

            // look urp asset's depthTexture mode
            var urpAsset = UniversalRenderPipeline.asset;
            var depthTarget = urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;

            cmd.BlitColorDepth(Renderer.cameraColorTarget, sceneColorRTId, depthTarget, mat, 0);
            cmd.BlitColorDepth(sceneColorRTId, Renderer.cameraColorTarget, depthTarget, mat, 1);
            context.ExecuteCommandBuffer(cmd);

            CommandBufferPool.Release(cmd);
            cmd.EndSample(nameof(SSSSPass));
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(sceneColorRTId, cameraTextureDescriptor);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(sceneColorRTId);
        }
    }
}