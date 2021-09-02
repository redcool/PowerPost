using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost {
    public class SSSSPass : PostExPass
    {
        public Material mat;
        int sceneColorRTId = Shader.PropertyToID("_SceneColorRT");
        List<Vector4> kernels = new List<Vector4>();


        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var sssSetings = VolumeManager.instance.stack.GetComponent<SSSSSettings>();
            if (!sssSetings.IsActive())
                return;

            SSSSKernel.CalculateKernel(kernels,25, sssSetings.strength.value, sssSetings.falloff.value);

            var cmd = CommandBufferPool.Get();
            cmd.BeginSample("SSSS Pass");
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            if(!mat)
                mat = new Material(Shader.Find("Hidden/PostProcessingEx/ScreenDiffuseProfile"));
            //cmd.SetGlobalVectorArray("_Kernel",kernels);
            mat.SetVectorArray("_Kernel",kernels);
            mat.SetFloat("_BlurSize",sssSetings.blurScale.value);

            //cmd.SetGlobalTexture("_MainTex", Renderer.cameraColorTarget);
            //cmd.SetRenderTarget(sceneColorRTId, Renderer.cameraColorTarget);
            //cmd.DrawMesh(CommandBufferEx.FullscreenQuad, Matrix4x4.identity, mat, 0, 0);

            var urpAsset = UniversalRenderPipeline.asset;
            var depthTarget = urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;

            cmd.BlitColorDepth(Renderer.cameraColorTarget, sceneColorRTId, depthTarget, mat, 0);
            cmd.BlitColorDepth(sceneColorRTId, Renderer.cameraColorTarget, depthTarget, mat, 1);
            cmd.EndSample("SSSS Pass");
            context.ExecuteCommandBuffer(cmd);

            CommandBufferPool.Release(cmd);
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