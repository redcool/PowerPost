namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class GlitchPass : BasePostExPass<GlitchSettings>
    {
        const string GLITCH_SHADER = "Hidden/PowerPost/Glitch";
        int _ScanlineJiiterId = Shader.PropertyToID("_ScanlineJitter");
        int _SnowFlake = Shader.PropertyToID("_SnowFlake");
        int _VerticalJump = Shader.PropertyToID("_VerticalJump");
        int _HorizontalShake = Shader.PropertyToID("_HorizontalShake");
        int _ColorDrift = Shader.PropertyToID("_ColorDrift");
        int _StencilRef = Shader.PropertyToID("_StencilRef");
        int _BlockSize = Shader.PropertyToID("_BlockSize");

        float verticalJumpTime;

        public override string PassName => nameof(GlitchPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData,GlitchSettings settings,CommandBuffer cmd)
        {
            // draw layer's objects
            if (settings.layer.value != 0)
            {
                var stencilRef = settings.stencilRef.value;
                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, settings.layer.value, (ref RenderStateBlock block) =>
                {
                    GraphicsUtils.SetStencilState(ref block, stencilRef, new StencilState(passOperation: StencilOp.Replace));
                });
            }

            var mat = GetTargetMaterial(GLITCH_SHADER);
            
            //jitter
            var jitterIntensity = (settings.scanlineJitter.value * 1.2f);
            var jitterThreshold = 0.002f + Mathf.Pow(settings.scanlineJitter.value, 3) * 0.05f;
            mat.SetVector(_ScanlineJiiterId, new Vector4(
                jitterIntensity, 
                jitterThreshold,
                1f/settings.jitterBlockSize.value, 
                settings.glitchHorizontalIntensity.value
                ));
            // snow flake
            mat.SetVector(_SnowFlake, new Vector2(
                Mathf.Sin(Random.value) * 2,
                settings.snowFlakeAmplitude.value));

            verticalJumpTime += Time.deltaTime * settings.verticalJump.value * 10;
            mat.SetVector(_VerticalJump, new Vector2(
                settings.verticalJump.value,
                verticalJumpTime));
            mat.SetFloat(_HorizontalShake, settings.horizontalShake.value * 0.2f);

            //sin(y) * x :
            //x: amplitude, y : period
            mat.SetVector(_ColorDrift, new Vector2(
                settings.colorDrift.value * 0.4f,
                Time.time * settings.colorDriftSpeed.value));

            mat.SetInt(_StencilRef, settings.layer.value != 0 ? settings.stencilRef.value : 0);


            cmd.BlitColorDepth(BuiltinRenderTextureType.None, ColorTarget, ColorTarget, mat, 0);

        }
    }
}