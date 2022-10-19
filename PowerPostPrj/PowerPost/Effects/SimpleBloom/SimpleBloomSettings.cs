namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/SimpleBloom")]
    public class SimpleBloomSettings : BasePostExSettings
    {
        public ClampedFloatParameter intensity = new ClampedFloatParameter(0 ,0,10);
        public ClampedIntParameter iterators = new ClampedIntParameter( 4,1,7 );
        public ClampedFloatParameter threshold = new ClampedFloatParameter( 0.5f ,0,2);
        public ClampedFloatParameter softThreshold = new ClampedFloatParameter( 0.5f ,0,1);
        //public ClampedFloatParameter smoothBorder = new ClampedFloatParameter( 1 ,0.5f,2);
        public ColorParameter bloomColor = new ColorParameter ( Color.white );

        public override BasePostExPass CreateNewInstance()
        {
            return new SimpleBloomPass();
        }

        public override bool IsActive()
        {
            return intensity.value > 0;
        }
    }
}