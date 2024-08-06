namespace PowerPost {
    using PowerUtilities;
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
            var mat = GetTargetMaterial("Hidden/PowerPost/Outline");
            mat.SetFloat("_OutlineWidth", settings.outlineWidth.value);
            mat.SetColor("_OutlineColor", settings.outlineColor.value);
            mat.SetFloat("_Smoothness",settings.smoothness.value);

            var cam = renderingData.cameraData.camera;

            var layer = settings.layer.value;

            var isCreateTex = layer > 0 || settings.downSamples.value > 0;
            if (isCreateTex)
                SetupTextures(cmd, cam, settings);


            if (layer > 0)
            {
                DrawLayeredRenderers(ref context, ref renderingData, cmd, layer);
            }
            else
            {
                BlitTargetDepthTex(settings, cmd);
            }

            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, mat);

        }

        private void BlitTargetDepthTex(OutlineSettings settings, CommandBuffer cmd)
        {
            if (settings.downSamples.value > 0)
            {
                cmd.BlitColorDepth(ShaderPropertyIdentifier._CameraDepthAttachment, _DepthTex, _DepthTex, DefaultBlitMaterial, 1);
            }
            else
            {
                cmd.SetGlobalTexture(_DepthTex, ShaderPropertyIdentifier._CameraDepthTexture);
            }
        }

        private void DrawLayeredRenderers(ref ScriptableRenderContext context, ref RenderingData renderingData, CommandBuffer cmd, LayerMask layer)
        {
            cmd.SetRenderTarget(_DepthTex);
            cmd.ClearRenderTarget(true, false, Color.clear);
            cmd.Execute(ref context);

            GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, null);

            cmd.SetRenderTarget(ColorTarget);
            cmd.Execute(ref context);
        }

        private void SetupTextures(CommandBuffer cmd,Camera cam,OutlineSettings settings)
        {
            var w = cam.pixelWidth >> settings.downSamples.value;
            var h = cam.pixelHeight >> settings.downSamples.value;
            cmd.GetTemporaryRT(_DepthTex, w, h,16,FilterMode.Point, RenderTextureFormat.Depth);
        }
    }
}