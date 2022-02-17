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
        int _ColorTex = Shader.PropertyToID("_ColorTex");
        int _BlurColorTex = Shader.PropertyToID("_BlurColorTex");

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, OutlineSettings settings)
        {
            var outlineMat = GetTargetMaterial("Hidden/PowerPost/Outline");
            outlineMat.SetFloat("_OutlineWidth", settings.outlineWidth.value);
            outlineMat.SetColor("_OutlineColor", settings.outlineColor.value);

            var cmd = CommandBufferUtils.Get(context, nameof(OutlinePass));

            var cam = renderingData.cameraData.camera;

            SetupTextures(cmd, cam, settings);

            var layer = settings.layer.value;
            if (layer != 0 && layer != -1)
            {
                cmd.SetRenderTarget(_BlurColorTex);
                context.ExecuteCommandBuffer(cmd);
                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, null);

                cmd.SetRenderTarget(ColorTarget);
                context.ExecuteCommandBuffer(cmd);

                cmd.BlitColorDepth(_BlurColorTex, _DepthTex, _DepthTex, DefaultMaterialBlit, 0);
            }
            else if (layer == -1)
            {
                cmd.BlitColorDepth(DepthTarget, _DepthTex, _DepthTex, DefaultMaterialBlit, 0);
            }

            cmd.SetGlobalTexture(_BlurColorTex, _BlurColorTex);
            cmd.SetGlobalTexture(_DepthTex, _DepthTex);


            cmd.BlitColorDepth(ColorTarget, _ColorTex, _ColorTex, outlineMat, 0);
            cmd.BlitColorDepth(_ColorTex, ColorTarget, DepthTarget, DefaultMaterialBlit, 0);

            context.ExecuteCommandBuffer(cmd);
            ReleaseTextures(cmd);
            CommandBufferUtils.Release(cmd);
        }

        private void ReleaseTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_DepthTex);
            cmd.ReleaseTemporaryRT(_ColorTex);
        }

        private void SetupTextures(CommandBuffer cmd,Camera cam,OutlineSettings settings)
        {
            var w = cam.pixelWidth >> settings.downSamples.value;
            var h = cam.pixelHeight >> settings.downSamples.value;
            cmd.GetTemporaryRT(_DepthTex, w, h,0,FilterMode.Bilinear, RenderTextureFormat.Default);
            cmd.GetTemporaryRT(_BlurColorTex, w, h, 16, FilterMode.Bilinear, RenderTextureFormat.Default);

            cmd.GetTemporaryRT(_ColorTex, cam.pixelWidth, cam.pixelHeight, 0, FilterMode.Bilinear);
        }
    }
}