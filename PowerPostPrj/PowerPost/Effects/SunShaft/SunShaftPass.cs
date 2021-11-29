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
        int COLOR_RT_ID = Shader.PropertyToID("_ColorRT");
        int COLOR_RT2_ID = Shader.PropertyToID("_ColorRT2");

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SunShaftSettings settings)
        {
            var sunPos = new Vector4(Mathf.Clamp01(settings.sunPos.value.x), Mathf.Clamp01(settings.sunPos.value.y), 0, settings.maxRadius.value);

            var mat = GetTargetMaterial(SHADER_NAME);
            var cmd = CommandBufferUtils.Get(context, nameof(SunShaftPass));

            mat.SetVector("_BlurRadius4", new Vector4(1, 1, 0, 0) * settings.sunShaftBlurRadius.value);
            mat.SetVector("_SunPos", sunPos);
            mat.SetVector("_SunThreshold", settings.sunThreshold.value);

            // 1 depth
            cmd.BlitColorDepth(ColorTarget, COLOR_RT_ID, DepthTarget, mat, DEPTH_PASS);
            // show pass 1
            //cmd.BlitColorDepth(COLOR_RT_ID, ColorTarget, DepthTarget, DefaultMaterial, 0);
            //context.ExecuteCommandBuffer(cmd);
            //return;

            // 2 radial blur
            var offsets = settings.sunShaftBlurRadius.value * (1f / 768f);
            mat.SetVector("_BlurRadius4", new Vector4(offsets, offsets, 0, 0));
            for (int i = 0; i < settings.radialBlurIterations.value; i++)
            {
                cmd.BlitColorDepth(COLOR_RT_ID, COLOR_RT2_ID, DepthTarget, mat, RADIAS_BLUR_PASS);

                offsets = settings.sunShaftBlurRadius.value * (i * 2 + 1) * 6 / 768f;
                mat.SetVector("_BlurRadius4", new Vector4(offsets, offsets, 0, 0));

                cmd.BlitColorDepth(COLOR_RT2_ID, COLOR_RT_ID, DepthTarget, mat, RADIAS_BLUR_PASS);

                offsets = settings.sunShaftBlurRadius.value * (i * 2 + 2) * 6 / 768f;
                mat.SetVector("_BlurRadius4", new Vector4(offsets, offsets, 0, 0));
            }

            // 3 composite
            var sunColor = Color.clear;
            if (sunPos.z >= 0)
            {
                sunColor = settings.sunColor.value * settings.sunShaftIntensity.value;
            }
            mat.SetVector("_SunColor", sunColor);

            cmd.SetGlobalTexture("_ColorRT", COLOR_RT_ID);

            cmd.BlitColorDepth(ColorTarget, COLOR_RT2_ID, DepthTarget, mat, SCREEN_PASS);
            cmd.BlitColorDepth(COLOR_RT2_ID, ColorTarget, DepthTarget, DefaultMaterial, 0);

            context.ExecuteCommandBuffer(cmd);

            CommandBufferUtils.Release(cmd);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            cmd.GetTemporaryRT(COLOR_RT_ID, cameraTextureDescriptor);
            cmd.GetTemporaryRT(COLOR_RT2_ID, cameraTextureDescriptor);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(COLOR_RT_ID);
            cmd.ReleaseTemporaryRT(COLOR_RT2_ID);
        }
    }
}