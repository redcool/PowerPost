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

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, VolumeLightingSettings settings, CommandBuffer cmd)
        {
            var mat = GetTargetMaterial("Hidden/PowerPost/VolumeLight");
            cmd.BlitColorDepth(sourceTex, targetTex, targetTex, mat);
        }
    }

}