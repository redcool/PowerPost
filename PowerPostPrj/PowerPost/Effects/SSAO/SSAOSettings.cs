namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/SSAO")]
    public class SSAOSettings : BasePostExSettings
    {
        public ClampedIntParameter downSamples = new ClampedIntParameter(0, 0, 4);
        public LayerMaskParameter layer = new LayerMaskParameter(0);

        public ColorParameter outlineColor = new ColorParameter(Color.white);
        public ClampedFloatParameter outlineWidth = new ClampedFloatParameter(1, 0.1f, 3f);

        public override BasePostExPass CreateNewInstance()
        {
            return new SSAOPass();
        }

        public override bool IsActive()
        {
            //return true;
            return layer.value != 0;
        }
    }
}