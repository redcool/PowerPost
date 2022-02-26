namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable, VolumeComponentMenu("PowerPostEx/SSAO")]
    public class SSAOSettings : BasePostExSettings
    {
        public ClampedIntParameter downSamples = new ClampedIntParameter(1, 1, 4);
        public ClampedFloatParameter sampleRadius = new ClampedFloatParameter(0.1f, 0.01f, 0.2f);
        public ClampedIntParameter sampleCount = new ClampedIntParameter(1,1,6);
        public ClampedFloatParameter intensity = new ClampedFloatParameter(1, 0.1f, 5f);
        public override BasePostExPass CreateNewInstance()
        {
            var pass =  new SSAOPass();
            return pass;
        }
        public override bool IsActive()
        {

            return true;
        }
    }
}