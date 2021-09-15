using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

namespace PowerPost
{
    [ VolumeComponentMenu("PowerPostEx/RadialBlur")]
    public class RadialBlurSettings : BasePostExSettings
    {
        public Vector2Parameter center = new Vector2Parameter(new Vector2(.5f, .5f));
        public ClampedFloatParameter radiusMin = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter radiusMax = new ClampedFloatParameter(1, 0, 1);
        public FloatParameter blurSize = new FloatParameter(0.5f);
        public BoolParameter roundness = new BoolParameter(false);

        public override BasePostExPass CreateNewInstance()
        {
            return new RadialBlurPass();
        }

        public override bool IsActive()
        {
            return false;
            return blurSize != 0;

        }
    }
}
