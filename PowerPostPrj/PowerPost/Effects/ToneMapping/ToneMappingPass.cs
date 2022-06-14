namespace PowerPost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class ToneMappingPass : BasePostExPass<ToneMappingSettings>
    {
        int _ColorRT = Shader.PropertyToID(nameof(_ColorRT));
        int _Mode = Shader.PropertyToID(nameof(_Mode));
        int _Weight = Shader.PropertyToID(nameof(_Weight));

        int _Saturate = Shader.PropertyToID(nameof(_Saturate));
        int _Brightness = Shader.PropertyToID(nameof(_Brightness));
        int _Hue = Shader.PropertyToID(nameof(_Hue));
        int _Scale = Shader.PropertyToID(nameof(_Scale));
        int _Offset = Shader.PropertyToID(nameof(_Offset));

        const string TONE_MAPPING_SHADER = "Hidden/PowerPost/ToneMapping";

        void InitTextures(CommandBuffer cmd, RenderTextureDescriptor desc)
        {
            cmd.GetTemporaryRT(_ColorRT, desc);

        }

        void CleanTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ColorRT);
        }

        public override string PassName => nameof(ToneMappingPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, ToneMappingSettings settings,CommandBuffer cmd)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;

            InitTextures(cmd, desc);
            var mat = GetTargetMaterial(TONE_MAPPING_SHADER);
            mat.SetInt(_Mode, (int)settings.mode.value);
            mat.SetFloat(_Weight, settings.weight.value);
            mat.SetFloat(_Saturate, settings.saturate.value);
            mat.SetFloat(_Brightness, settings.brightness.value);
            //mat.SetFloat(_Hue, settings.hue.value);
            mat.SetFloat(_Scale, settings.scale.value);
            mat.SetFloat(_Offset, settings.offset.value);

            cmd.BlitColorDepth(ColorTarget, _ColorRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_ColorRT, ColorTarget, DepthTarget, DefaultBlitMaterial, 0);

            CleanTextures(cmd);
        }
    }
}
