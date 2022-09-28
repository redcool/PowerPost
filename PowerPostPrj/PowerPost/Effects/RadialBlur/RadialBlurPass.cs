using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using PowerUtilities;

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

        //radial tex
        static int _RadialTex = Shader.PropertyToID(nameof(_RadialTex));
        static int _RadialInfo = Shader.PropertyToID(nameof(_RadialInfo));
        static int _NoiseMap = Shader.PropertyToID(nameof(_NoiseMap));
        static int _NoiseMapST = Shader.PropertyToID(nameof(_NoiseMapST));
        static int _GrayScale = Shader.PropertyToID(nameof(_GrayScale));

        const string RADIAL_TEX_ON = nameof(RADIAL_TEX_ON);
        const string _GRAY_SCALE_ON = nameof(_GRAY_SCALE_ON);
        const string _NOISE_MAP_ON = nameof(_NOISE_MAP_ON);

        public override string PassName => nameof(RadialBlurPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, RadialBlurSettings settings, CommandBuffer cmd)
        {
            var cam = renderingData.cameraData.camera;
            var aspect = cam.pixelWidth / (float)cam.pixelHeight;

            var mat = GetTargetMaterial(SHADER_PATH);

            mat.SetVector(_Center, settings.center.value);
            mat.SetFloat(_RadiusMin, settings.radiusMin.value);
            mat.SetFloat(_RadiusMax, settings.radiusMax.value);

            mat.SetFloat(_BlurSize, settings.blurSize.value);
            mat.SetFloat(_Aspect, settings.roundness.value ? aspect : 1);

            // blur 
            cmd.BlitColorDepth(ColorTarget, _BlurRT, _BlurRT, DefaultBlitMaterial, 0);


            cmd.BlitColorDepth(ColorTarget, _ResultRT, DepthTarget, mat, 0);
            cmd.BlitColorDepth(_ResultRT, ColorTarget, DepthTarget, DefaultBlitMaterial, 0);

            // radial tex
            if (settings.radialTexOn.value)
            {
                mat.EnableKeyword(RADIAL_TEX_ON);
                mat.SetTexture(_RadialTex, settings.radialTex.value);
                mat.SetVector(_RadialInfo, new Vector4(settings.radialScale.value.x,
                    settings.radialScale.value.y,
                    settings.radialLength.value,
                    settings.noiseMapScale.value
                    ));

                if (settings.noiseMapOn.value)
                {
                    mat.SetTexture(_NoiseMap, settings.noiseMap.value);
                    mat.SetVector(_NoiseMapST,settings.noiseMapST.value);
                }
            }

            if (settings.isGrayScale.value)
            {
                mat.SetFloat(_GrayScale,settings.grayScale.value);
            }

            mat.SetKeyword(RADIAL_TEX_ON, settings.radialTexOn.value);
            mat.SetKeyword(_GRAY_SCALE_ON, settings.isGrayScale.value);
            mat.SetKeyword(_NOISE_MAP_ON, settings.noiseMapOn.value);
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