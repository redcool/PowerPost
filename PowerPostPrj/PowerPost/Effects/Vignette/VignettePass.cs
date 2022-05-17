using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public class VignettePass : BasePostExPass<VignetteSettings>
    {
        int _Falloff = Shader.PropertyToID(nameof(_Falloff));
        int _Aspect = Shader.PropertyToID(nameof(_Aspect));
        int _Center = Shader.PropertyToID(nameof(_Center));
        int _Intensity = Shader.PropertyToID(nameof(_Intensity));
        int _Smoothness = Shader.PropertyToID(nameof(_Smoothness));
        int _Oval = Shader.PropertyToID(nameof(_Oval));
        int _VignetteColor = Shader.PropertyToID(nameof(_VignetteColor));

        int _ColorTex = Shader.PropertyToID(nameof(_ColorTex));

        public override string PassName => nameof(VignettePass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, VignetteSettings settings, CommandBuffer cmd)
        {
            ref var cam = ref renderingData.cameraData.camera;

            var mat = GetTargetMaterial("Hidden/PowerPost/Vignette");
            mat.SetFloat(_Aspect,settings.rounded.value ? (float)cam.pixelWidth/cam.pixelHeight : 1);
            mat.SetFloat(_Intensity, settings.intensity.value);
            mat.SetFloat(_Smoothness, settings.smoothness.value);
            mat.SetVector(_Oval, new Vector4(settings.ovalX.value, settings.ovalY.value));
            mat.SetVector(_Center, new Vector4(settings.centerX.value, settings.centerY.value));
            mat.SetColor(_VignetteColor, settings.color.value);

            InitTextures(ref renderingData.cameraData.cameraTargetDescriptor,cmd);

            cmd.BlitColorDepth(ColorTarget, _ColorTex, _ColorTex, mat, 0);
            cmd.BlitColorDepth(_ColorTex, ColorTarget, ColorTarget, DefaultBlitMaterial, 0);

            CleanTextures(cmd);
        }

        private void CleanTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ColorTex);
        }

        private void InitTextures(ref RenderTextureDescriptor desc, CommandBuffer cmd)
        {
            cmd.GetTemporaryRT(_ColorTex, desc);
        }
    }
}
