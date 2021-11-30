namespace PowerPost {
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public class SunShaftSettings : BasePostExSettings
    {
        [Header("Sun")]
        public BoolParameter useRenderSettingsSun = new BoolParameter(true);
        public Vector2Parameter sunPos = new Vector2Parameter(new Vector4(.5f, .5f));

        [Header("Radial Blur")]
        public ClampedIntParameter radialBlurIterations = new ClampedIntParameter(1, 1,4);
        public ClampedFloatParameter sunShaftBlurRadius = new ClampedFloatParameter(2.5f,1,10);
        [Header("Sun Color")]
        public ColorParameter sunThreshold = new ColorParameter(new Color(.8f, .7f, .6f,0));
        public ColorParameter sunColor = new ColorParameter(Color.white);
        public FloatParameter sunShaftIntensity = new FloatParameter(1.15f);

        [Header("Sun Range")]
        public ClampedFloatParameter maxRadius = new ClampedFloatParameter(0.2f,0.1f,1f);


        public override BasePostExPass CreateNewInstance()
        {
            return new SunShaftPass();
        }

        public override bool IsActive()
        {
            return true;
        }
    }
}