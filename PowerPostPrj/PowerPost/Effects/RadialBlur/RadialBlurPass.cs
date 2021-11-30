﻿using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public class RadialBlurPass : BasePostExPass<RadialBlurSettings>
    {
        const string SHADER_PATH = "Hidden/PowerPost/RadialBlur";
        static int _Center = Shader.PropertyToID("_Center");
        static int _RadiusMin = Shader.PropertyToID("_RadiusMin");
        static int _RadiusMax = Shader.PropertyToID("_RadiusMax");
        static int _BlurSize = Shader.PropertyToID("_BlurSize");
        static int _Aspect = Shader.PropertyToID("_Aspect");

        static int _ColorRT = Shader.PropertyToID("_ColorRT");

        Material mat;

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, RadialBlurSettings settings)
        {
            var cam = renderingData.cameraData.camera;
            var aspect = cam.pixelWidth / (float)cam.pixelHeight;

            if (!mat)
                mat = new Material(Shader.Find(SHADER_PATH));

            mat.SetVector(_Center, settings.center.value);
            mat.SetFloat(_RadiusMin, settings.radiusMin.value);
            mat.SetFloat(_RadiusMax, settings.radiusMax.value);

            mat.SetFloat(_BlurSize, settings.blurSize.value);
            mat.SetFloat(_Aspect, settings.roundness.value ? aspect : 1);

            var cmd = CommandBufferUtils.Get(context, nameof(RadialBlurPass));

            cmd.BlitColorDepth(ColorTarget, _ColorRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_ColorRT, ColorTarget, DepthTarget, DefaultMaterial, 0);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferUtils.Release( cmd);
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