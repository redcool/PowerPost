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
        public ClampedIntParameter downSamples = new ClampedIntParameter(0, 0, 4);
        public ClampedFloatParameter sampleRadius = new ClampedFloatParameter(0.5f, 0.1f, 1);
        public TextureParameter noiseTex = new TextureParameter(null);
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