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
        static int COLOR_RT_ID = Shader.PropertyToID("_ColorRT");
        static int DEPTH_RT_ID = Shader.PropertyToID("_DepthRT");
        static int DISTANCE_ID = Shader.PropertyToID("_Distance");
        static int BLUR_SIZE_ID = Shader.PropertyToID("_BlurSize");
        static int DEPTH_RANGE_ID = Shader.PropertyToID("_DepthRange");
        static int DEBUG_ID = Shader.PropertyToID("_Debug");
        const string SHADER_NAME = "Hidden/PowerPost/SimpleDOF";

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SimpleDOFSettings settings)
        {
            var mat = GetTargetMaterial(SHADER_NAME);
            var cmd = CommandBufferUtils.Get(context, nameof(SimpleDOFPass));

            cmd.BlitColorDepth(DepthTarget, DEPTH_RT_ID, DEPTH_RT_ID, DefaultMaterial, 0);
            cmd.SetGlobalTexture(DEPTH_RT_ID, DEPTH_RT_ID);

            mat.SetFloat(DISTANCE_ID, settings.distance.value);
            mat.SetFloat(BLUR_SIZE_ID, settings.blurSize.value);
            mat.SetFloat(DEPTH_RANGE_ID, settings.depthRange.value);
            mat.SetFloat(DEBUG_ID, settings.debugMode.value ? 1 : 0);

            cmd.BlitColorDepth(ColorTarget, COLOR_RT_ID, DepthTarget, mat, 0);
            cmd.BlitColorDepth(COLOR_RT_ID, ColorTarget, DepthTarget, DefaultMaterial, 0);
            context.ExecuteCommandBuffer(cmd);

            CommandBufferUtils.Release(cmd);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor rtDesc)
        {
            //var settings = GetSettings();
            //var downsample = settings.downsample.value;

            cmd.GetTemporaryRT(COLOR_RT_ID, rtDesc);
            //cmd.GetTemporaryRT(COLOR_RT_ID, rtDesc.width / downsample, rtDesc.height / downsample, 0, FilterMode.Bilinear, rtDesc.colorFormat);
            cmd.GetTemporaryRT(DEPTH_RT_ID, rtDesc.width, rtDesc.height,16,FilterMode.Bilinear,RenderTextureFormat.R16);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(COLOR_RT_ID);
            cmd.ReleaseTemporaryRT(DEPTH_RT_ID);
        }
    }
}
