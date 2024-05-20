namespace PowerPost
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/SimpleDOF")]
    public class SimpleDOFSettings : BasePostExSettings
    {
        [Tooltip("follow Tag target ,use distance when empty")]
        public VolumeParameter<string> targetTag = new VolumeParameter<string>();

        [Tooltip("clear distance")]
        public ClampedFloatParameter distance = new ClampedFloatParameter(1, 0, 1);
        //public ClampedIntParameter downsample = new ClampedIntParameter(2, 1, 8);

        [Tooltip("clear range near clear distace")]
        public ClampedFloatParameter depthRange = new ClampedFloatParameter(0.05f, 0, 1f);

        [Tooltip("gaussian blur size")]
        public ClampedFloatParameter blurSize = new ClampedFloatParameter(1.1f, 1, 8);

        [Tooltip("blur area show red")]
        public BoolParameter debugMode = new BoolParameter(false);

        public override BasePostExPass CreateNewInstance()
        {
            return new SimpleDOFPass();
        }

        public override bool IsActive()
        {
            return blurSize.value > 1.1f;
        }

    }
}