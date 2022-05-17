using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
namespace PowerPost
{
    [Serializable,VolumeComponentMenu("PowerPostEx/Vignette")]
    public class VignetteSettings : BasePostExSettings
    {
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0,0,2);
        public ClampedFloatParameter smoothness = new ClampedFloatParameter(0.5f, 0, 1);
        public BoolParameter rounded = new BoolParameter(false);
        public ColorParameter color = new ColorParameter(Color.black);

        [Header("Center")]
        public ClampedFloatParameter centerX = new ClampedFloatParameter(0.5f, 0, 1);
        public ClampedFloatParameter centerY = new ClampedFloatParameter(0.5f, 0, 1);
        [Header("Oval Shape")]
        public ClampedFloatParameter ovalX = new ClampedFloatParameter(1, 0, 1);
        public ClampedFloatParameter ovalY = new ClampedFloatParameter(1, 0, 1);

        public override BasePostExPass CreateNewInstance()
        {
            return new VignettePass();
        }

        public override bool IsActive()
        {
            return intensity.value>0;
        }
    }
}
