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
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0f, 5f);
        public ClampedFloatParameter radius = new ClampedFloatParameter(0.1f, 0.01f, 0.2f);
        public BoolParameter downSample = new BoolParameter(true);
        public ClampedIntParameter sampleCount = new ClampedIntParameter(1, 1, 40);
        public override BasePostExPass CreateNewInstance()
        {
            var pass =  new SSAOPass();
            return pass;
        }
        public override bool IsActive()
        {
            return intensity.value > 0;
            return true;
        }
    }
}