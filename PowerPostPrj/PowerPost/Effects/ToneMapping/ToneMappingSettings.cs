namespace PowerPost
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/ToneMapping")]
    public class ToneMappingSettings : BasePostExSettings
    {
        public enum Mode
        {
            None = 0,
            Lotte = 1,
            ACES = 2,
            Reinhard =3,
            Uncharted2Tonemap = 4,
            Weight = 5,
            Exposure = 6,
            ACESFitted = 7,
            GTTone = 8,
        }
        public ToneMappingModeParameter mode = new ToneMappingModeParameter(Mode.None);

        [Tooltip("Weight tone:")]
        public FloatParameter weight = new FloatParameter(1);

        [Header("Color Tint")]
        //public ClampedFloatParameter hue = new ClampedFloatParameter(0,0,1);
        [Tooltip("hue scale")]
        public FloatParameter scale = new FloatParameter(1);
        [Tooltip("hue offset")]
        public ClampedFloatParameter offset = new ClampedFloatParameter(0,0,1);
        public MinFloatParameter saturate = new MinFloatParameter(1,0);
        public MinFloatParameter brightness = new MinFloatParameter(1,0);

        [Header("ColorGrading")]
        public TextureParameter colorGradingLut = new TextureParameter(null);
        public BoolParameter colorGradingUseLogC = new BoolParameter(false);

        public override BasePostExPass CreateNewInstance()
        {
            var pass = new ToneMappingPass();
            pass.renderPassEvent = UnityEngine.Rendering.Universal.RenderPassEvent.BeforeRenderingPostProcessing;
            return pass;
        }

        public override bool IsActive()
        {
            return mode != Mode.None;
        }

        public override int Order => int.MaxValue;
    }

    [Serializable]
    public class ToneMappingModeParameter : VolumeParameter<ToneMappingSettings.Mode>
    {
        public ToneMappingModeParameter(ToneMappingSettings.Mode mode):base(mode,false)
        {
        }
    }
}