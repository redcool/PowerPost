namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/Outline")]
    public class OutlineSettings : BasePostExSettings
    {
        public ClampedFloatParameter width = new ClampedFloatParameter(0.01f, 0.0001f, 0.01f);
        public ClampedIntParameter downSamples = new ClampedIntParameter(0, 0, 4);
        public LayerMaskParameter layer = new LayerMaskParameter(0);

        public override BasePostExPass CreateNewInstance()
        {
            return new OutlinePass();
        }

        public override bool IsActive()
        {
            return layer.value != 0;
        }
    }
}