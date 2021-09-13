namespace PowerPost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable, VolumeComponentMenu("PowerPostEx/Glitch")]
    public class GlitchSettings : BasePostExSettings
    {
        public ClampedFloatParameter scanlineJitter = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter snowFlakeAmplitude = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter verticalJump = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter horizontalShake = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter colorDrift = new ClampedFloatParameter(0, 0, 1);

        public LayerMaskParameter layer = new LayerMaskParameter(0);

        public ClampedIntParameter stencilRef = new ClampedIntParameter(6, 0, 255);
        public override BasePostExPass CreateNewInstance()
        {
            return new GlitchPass();
        }

        public override bool IsActive()
        {
            //var active = scanlineJitter.value != 0 ||
            //    snowFlakeAmplitude.value != 0 ||
            //    verticalJump.value != 0 ||
            //    horizontalShake.value != 0 ||
            //    colorDrift.value != 0
            //    ;

            return true;
        }

    }
}
