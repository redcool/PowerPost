namespace PowerPost
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/Sketch")]
    public class SketchSettings : BasePostExSettings
    {
        //public ToneMappingModeParameter mode = new ToneMappingModeParameter(Mode.None);
        [Header("BaseSettings")]
        public BoolParameter sketchToggle = new BoolParameter(false);
        public ColorParameter sketchColor = new ColorParameter(Color.black);
        public Texture2DParameter sketchTexture = new Texture2DParameter(null);
        public ClampedFloatParameter sketchBlend = new ClampedFloatParameter(0.5f, 0, 1);
        public ClampedFloatParameter lightInfluence = new ClampedFloatParameter(0,0,5);
        //public LayerMaskParameter cullingLayer = new LayerMaskParameter(0);
        public FloatParameter triPlanerPower = new FloatParameter(30);
        public IntParameter renderOffset = new IntParameter(0);
        //public CullingGroup cullingGroup = new CullingGroup();
        [Header("OriginTextureAdjust")]
        public BoolParameter DebugMode = new BoolParameter(false);
        public FloatParameter sketchArea = new ClampedFloatParameter(0.5f, 0, 1f);
        public FloatParameter originLightness = new FloatParameter(5);
        public FloatParameter originContrast = new FloatParameter(1);
        public BoolParameter inveseArea =  new BoolParameter(false);
        [Header("SketchAdjust")]
        public ClampedFloatParameter sketchThreshold = new ClampedFloatParameter(0.5f, 0, 1);
        public ClampedFloatParameter sketchSmooth = new ClampedFloatParameter(0.5f, 0, 1);
        public FloatParameter sketchSize = new FloatParameter(2f);
        public FloatParameter sketchTransitionRange = new ClampedFloatParameter(0.2f,0f,1f);
        public FloatParameter sketchLower = new ClampedFloatParameter(0,0f,1f);
        public FloatParameter sketchUpper = new ClampedFloatParameter(0.5f,0f,1f);

        //[Tooltip("Weight tone:")]
        //public FloatParameter weight = new FloatParameter(1);

        //[Header("Color Tint")]
        ////public ClampedFloatParameter hue = new ClampedFloatParameter(0,0,1);
        //[Tooltip("hue scale")]
        //public FloatParameter scale = new FloatParameter(1);
        //[Tooltip("hue offset")]
        //public ClampedFloatParameter offset = new ClampedFloatParameter(0, 0, 1);
        //public MinFloatParameter saturate = new MinFloatParameter(1, 0);
        //public MinFloatParameter brightness = new MinFloatParameter(1, 0);

        //[Header("ColorGrading")]
        //public TextureParameter colorGradingLut = new TextureParameter(null);
        //public BoolParameter colorGradingUseLogC = new BoolParameter(false);
        public override bool NeedWriteToTarget() => false;
        public override BasePostExPass CreateNewInstance()
        {
            var pass = new SketchPass();
            pass.renderPassEvent = UnityEngine.Rendering.Universal.RenderPassEvent.BeforeRenderingTransparents - 10;
            return pass;
        }

        public override bool IsActive()
        {
            return sketchToggle.value;
        }

        public override int Order => int.MaxValue - 20;
    }


}