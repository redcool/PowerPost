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
        int
            _ColorRT = Shader.PropertyToID(nameof(_ColorRT)),
            _Mode = Shader.PropertyToID(nameof(_Mode)),
            _Weight = Shader.PropertyToID(nameof(_Weight)),

            _Saturate = Shader.PropertyToID(nameof(_Saturate)),
            _Brightness = Shader.PropertyToID(nameof(_Brightness)),
            _Hue = Shader.PropertyToID(nameof(_Hue)),
            _Scale = Shader.PropertyToID(nameof(_Scale)),
            _Offset = Shader.PropertyToID(nameof(_Offset)),

            _UseColorGradingLUT = Shader.PropertyToID(nameof(_UseColorGradingLUT)),
            _ColorGradingLUT = Shader.PropertyToID(nameof(_ColorGradingLUT)),
            _ColorGradingLUTParams = Shader.PropertyToID(nameof(_ColorGradingLUTParams)),
            _ColorGradingUseLogC = Shader.PropertyToID(nameof(_ColorGradingUseLogC))
            ;

        const string TONE_MAPPING_SHADER = "Hidden/PowerPost/ToneMapping";

        public override string PassName => nameof(ToneMappingPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, ToneMappingSettings settings,CommandBuffer cmd)
        {
            var mat = GetTargetMaterial(TONE_MAPPING_SHADER);
            mat.SetInt(_Mode, (int)settings.mode.value);
            mat.SetFloat(_Weight, settings.weight.value);
            mat.SetFloat(_Saturate, settings.saturate.value);
            mat.SetFloat(_Brightness, settings.brightness.value);
            //mat.SetFloat(_Hue, settings.hue.value);
            mat.SetFloat(_Scale, settings.scale.value);
            mat.SetFloat(_Offset, settings.offset.value);


            var gradingLut = settings.colorGradingLut.value;
            if (gradingLut)
            {
                cmd.SetGlobalVector(_ColorGradingLUTParams, new Vector4(1f/gradingLut.width, 1f/gradingLut.height, gradingLut.height-1));
            }
            cmd.SetGlobalTexture(_ColorGradingLUT,settings.colorGradingLut.value);
            cmd.SetGlobalFloat(_UseColorGradingLUT, gradingLut ? 1 : 0);
            cmd.SetGlobalFloat(_ColorGradingUseLogC, settings.colorGradingUseLogC.value ? 1 : 0);


            cmd.BlitColorDepth(sourceTex,targetTex,targetTex, mat, 0);
        }
    }
}
