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

            var cam = renderingData.cameraData.camera;

            var layer = settings.layer.value;
            if (layer > 0)
            {
                SetupTextures(cmd, cam, settings);

                cmd.SetRenderTarget(_DepthTex);
                cmd.ClearRenderTarget(true, false, Color.clear);
                cmd.Execute(ref context);

                GraphicsUtils.DrawRenderers(context, ref renderingData, cmd, layer, null);

                cmd.SetRenderTarget(ColorTarget);
                cmd.Execute(ref context);
            }
            else
            {
                cmd.SetGlobalTexture(_DepthTex, ShaderPropertyIds.GetURPRTID(nameof(ShaderPropertyIds._CameraDepthTexture)));
            }

            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, mat);

        }

        private void SetupTextures(CommandBuffer cmd,Camera cam,OutlineSettings settings)
        {
            var w = cam.pixelWidth >> settings.downSamples.value;
            var h = cam.pixelHeight >> settings.downSamples.value;
            cmd.GetTemporaryRT(_DepthTex, w, h,16,FilterMode.Point, RenderTextureFormat.Depth);
        }
    }
}