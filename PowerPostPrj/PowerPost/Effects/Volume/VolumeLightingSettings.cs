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
        public override BasePostExPass CreateNewInstance()
        {
            return new VolumeLighting();
        }

        public override bool IsActive()
        {
            return true;
        }
    }
}