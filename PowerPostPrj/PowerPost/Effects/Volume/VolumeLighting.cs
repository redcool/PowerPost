namespace PowerPost
{
    using PowerUtilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class VolumeLighting : BasePostExPass<VolumeLightingSettings>
    {
        public override string PassName => nameof(VolumeLighting);

        readonly int
            _StepCount = Shader.PropertyToID(nameof(_StepCount)),
            _BlurSize = Shader.PropertyToID(nameof(_BlurSize)),
            _ReverseLight = Shader.PropertyToID(nameof(_ReverseLight)),

            _BlurTexA = Shader.PropertyToID(nameof(_BlurTexA)),
            _BlurTexB = Shader.PropertyToID(nameof(_BlurTexB)),
            _LightTex = Shader.PropertyToID(nameof(_LightTex)),
            _Color = Shader.PropertyToID(nameof(_Color))
        ;

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, VolumeLightingSettings settings, CommandBuffer cmd)
        {
            ref var cameraData = ref renderingData.cameraData;
            var width = cameraData.cameraTargetDescriptor.width >> settings.downSampers.value;
            var height = cameraData.cameraTargetDescriptor.height >> settings.downSampers.value;

            cmd.GetTemporaryRT(_BlurTexA, width, height, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(_BlurTexB, width, height, 0, FilterMode.Bilinear);

            var mat = GetTargetMaterial("Hidden/PowerPost/VolumeLight");
            mat.SetInt(_StepCount, settings.stepCount.value);

            // light pass
            cmd.BlitColorDepth(sourceTex, _BlurTexA, _BlurTexA, mat, 0);

            //cmd.BlitColorDepth(_BlurTexA, targetTex, targetTex, DefaultBlitMaterial);
            //return;

            //1,3,5,7
            // blur pass
            for (int i = 0; i < settings.iterators.value; i++)
            {
                mat.SetFloat(_BlurSize, (i*2)*2+1);
                cmd.BlitColorDepth(_BlurTexA, _BlurTexB, _BlurTexB, mat,1);

                mat.SetFloat(_BlurSize,((i*2)*2+3));
                cmd.BlitColorDepth(_BlurTexB, _BlurTexA, _BlurTexA, mat, 1);
            }
            // composite pass
            mat.SetFloat(_ReverseLight,settings.reverseLight.value?1:0);
            mat.SetColor(_Color, settings.color.value);

            cmd.SetGlobalTexture(_LightTex, _BlurTexA);
            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, mat,2);

            cmd.ReleaseTemporaryRT(_BlurTexA);
            cmd.ReleaseTemporaryRT(_BlurTexB);
        }

    }

}