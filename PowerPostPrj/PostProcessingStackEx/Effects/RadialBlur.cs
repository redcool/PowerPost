namespace PostProcessiongEx
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering.PostProcessing;

    [PostProcess(typeof(RadialBlurRenderer), PostProcessEvent.BeforeStack,"Custom/Radial Blur")]
    public class RadialBlur : PostProcessEffectSettings
    {
        public Vector2Parameter center = new Vector2Parameter { value = new Vector2(.5f, .5f) };
        [Range(0,1)]public FloatParameter radiusMin = new FloatParameter { value = 0 };
        [Range(0,1)]public FloatParameter radiusMax = new FloatParameter { value = 1 };
        public FloatParameter blurSize = new FloatParameter { value = 0.5f };
        public BoolParameter roundness = new BoolParameter();
    }

    public class RadialBlurRenderer : PostProcessEffectRenderer<RadialBlur>
    {
        const string RADIAL_BLUR_SHADER = "Hidden/Custom/RadialBlur";
        static int _Center = Shader.PropertyToID("_Center");
        static int _RadiusMin = Shader.PropertyToID("_RadiusMin");
        static int _RadiusMax = Shader.PropertyToID("_RadiusMax");
        static int _BlurSize = Shader.PropertyToID("_BlurSize");
        static int _Aspect = Shader.PropertyToID("_Aspect");


        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find(RADIAL_BLUR_SHADER));
            sheet.properties.SetVector(_Center, settings.center.value);
            sheet.properties.SetFloat(_RadiusMin, settings.radiusMin.value);
            sheet.properties.SetFloat(_RadiusMax,settings.radiusMax.value);

            sheet.properties.SetVector(_BlurSize, new Vector2(settings.blurSize.value, 0));
            sheet.properties.SetFloat(_Aspect, settings.roundness.value ? (context.width/(float)context.height) : 1);

            var cmd = context.command;
            cmd.BeginSample(nameof(RadialBlur));
            cmd.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
            cmd.EndSample(nameof(RadialBlur));
        }
    }
}