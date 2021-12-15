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
        int _ColorRT = Shader.PropertyToID("_ColorRT");

        const string TONE_MAPPING_SHADER = "Hidden/PowerPost/ToneMapping";

        void InitTextures(CommandBuffer cmd,RenderTextureDescriptor desc)
        {
            cmd.GetTemporaryRT(_ColorRT, desc);

        }

        void CleanTextures(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ColorRT);
        }

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, ToneMappingSettings settings)
        {
            var cmd = CommandBufferUtils.Get(context, nameof(ToneMappingPass));

            var desc = renderingData.cameraData.cameraTargetDescriptor;

            InitTextures(cmd, desc);
            var mat = GetTargetMaterial(TONE_MAPPING_SHADER);
            mat.SetInt("_Mode", (int)settings.mode.value);
            mat.SetFloat("_Weight", settings.weight.value);

            cmd.BlitColorDepth(ColorTarget, _ColorRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_ColorRT, ColorTarget, DepthTarget, DefaultMaterial, 0);

            context.ExecuteCommandBuffer(cmd);
            CleanTextures(cmd);
            CommandBufferUtils.Release(cmd);
        }
    }
}
