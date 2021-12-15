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
            Weight = 5
        }
        public ToneMappingModeParameter mode = new ToneMappingModeParameter(Mode.None);
        public FloatParameter weight = new FloatParameter(1);

        public override BasePostExPass CreateNewInstance()
        {
            return new ToneMappingPass();
        }

        public override bool IsActive()
        {
            return true;
        }
    }

    [Serializable]
    public class ToneMappingModeParameter : VolumeParameter<ToneMappingSettings.Mode>
    {
        public ToneMappingModeParameter(ToneMappingSettings.Mode mode):base(mode,false)
        {
        }
    }
}