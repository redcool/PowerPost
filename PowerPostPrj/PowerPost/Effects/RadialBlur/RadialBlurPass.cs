using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    public class RadialBlurPass : BasePostExPass<RadialBlurSettings>
    {
        const string SHADER_PATH = "Hidden/PowerPost/RadialBlur";
        static int _Center = Shader.PropertyToID("_Center");
        static int _RadiusMin = Shader.PropertyToID("_RadiusMin");
        static int _RadiusMax = Shader.PropertyToID("_RadiusMax");
        static int _BlurSize = Shader.PropertyToID("_BlurSize");
        static int _Aspect = Shader.PropertyToID("_Aspect");

        static int _BlurRT = Shader.PropertyToID("_BlurRT");
        static int _ResultRT = Shader.PropertyToID("_ResultRT");

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, RadialBlurSettings settings)
        {
            var cam = renderingData.cameraData.camera;
            var aspect = cam.pixelWidth / (float)cam.pixelHeight;

            var mat = GetTargetMaterial(SHADER_PATH);

            mat.SetVector(_Center, settings.center.value);
            mat.SetFloat(_RadiusMin, settings.radiusMin.value);
            mat.SetFloat(_RadiusMax, settings.radiusMax.value);

            mat.SetFloat(_BlurSize, settings.blurSize.value);
            mat.SetFloat(_Aspect, settings.roundness.value ? aspect : 1);

            var cmd = CommandBufferUtils.Get(context, nameof(RadialBlurPass));

            // blur 
            cmd.BlitColorDepth(ColorTarget, _BlurRT, _BlurRT, DefaultMaterialBlit, 0);


            cmd.BlitColorDepth(ColorTarget, _ResultRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_ResultRT, ColorTarget, DepthTarget, DefaultMaterialBlit, 0);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferUtils.Release( cmd);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            var w = cameraTextureDescriptor.width >> 3;
            var h = cameraTextureDescriptor.height >> 3;
            cmd.GetTemporaryRT(_BlurRT, w, h, 16, FilterMode.Bilinear);
            cmd.GetTemporaryRT(_ResultRT,cameraTextureDescriptor);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(_BlurRT);
            cmd.ReleaseTemporaryRT(_ResultRT);
        }
    }
}