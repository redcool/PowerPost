namespace PowerPost
{
    using PowerUtilities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class SimpleDOFPass : BasePostExPass<SimpleDOFSettings>
    {
        static int _BlurRT = Shader.PropertyToID("_BlurRT");

        static int _Distance = Shader.PropertyToID("_Distance");
        static int _BlurSize = Shader.PropertyToID("_BlurSize");
        static int _DepthRange = Shader.PropertyToID("_DepthRange");

        const string SHADER_NAME = "Hidden/PowerPost/SimpleDOF";

        public override string PassName => nameof(SimpleDOFPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SimpleDOFSettings settings,CommandBuffer cmd)
        {
            var mat = GetTargetMaterial(SHADER_NAME);

            mat.SetFloat(_Distance, settings.distance.value);
            mat.SetFloat(_BlurSize, settings.blurSize.value);
            mat.SetFloat(_DepthRange, settings.depthRange.value);

            cmd.BlitColorDepth(ColorTarget, _BlurRT, _BlurRT, DefaultBlitMaterial, 0);
            cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture, ColorTarget, ColorTarget, mat, 0);

            mat.SetKeyword(ShaderPropertyIds._DEBUG, settings.debugMode.value);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor rtDesc)
        {
            var w = rtDesc.width >> 1;
            var h = rtDesc.height >> 1;
            cmd.GetTemporaryRT(_BlurRT, w, h, 16, FilterMode.Bilinear);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_BlurRT);
        }
    }
}
