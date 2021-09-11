namespace PowerPost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("Custom/Glitch")]
    public class GlitchSettings : BasePostExSettings
    {
        [Range(0, 1)] public FloatParameter scanlineJitter = new FloatParameter(0);
        [Range(0, 1)] public FloatParameter snowFlakeAmplitude = new FloatParameter(0);
        [Range(0, 1)] public FloatParameter verticalJump = new FloatParameter(0);
        [Range(0, 1)] public FloatParameter horizontalShake = new FloatParameter(0);
        [Range(0, 1)] public FloatParameter colorDrift = new FloatParameter(0);

        public LayerMaskParameter layer = new LayerMaskParameter(-1);

        public override BasePostExPass CreateNewInstance()
        {
            return new GlitchPass();
        }

        public override bool IsActive()
        {
            return true;
        }

    }
}
