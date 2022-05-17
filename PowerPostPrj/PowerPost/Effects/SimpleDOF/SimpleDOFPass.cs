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

    public class SimpleDOFPass : BasePostExPass<SimpleDOFSettings>
    {
        static int _ColorRT = Shader.PropertyToID("_ColorRT");
        static int _DepthRT = Shader.PropertyToID("_DepthRT");
        static int _BlurRT = Shader.PropertyToID("_BlurRT");

        static int _Distance = Shader.PropertyToID("_Distance");
        static int _BlurSize = Shader.PropertyToID("_BlurSize");
        static int _DepthRange = Shader.PropertyToID("_DepthRange");
        static int _Debug = Shader.PropertyToID("_Debug");

        const string SHADER_NAME = "Hidden/PowerPost/SimpleDOF";

        public override string PassName => nameof(SimpleDOFPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SimpleDOFSettings settings,CommandBuffer cmd)
        {
            var mat = GetTargetMaterial(SHADER_NAME);

            cmd.BlitColorDepth(DepthTarget, _DepthRT, _DepthRT, DefaultBlitMaterial, 0);
            cmd.SetGlobalTexture(_DepthRT, _DepthRT);

            mat.SetFloat(_Distance, settings.distance.value);
            mat.SetFloat(_BlurSize, settings.blurSize.value);
            mat.SetFloat(_DepthRange, settings.depthRange.value);
            mat.SetFloat(_Debug, settings.debugMode.value ? 1 : 0);

            cmd.BlitColorDepth(ColorTarget, _BlurRT, _BlurRT, DefaultBlitMaterial, 0);

            cmd.BlitColorDepth(_BlurRT, _ColorRT, _ColorRT, mat, 0);
            cmd.BlitColorDepth(_ColorRT, ColorTarget, DepthTarget, DefaultBlitMaterial, 0);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor rtDesc)
        {
            //var settings = GetSettings();
            //var downsample = settings.downsample.value;

            cmd.GetTemporaryRT(_ColorRT, rtDesc);
            //cmd.GetTemporaryRT(COLOR_RT_ID, rtDesc.width / downsample, rtDesc.height / downsample, 0, FilterMode.Bilinear, rtDesc.colorFormat);
            cmd.GetTemporaryRT(_DepthRT, rtDesc.width, rtDesc.height,16,FilterMode.Bilinear,RenderTextureFormat.R16);

            var w = rtDesc.width >> 1;
            var h = rtDesc.height >> 1;
            cmd.GetTemporaryRT(_BlurRT, w, h, 16, FilterMode.Bilinear);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_ColorRT);
            cmd.ReleaseTemporaryRT(_DepthRT);
            cmd.ReleaseTemporaryRT(_BlurRT);
        }
    }
}
