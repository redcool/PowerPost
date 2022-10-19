namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class SimpleBloomPass : BasePostExPass<SimpleBloomSettings>
    {
        const int GRAB_ILLUM_PASS = 0;
        const int BOX_DOWN = 1;
        const int BOX_UP = 2;
        const int COMBINE_PASS = 3;
        const string SHADER_NAME = "Hidden/PowerPost/SimpleBloom";

        RenderTexture[] textures = new RenderTexture[16];
        RenderTextureFormat rtFormat;

        public override string PassName => nameof(SimpleBloomPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SimpleBloomSettings settings,CommandBuffer cmd)
        {
            var mat = GetTargetMaterial(SHADER_NAME);

            var knee = settings.threshold.value * settings.softThreshold.value;
            Vector4 filter;
            filter.x = settings.threshold.value;
            filter.y = filter.x - knee;
            filter.z = 2 * knee;
            filter.w = 0.25f / (knee + 0.0001f);
            mat.SetVector("_Filter", filter);

            var cam = renderingData.cameraData.camera;
            var w = cam.pixelWidth / 2;
            var h = cam.pixelHeight / 2;

            // pass 0
            var buffer0 = textures[0] = RenderTexture.GetTemporary(w, h, 0, rtFormat);

            cmd.BlitColorDepth(ColorTarget, buffer0, buffer0, mat, GRAB_ILLUM_PASS);

            //pass 1,downsample
            int i = 1;
            for (i = 1; i < settings.iterators.value; i++)
            {
                w /= 2;
                h /= 2;
                if (h < 2)
                    break;

                //blur1
                var buffer1 = textures[i] = RenderTexture.GetTemporary(w, h, 0, rtFormat);
                cmd.BlitColorDepth(buffer0, buffer1, buffer1, mat, BOX_DOWN);

                buffer0 = buffer1;
            }

            // pass 2 upsample
            var lastId = i - 1;
            for (i -= 2; i >= 0; i--)
            {
                var buffer1 = textures[i];
                if (lastId < textures.Length)
                {
                    mat.SetTexture("_BloomTex", textures[lastId]);
                    lastId--;
                }
                cmd.BlitColorDepth(buffer0, buffer1, buffer1, mat, BOX_UP);
                buffer0 = buffer1;
            }
            cmd.BlitColorDepth(buffer0, ColorTarget, ColorTarget, mat, BOX_UP);

            //pass 3
            mat.SetFloat("_Intensity", Mathf.GammaToLinearSpace(settings.intensity.value));
            //mat.SetFloat("_SmoothBorder", Mathf.GammaToLinearSpace(settings.smoothBorder.value));
            mat.SetTexture("_BloomTex", buffer0);
            mat.SetColor("_BloomColor", settings.bloomColor.value);

            cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture, ColorTarget, ColorTarget, mat, COMBINE_PASS);

            for (i = 0; i < settings.iterators.value; i++)
            {
                RenderTexture.ReleaseTemporary(textures[i]);
            }
        }

    }
}