namespace PowerPost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using PowerUtilities;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public class SketchPass : BasePostExPass<SketchSettings>
    {
        int
            _SketchToggle = Shader.PropertyToID(nameof(_SketchToggle)),
            _SketchTexture = Shader.PropertyToID(nameof(_SketchTexture)),   
            _SketchColor = Shader.PropertyToID(nameof(_SketchColor)),
            _SketchBlend = Shader.PropertyToID(nameof(_SketchBlend)),
            _LightInfluence = Shader.PropertyToID(nameof(_LightInfluence)),
            _TriPlanerPower = Shader.PropertyToID(nameof(_TriPlanerPower)),
            _DebugToggle = Shader.PropertyToID(nameof(_DebugToggle)),
            _OriginBrightnessAdjust = Shader.PropertyToID(nameof(_OriginBrightnessAdjust)),
            _OriginContrastAdjust = Shader.PropertyToID(nameof(_OriginContrastAdjust)),
            _InveseArea = Shader.PropertyToID(nameof(_InveseArea)),
            _SketchThreshold = Shader.PropertyToID(nameof(_SketchThreshold)),
            _SketchSmooth = Shader.PropertyToID(nameof(_SketchSmooth)),
            _SketchSize = Shader.PropertyToID(nameof(_SketchSize)),
            _SketchTransitionRange = Shader.PropertyToID(nameof(_SketchTransitionRange)),
            _SketchLower = Shader.PropertyToID(nameof(_SketchLower)),
            _SketchUpper = Shader.PropertyToID(nameof(_SketchUpper)),
            _SketchRange = Shader.PropertyToID(nameof(_SketchRange))
            ;

        const string SKETCH_SHADER = "Hidden/PowerPost/Sketch";

        public override string PassName => nameof(SketchPass);

        public override void OnExecute(ScriptableRenderContext context, ref RenderingData renderingData, SketchSettings settings, CommandBuffer cmd)
        {
                var mat = GetTargetMaterial(SKETCH_SHADER);
                mat.SetInt(_SketchToggle, settings.sketchToggle.value ? 1 : 0);
                mat.SetInt(_DebugToggle,settings.DebugMode.value ? 1 : 0);
                mat.SetColor(_SketchColor, settings.sketchColor.value);
                mat.SetFloat(_SketchBlend, settings.sketchBlend.value);
                mat.SetFloat(_LightInfluence,settings.lightInfluence.value);
                mat.SetFloat(_TriPlanerPower,settings.triPlanerPower.value);
                mat.SetFloat(_SketchThreshold,1 - settings.sketchThreshold.value);
                mat.SetTexture(_SketchTexture, settings.sketchTexture.value);
                mat.SetFloat(_SketchSmooth, settings.sketchSmooth.value);
                mat.SetFloat(_SketchSize, settings.sketchSize.value);
                mat.SetFloat(_SketchRange,settings.inveseArea.value ? (1 -  settings.sketchArea.value) : (settings.sketchArea.value));
                mat.SetFloat(_SketchTransitionRange,settings.sketchTransitionRange.value);
                mat.SetFloat(_SketchLower,settings.sketchLower.value);
                mat.SetFloat(_SketchUpper,settings.sketchUpper.value);
                mat.SetFloat(_OriginBrightnessAdjust,settings.originLightness.value);
                mat.SetFloat(_OriginContrastAdjust,settings.originContrast.value > 0 ? settings.originContrast.value : 0.01f);
                mat.SetFloat(_InveseArea,settings.inveseArea.value ? 1 : 0);

                cmd.BlitColorDepth(ShaderPropertyIds._CameraColorAttachmentA,ShaderPropertyIds._CameraOpaqueTexture,ShaderPropertyIds._CameraDepthAttachment,mat,0);
                //cmd.BlitColor(ShaderPropertyIds._CameraColorAttachmentA,ShaderPropertyIds._CameraOpaqueTexture,mat,0);
                //cmd.BlitColor(ShaderPropertyIds._CameraOpaqueTexture,ShaderPropertyIds._CameraColorAttachmentA,DefaultBlitMaterial,0);
                cmd.BlitColorDepth(ShaderPropertyIds._CameraOpaqueTexture,ShaderPropertyIds._CameraColorAttachmentA,ShaderPropertyIds._CameraDepthAttachment,DefaultBlitMaterial,0);
        }
    }
}
