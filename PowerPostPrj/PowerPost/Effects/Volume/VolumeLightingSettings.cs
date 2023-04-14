namespace PowerPost
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/VolumeLighting")]
    public class VolumeLightingSettings : BasePostExSettings
    {
        public BoolParameter enabled = new BoolParameter(false);
        public ClampedIntParameter stepCount = new ClampedIntParameter(10,1,20);
        public ClampedFloatParameter intenstiy = new ClampedFloatParameter(1,0,1);
        public override BasePostExPass CreateNewInstance()
        {
            return new VolumeLighting();
        }

        public override bool IsActive()
        {
            return enabled.value;
        }
    }
}