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
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0, 0f, 50f);
        public ClampedFloatParameter radius = new ClampedFloatParameter(0.1f, 0.00001f, 0.2f);
        
        public BoolParameter downSample = new BoolParameter(true);
        [Header("Samples")]
        public ClampedIntParameter sampleCount = new ClampedIntParameter(1, 1, 5);
        public ColorParameter aoColor = new ColorParameter(Color.black,true,false,false);

        public override BasePostExPass CreateNewInstance()
        {
            return new SSAOPass();
        }
        public override bool IsActive()
        {
            return intensity.value > 0;
        }
    }
}