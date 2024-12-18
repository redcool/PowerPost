namespace PowerPost
{
    using PowerUtilities;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable, VolumeComponentMenu("PowerPostEx/Blur")]
    public class BlurSettings : BasePostExSettings
    {
        public ClampedFloatParameter blurSize = new ClampedFloatParameter(0, 0, 10);
        public ClampedIntParameter stepCount = new ClampedIntParameter(1, 1, 100);
        public ClampedIntParameter downSamples = new ClampedIntParameter(1, 0, 3);
        public override BasePostExPass CreateNewInstance()
        {
            return new BlurPass();
        }

        public override bool IsActive()
        {
            return blurSize.value > 0;
        }
    }


    public class BlurPass : BasePostExPass<BlurSettings>
    {
        readonly int
            _StepCount = Shader.PropertyToID(nameof(_StepCount)),
            _BlurSize = Shader.PropertyToID(nameof(_BlurSize)),
            _BlurTexA = Shader.PropertyToID(nameof(_BlurTexA)),
            _BlurTexB = Shader.PropertyToID(nameof(_BlurTexB))
            ;


        public override string PassName => nameof(BlurPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, BlurSettings settings, CommandBuffer cmd)
        {
            ref var cameraData = ref renderingData.cameraData;
            var width = cameraData.cameraTargetDescriptor.width >> settings.downSamples.value;
            var height = cameraData.cameraTargetDescriptor.height >> settings.downSamples.value;
            cmd.GetTemporaryRT(_BlurTexA, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
            cmd.GetTemporaryRT(_BlurTexB, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);

            var mat = GetTargetMaterial("Hidden/PowerPost/Blur");
            mat.SetInt(_StepCount, settings.stepCount.value);
            mat.SetFloat(_BlurSize, settings.blurSize.value);

            // 1 time,
            cmd.BlitColorDepth(sourceTex, _BlurTexB, _BlurTexB, mat, 0);

            // 2 times
            cmd.BlitColorDepth(_BlurTexB, _BlurTexA, _BlurTexA, mat, 1);
            
            // 3 do pass 0,one more time
            cmd.BlitColorDepth(_BlurTexA, targetTex, targetTex, mat,0);

            cmd.ReleaseTemporaryRT(_BlurTexA);
            cmd.ReleaseTemporaryRT(_BlurTexB);
        }
    }
}