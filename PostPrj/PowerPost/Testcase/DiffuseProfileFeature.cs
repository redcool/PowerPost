using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 1 Render Objects render target character ,set stencil override 5
/// 2 use this feature render diffuse profile
/// </summary>
public class DiffuseProfileFeature : ScriptableRendererFeature
{
    [Serializable]
    public class Settings
    {
        public Color strength = Color.red, falloff = Color.red;
        public float blurSize = 1;
    }
    class CustomRenderPass : ScriptableRenderPass
    {
        public Settings settings;
        int sceneColorRTId = Shader.PropertyToID("_SceneColorRT");
        Material mat;
        List<Vector4> kernels = new List<Vector4>();

        public ScriptableRenderer renderer;

        //public RenderTargetIdentifier colorId,depthId;
        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(sceneColorRTId, cameraTextureDescriptor);
            base.Configure(cmd, cameraTextureDescriptor);
        }


        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (settings == null)
                return;

            var cmd = CommandBufferPool.Get();
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            if (!mat)
                mat = new Material(Shader.Find("Hidden/PostProcessingEx/ScreenDiffuseProfile"));

            SSSSKernel.CalculateKernel(kernels, 25, settings.strength, settings.falloff);
            mat.SetVectorArray("_Kernel",kernels);
            mat.SetFloat("_BlurSize",settings.blurSize);


            //cmd.SetGlobalTexture("_MainTex",renderer.cameraColorTarget);
            //cmd.SetRenderTarget(sceneColorRTId, renderer.cameraColorTarget);
            //cmd.DrawMesh(CommandBufferEx.FullscreenQuad, Matrix4x4.identity, mat, 0, 0);

            cmd.BlitColorDepth(renderer.cameraColorTarget, sceneColorRTId, renderer.cameraColorTarget, mat, 0);

            //cmd.SetGlobalTexture("_MainTex", sceneColorRTId);
            //cmd.SetRenderTarget(renderer.cameraColorTarget);
            //cmd.DrawMesh(CommandBufferEx.FullscreenQuad, Matrix4x4.identity, mat, 0, 1);
            cmd.BlitColorDepth(sceneColorRTId, renderer.cameraColorTarget, renderer.cameraColorTarget, mat, 1);


            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(sceneColorRTId);
        }

    }

    CustomRenderPass m_ScriptablePass;
    public Settings settings;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();
        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing ;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        m_ScriptablePass.settings = settings;
        m_ScriptablePass.renderer = renderer;
        renderer.EnqueuePass(m_ScriptablePass);
    }
}


