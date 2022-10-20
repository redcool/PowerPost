namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class SunShaftPass : BasePostExPass<SunShaftSettings>
    {
        const int SCREEN_PASS = 0, RADIAS_BLUR_PASS = 1, DEPTH_PASS = 2;

        const string SHADER_NAME = "Hidden/PowerPost/SunShaft";
        // down size
        int _BlurRT = Shader.PropertyToID("_BlurRT");
        int _BlurRT2 = Shader.PropertyToID("_BlurRT2");

        public override string PassName => nameof(SunShaftPass);
        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SunShaftSettings settings,CommandBuffer cmd)
        {
            var sunPos = new Vector4(Mathf.Clamp01(settings.sunPos.value.x), Mathf.Clamp01(settings.sunPos.value.y), 0, settings.maxRadius.value);
            var cam = renderingData.cameraData.camera;
            var sun = RenderSettings.sun;
            if (sun)
            {
                if (settings.useRenderSettingsSun.value)
                {
                    sunPos = cam.WorldToViewportPoint(sun.transform.position);
                    sunPos.w = settings.maxRadius.value;
                }
                if (settings.hiddenSunShaftBackfaceSun.value)
                {
                    var cos = Vector3.Dot(cam.transform.forward, sun.transform.forward);
                    if (cos > 0)
                        return;
                }
            }

            var mat = GetTargetMaterial(SHADER_NAME);
            InitTextures(cmd, renderingData.cameraData.cameraTargetDescriptor);

            mat.SetVector("_BlurRadius4", new Vector4(1, 1, 0, 0) * settings.sunShaftBlurRadius.value);
            mat.SetVector("_SunPos", sunPos);
            mat.SetVector("_SunThreshold", settings.sunThreshold.value);

            // 1 depth
            cmd.BlitColorDepth(ColorTarget, _BlurRT, _BlurRT, mat, DEPTH_PASS);

            // 2 radial blur
            const float DELTA = 0.0078f;
            var offsets = settings.sunShaftBlurRadius.value * DELTA;
            mat.SetVector("_BlurRadius4", new Vector4(offsets, offsets, 0, 0));
            for (int i = 0; i < settings.radialBlurIterations.value; i++)
            {
                cmd.BlitColorDepth(_BlurRT, _BlurRT2, _BlurRT2, mat, RADIAS_BLUR_PASS);

                offsets = settings.sunShaftBlurRadius.value * (i * 2 + 1) * DELTA;
                mat.SetVector("_BlurRadius4", new Vector4(offsets, offsets, 0, 0));

                cmd.BlitColorDepth(_BlurRT2, _BlurRT, _BlurRT, mat, RADIAS_BLUR_PASS);

                offsets = settings.sunShaftBlurRadius.value * (i * 2 + 2) * DELTA;
                mat.SetVector("_BlurRadius4", new Vector4(offsets, offsets, 0, 0));
            }

            // 3 composite
            var sunColor = Color.clear;
            if (sunPos.z >= 0)
            {
                sunColor = settings.sunColor.value * settings.sunShaftIntensity.value;
            }
            mat.SetVector("_SunColor", sunColor);

            cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture, ColorTarget, ColorTarget, mat, SCREEN_PASS);

            CleanupTextures(cmd);
        }

        void InitTextures(CommandBuffer cmd, RenderTextureDescriptor desc)
        {
            var w = desc.width >> 2;
            var h = desc.height >> 2;
            cmd.GetTemporaryRT(_BlurRT, w, h, 16, FilterMode.Bilinear);
            cmd.GetTemporaryRT(_BlurRT2, w,h,16, FilterMode.Bilinear);
        }

        void CleanupTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_BlurRT);
            cmd.ReleaseTemporaryRT(_BlurRT2);
        }
    }
}