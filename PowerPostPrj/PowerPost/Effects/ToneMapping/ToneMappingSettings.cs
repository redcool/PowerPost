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
        }
        public ToneMappingModeParameter mode = new ToneMappingModeParameter(Mode.None);

        [Tooltip("Weight tone:")]
        public FloatParameter weight = new FloatParameter(1);

        [Header("Tone")]
        public FloatParameter saturate = new FloatParameter(1);
        public FloatParameter brightness = new FloatParameter(1);

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

        public new int ID => int.MaxValue;
    }

    [Serializable]
    public class ToneMappingModeParameter : VolumeParameter<ToneMappingSettings.Mode>
    {
        public ToneMappingModeParameter(ToneMappingSettings.Mode mode):base(mode,false)
        {
        }
    }
}