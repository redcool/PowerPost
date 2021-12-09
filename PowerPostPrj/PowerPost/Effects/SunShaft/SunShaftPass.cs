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
        // full size
        int _ResultTex = Shader.PropertyToID("_ResultTex");
        

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SunShaftSettings settings)
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
            var cmd = CommandBufferUtils.Get(context, nameof(SunShaftPass));
            InitTextures(cmd, renderingData.cameraData.cameraTargetDescriptor);

            mat.SetVector("_BlurRadius4", new Vector4(1, 1, 0, 0) * settings.sunShaftBlurRadius.value);
            mat.SetVector("_SunPos", sunPos);
            mat.SetVector("_SunThreshold", settings.sunThreshold.value);

            // 1 depth
            cmd.BlitColorDepth(ColorTarget, _BlurRT, _BlurRT, mat, DEPTH_PASS);
            //show pass 1
            //cmd.BlitColorDepth(_BlurRT, ColorTarget, DepthTarget, DefaultMaterial, 0);
            //context.ExecuteCommandBuffer(cmd);
            //return;

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

            //cmd.BlitColorDepth(_ShaftTex, ColorTarget, DepthTarget, DefaultMaterial, 0);
            //context.ExecuteCommandBuffer(cmd);
            //return;
            // 3 composite
            var sunColor = Color.clear;
            if (sunPos.z >= 0)
            {
                sunColor = settings.sunColor.value * settings.sunShaftIntensity.value;
            }
            mat.SetVector("_SunColor", sunColor);

            cmd.BlitColorDepth(ColorTarget, _ResultTex, DepthTarget, mat, SCREEN_PASS);
            cmd.BlitColorDepth(_ResultTex, ColorTarget, DepthTarget, DefaultMaterial, 0);

            CleanupTextures(cmd);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferUtils.Release(cmd);
        }

        void InitTextures(CommandBuffer cmd, RenderTextureDescriptor desc)
        {
            var w = desc.width >> 2;
            var h = desc.height >> 2;
            cmd.GetTemporaryRT(_BlurRT, w, h, 16, FilterMode.Bilinear);
            cmd.GetTemporaryRT(_BlurRT2, w,h,16, FilterMode.Bilinear);
            cmd.GetTemporaryRT(_ResultTex, desc);
        }

        void CleanupTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_BlurRT);
            cmd.ReleaseTemporaryRT(_BlurRT2);
            cmd.ReleaseTemporaryRT(_ResultTex);
        }
    }
}