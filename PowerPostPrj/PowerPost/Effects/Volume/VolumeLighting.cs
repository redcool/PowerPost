namespace PowerPost
{
    using PowerUtilities;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class VolumeLighting : BasePostExPass<VolumeLightingSettings>
    {
        public override string PassName => nameof(VolumeLighting);

        readonly int _StepCount = Shader.PropertyToID(nameof(_StepCount));
        readonly int _Intenstiy = Shader.PropertyToID(nameof(_Intenstiy));

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, VolumeLightingSettings settings, CommandBuffer cmd)
        {
            var mat = GetTargetMaterial("Hidden/PowerPost/VolumeLight");
            mat.SetInt(_StepCount, settings.stepCount.value);
            mat.SetFloat(_Intenstiy, settings.intenstiy.value);
            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, mat);
        }
    }

}