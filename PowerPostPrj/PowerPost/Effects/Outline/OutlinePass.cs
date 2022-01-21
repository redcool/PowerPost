namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class OutlinePass : BasePostExPass<OutlineSettings>
    {
        int _BlurTex = Shader.PropertyToID("_BlurTex");
        int _ColorTex = Shader.PropertyToID("_ColorTex");

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, OutlineSettings settings)
        {
            var outlineMat = GetTargetMaterial("Hidden/PowerPost/Outline");
            var cmd = CommandBufferUtils.Get(context,nameof(OutlinePass));

            var cam = renderingData.cameraData.camera;

            SetupTextures(cmd,cam,settings);

            var layer = settings.layer.value;
            if (layer != 0)
            {
                cmd.SetRenderTarget(_BlurTex);
                context.ExecuteCommandBuffer(cmd);
                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, null);

                cmd.SetRenderTarget(ColorTarget);
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();
            }
            cmd.SetGlobalTexture(_BlurTex, _BlurTex);
            cmd.BlitColorDepth(ColorTarget, _ColorTex, DepthTarget, outlineMat, 0);
            cmd.BlitColorDepth(_ColorTex, ColorTarget, DepthTarget, DefaultMaterialBlit, 0);

            context.ExecuteCommandBuffer(cmd);
            ReleaseTextures(cmd);
            CommandBufferUtils.Release(cmd);
        }

        private void ReleaseTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_BlurTex);
            cmd.ReleaseTemporaryRT(_ColorTex);
        }

        private void SetupTextures(CommandBuffer cmd,Camera cam,OutlineSettings settings)
        {
            var w = cam.pixelWidth >> settings.downSamples.value;
            var h = cam.pixelHeight >> settings.downSamples.value;
            cmd.GetTemporaryRT(_BlurTex, w, h,16,FilterMode.Bilinear);
            cmd.GetTemporaryRT(_ColorTex, cam.pixelWidth, cam.pixelHeight, 0, FilterMode.Bilinear);
        }
    }
}