#if UNITY_POST_PROCESSING_STACK_V2
namespace PostProcessiongEx
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
    using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
    using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
    using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
    using UnityEngine.Rendering.PostProcessing;
    using System;
    using Random = UnityEngine.Random;

    [Serializable]
    [PostProcess(typeof(GlitchRenderer), PostProcessEvent.BeforeStack,"Custom/Glitch")]
    public class Glitch : PostProcessEffectSettings
    {
        [Range(0, 1)] public FloatParameter scanlineJitter = new FloatParameter { value = 0 };
        [Range(0,1)]public FloatParameter snowFlakeAmplitude = new FloatParameter { value = 0 };
        [Range(0, 1)] public FloatParameter verticalJump = new FloatParameter { value = 0 };
        [Range(0, 1)] public FloatParameter horizontalShake = new FloatParameter { value=0};
        [Range(0, 1)] public FloatParameter colorDrift = new FloatParameter { value = 0 };
    }

    public class GlitchRenderer : PostProcessEffectRenderer<Glitch>
    {
        int _ScanlineJiiterId = Shader.PropertyToID("_ScanlineJitter");
        int _SnowFlake = Shader.PropertyToID("_SnowFlake");
        int _VerticalJump = Shader.PropertyToID("_VerticalJump");
        int _HorizontalShake = Shader.PropertyToID("_HorizontalShake");
        int _ColorDrift = Shader.PropertyToID("_ColorDrift");

        float verticalJumpTime;

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Glitch"));
            var cmd = context.command;
            cmd.BeginSample(nameof(Glitch));

            //jitter
            var sl_threshold = Mathf.Clamp01(settings.scanlineJitter.value * 1.2f);
            var sl_disp = 0.002f + Mathf.Pow(settings.scanlineJitter.value, 3) * 0.05f;
            sheet.properties.SetVector(_ScanlineJiiterId, new Vector2(sl_threshold,sl_disp));
            // snow flake
            sheet.properties.SetVector(_SnowFlake, new Vector2(Mathf.Sin(Random.value) * 0.1f, settings.snowFlakeAmplitude));

            verticalJumpTime += Time.deltaTime * settings.verticalJump.value * 10;
            sheet.properties.SetVector(_VerticalJump,new Vector2(settings.verticalJump.value,verticalJumpTime));
            sheet.properties.SetFloat(_HorizontalShake, settings.horizontalShake.value * 0.2f);
            sheet.properties.SetVector(_ColorDrift, new Vector2(settings.colorDrift.value * 0.04f, Time.time * 666.66f));

            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(nameof(Glitch));
        }
    }
}
#endif