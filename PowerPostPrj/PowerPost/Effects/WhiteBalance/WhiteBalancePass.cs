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
    public class WhiteBalancePass : BasePostExPass<WhiteBalanceSettings>
    {
        int _ColorBalance = Shader.PropertyToID(nameof(_ColorBalance));

        const string SHADER_NAME = "Hidden/PowerPost/WhiteBalance";

        public override string PassName => nameof(WhiteBalancePass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, WhiteBalanceSettings settings,CommandBuffer cmd)
        {
            var mat = GetTargetMaterial(SHADER_NAME);

            mat.SetVector(_ColorBalance,PostUtils.GetColorBalanceCoeffs(settings.temperature.value,settings.tint.value));
            cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture, ColorTarget, ColorTarget, mat, 0);
        }
    }
}
