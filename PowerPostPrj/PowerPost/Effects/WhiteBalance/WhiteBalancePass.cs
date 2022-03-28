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

        int _ColorRT = Shader.PropertyToID("_ColorRT");

        const string SHADER_NAME = "Hidden/PowerPost/WhiteBalance";

        void InitTextures(CommandBuffer cmd, RenderTextureDescriptor desc)
        {
            cmd.GetTemporaryRT(_ColorRT, desc);

        }

        void CleanTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ColorRT);
        }

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, WhiteBalanceSettings settings)
        {
            var mat = GetTargetMaterial(SHADER_NAME);

            var cmd = CommandBufferUtils.Get(context, nameof(WhiteBalancePass));
            InitTextures(cmd,renderingData.cameraData.cameraTargetDescriptor);

            mat.SetVector(_ColorBalance,PostUtils.GetColorBalanceCoeffs(settings.temperature.value,settings.tint.value));

            cmd.BlitColorDepth(ColorTarget, _ColorRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_ColorRT, ColorTarget, DepthTarget, DefaultMaterialBlit, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferUtils.Release(cmd);
            cmd.Clear();
        }
    }
}
