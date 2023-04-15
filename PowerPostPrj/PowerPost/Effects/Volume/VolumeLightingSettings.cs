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
        public BoolParameter reverseLight = new BoolParameter(false);
        public ColorParameter color = new ColorParameter(Color.white, true, true, true);

        [Header("Key Options")]
        public ClampedIntParameter stepCount = new ClampedIntParameter(10,1,20);
        public ClampedIntParameter iterators = new ClampedIntParameter(1, 0, 3);
        public ClampedIntParameter downSampers = new ClampedIntParameter(1, 1, 3);

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