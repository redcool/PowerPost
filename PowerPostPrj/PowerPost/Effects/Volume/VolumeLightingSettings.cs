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