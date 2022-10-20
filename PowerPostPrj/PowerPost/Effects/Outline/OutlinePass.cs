namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class OutlinePass : BasePostExPass<OutlineSettings>
    {
        int _DepthTex = Shader.PropertyToID("_DepthTex");

        public override string PassName => nameof(OutlinePass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, OutlineSettings settings,CommandBuffer cmd)
        {
            var outlineMat = GetTargetMaterial("Hidden/PowerPost/Outline");
            outlineMat.SetFloat("_OutlineWidth", settings.outlineWidth.value);
            outlineMat.SetColor("_OutlineColor", settings.outlineColor.value);

            var cam = renderingData.cameraData.camera;

            SetupTextures(cmd, cam, settings);

            var layer = settings.layer.value;
            if (layer != 0 && layer != -1)
            {
                cmd.SetRenderTarget(_DepthTex);
                context.ExecuteCommandBuffer(cmd);
                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, null);

                cmd.SetRenderTarget(ColorTarget);
                context.ExecuteCommandBuffer(cmd);
            }
            else if (layer == -1)
            {
                cmd.BlitColorDepth(ShaderPropertyIds._CameraDepthTexture, _DepthTex, _DepthTex, DefaultBlitMaterial, 0);
            }

            cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture, ColorTarget, ColorTarget, outlineMat, 0);

            ReleaseTextures(cmd);
        }

        private void ReleaseTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_DepthTex);
        }

        private void SetupTextures(CommandBuffer cmd,Camera cam,OutlineSettings settings)
        {
            var w = cam.pixelWidth >> settings.downSamples.value;
            var h = cam.pixelHeight >> settings.downSamples.value;
            cmd.GetTemporaryRT(_DepthTex, w, h,16,FilterMode.Bilinear, RenderTextureFormat.Default);
        }
    }
}