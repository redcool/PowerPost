namespace PowerPost {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    [Serializable, VolumeComponentMenu("PowerPostEx/SunShaft")]
    public class SunShaftSettings : BasePostExSettings
    {
        [Header("Sun")]
        [Tooltip("use Lighting/Environment/Sun Source")]
        public BoolParameter useRenderSettingsSun = new BoolParameter(true);
        public Vector2Parameter sunPos = new Vector2Parameter(new Vector4(.5f, .5f));

        public BoolParameter hiddenSunShaftBackfaceSun = new BoolParameter(true);


        [Header("Radial Blur")]
        public ClampedIntParameter radialBlurIterations = new ClampedIntParameter(1, 1,4);
        public ClampedFloatParameter sunShaftBlurRadius = new ClampedFloatParameter(2.5f,0.1f,10,true);
        [Header("Sun Color")]
        public ColorParameter sunThreshold = new ColorParameter(new Color(.8f, .7f, .6f,0));
        public ColorParameter sunColor = new ColorParameter(Color.white);
        public ClampedFloatParameter sunShaftIntensity = new ClampedFloatParameter(0,0,50);

        [Header("Sun Range")]
        public ClampedFloatParameter maxRadius = new ClampedFloatParameter(0.2f,0,1);


        public override BasePostExPass CreateNewInstance()
        {
            return new SunShaftPass();
        }

        public override bool IsActive()
        {
            return maxRadius.value > 0 && sunShaftIntensity.value > 0;
        }
    }
}