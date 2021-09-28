#if UNITY_POST_PROCESSING_STACK_V2
namespace PostProcessiongEx
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.PostProcessing;
    using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
    using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
    using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
    using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
    [Serializable]
    [PostProcess(typeof(SimpleDOFRenderer), PostProcessEvent.BeforeStack, "Custom/SimpleDOF")]
    public class SimpleDOF : PostProcessEffectSettings
    {
        [Range(0, 1)] public FloatParameter distance = new FloatParameter { value = 1 };
        [Range(1,8)]public IntParameter downsample = new IntParameter { value = 2 };
        [Range(1,8)]public FloatParameter blurSize = new FloatParameter { value = 2 };
        [Range(0, 1)] public FloatParameter depthRange = new FloatParameter { value = 0.05f };
        public BoolParameter debugMode = new BoolParameter { value = false };
    }

    public class SimpleDOFRenderer : PostProcessEffectRenderer<SimpleDOF>
    {
        static int COLOR_RT_ID = Shader.PropertyToID("_ColorRT");
        static int DEPTH_RT_ID = Shader.PropertyToID("_DepthRT");
        static int DISTANCE_ID = Shader.PropertyToID("_Distance");
        static int BLUR_SIZE_ID = Shader.PropertyToID("_BlurSize");
        static int DEPTH_RANGE_ID = Shader.PropertyToID("_DepthRange");
        static int DEBUG_ID = Shader.PropertyToID("_Debug");

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/SimpleDOF"));

            var downsample = settings.downsample.value;
            downsample = downsample <= 0 ? 1 : downsample;

            var width = context.width / downsample;
            var height = context.height / downsample;

            var cmd = context.command;
            cmd.BeginSample(nameof(SimpleDOF));
            // init
            cmd.GetTemporaryRT(COLOR_RT_ID, width, height, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(DEPTH_RT_ID, width, height, 16, FilterMode.Bilinear, RenderTextureFormat.R16);

            //blit depth
            cmd.BlitFullscreenTriangle(BuiltinRenderTextureType.Depth, DEPTH_RT_ID);
            cmd.SetGlobalTexture(DEPTH_RT_ID, DEPTH_RT_ID);
            // set props
            sheet.properties.SetFloat(DISTANCE_ID, settings.distance.value);
            sheet.properties.SetFloat(BLUR_SIZE_ID, settings.blurSize.value);
            sheet.properties.SetFloat(DEPTH_RANGE_ID, settings.depthRange.value);
            sheet.properties.SetFloat(DEBUG_ID, settings.debugMode.value ? 1 : 0);
            // blit color
            cmd.BlitFullscreenTriangle(context.source, COLOR_RT_ID, sheet, 0);

            cmd.BlitFullscreenTriangle(COLOR_RT_ID, context.destination);

            //clean
            cmd.ReleaseTemporaryRT(COLOR_RT_ID);
            cmd.ReleaseTemporaryRT(DEPTH_RT_ID);
            cmd.EndSample(nameof(SimpleDOF));
        }
    }
}
#endif