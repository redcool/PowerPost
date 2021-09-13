namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class GlitchPass : BasePostExPass
    {
        const string GLITCH_SHADER = "Hidden/PowerPost/Glitch";
        int _ScanlineJiiterId = Shader.PropertyToID("_ScanlineJitter");
        int _SnowFlake = Shader.PropertyToID("_SnowFlake");
        int _VerticalJump = Shader.PropertyToID("_VerticalJump");
        int _HorizontalShake = Shader.PropertyToID("_HorizontalShake");
        int _ColorDrift = Shader.PropertyToID("_ColorDrift");

        Material mat;
        float verticalJumpTime;
        int _ColorRT = Shader.PropertyToID("_ColorRT");

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var settings = GetSettings<GlitchSettings>();
            if (!settings.IsActive())
                return;

            var cmd = CommandBufferUtils.GetBufferBeginSample(context,nameof(GlitchPass));

            var stencilRef = settings.stencilRef.value;

            GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, GraphicsUtils.shaderTags, settings.layer.value,(ref RenderStateBlock block)=> {
                GraphicsUtils.SetStencilState(ref block, stencilRef, new StencilState(passOperation: StencilOp.Replace));
            });

            if (!mat)
                mat = new Material(Shader.Find(GLITCH_SHADER));

            //jitter
            var sl_threshold = Mathf.Clamp01(settings.scanlineJitter.value * 1.2f);
            var sl_disp = 0.002f + Mathf.Pow(settings.scanlineJitter.value, 3) * 0.05f;
            mat.SetVector(_ScanlineJiiterId, new Vector2(sl_threshold, sl_disp));
            // snow flake
            mat.SetVector(_SnowFlake, new Vector2(Mathf.Sin(Random.value) * 0.1f, settings.snowFlakeAmplitude.value));

            verticalJumpTime += Time.deltaTime * settings.verticalJump.value * 10;
            mat.SetVector(_VerticalJump, new Vector2(settings.verticalJump.value, verticalJumpTime));
            mat.SetFloat(_HorizontalShake, settings.horizontalShake.value * 0.2f);
            mat.SetVector(_ColorDrift, new Vector2(settings.colorDrift.value * 0.04f, Time.time * 666.66f));

            var urpAsset = UniversalRenderPipeline.asset;
            var depthTarget = urpAsset.supportsCameraDepthTexture ? Renderer.cameraDepthTarget : Renderer.cameraColorTarget;

            cmd.BlitColorDepth(Renderer.cameraColorTarget, _ColorRT, depthTarget, mat, 0);
            cmd.BlitColorDepth(_ColorRT, Renderer.cameraColorTarget, depthTarget, mat, 1);
            context.ExecuteCommandBuffer(cmd);

            CommandBufferUtils.ReleaseBufferEndSample(context,cmd);
            cmd.Clear();
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(_ColorRT, cameraTextureDescriptor);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ColorRT);
        }
    }
}