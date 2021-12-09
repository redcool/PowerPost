﻿namespace PowerPost
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable, VolumeComponentMenu("PowerPostEx/Glitch")]
    public class GlitchSettings : BasePostExSettings
    {
        [Header("Global")]
        public ClampedFloatParameter glitchHorizontalIntensity = new ClampedFloatParameter(1,0,1);
        [Header("Jitter")]
        public ClampedFloatParameter scanlineJitter = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter jitterBlockSize = new ClampedFloatParameter(1, 1,1000);
        [Header("Snow Flake")]
        public ClampedFloatParameter snowFlakeAmplitude = new ClampedFloatParameter(0, 0, 1);
        [Header("Vertical Jump")]
        public ClampedFloatParameter verticalJump = new ClampedFloatParameter(0, 0, 1);

        [Header("h Shadke")]
        public ClampedFloatParameter horizontalShake = new ClampedFloatParameter(0, 0, 1);
        
        [Header("Color Drift")]
        public ClampedFloatParameter colorDrift = new ClampedFloatParameter(0, 0, 1);
        public ClampedFloatParameter colorDriftSpeed = new ClampedFloatParameter(666,1,1000f);

        [Header("Draw Per Objects")]
        [Tooltip("draw layer's objects with stencil")]
        public LayerMaskParameter layer = new LayerMaskParameter(0);

        public ClampedIntParameter stencilRef = new ClampedIntParameter(6, 0, 255);
        public override BasePostExPass CreateNewInstance()
        {
            return new GlitchPass();
        }

        public override bool IsActive()
        {
            //var active = scanlineJitter.value != 0 ||
            //    snowFlakeAmplitude.value != 0 ||
            //    verticalJump.value != 0 ||
            //    horizontalShake.value != 0 ||
            //    colorDrift.value != 0
            //    ;

            return glitchHorizontalIntensity.value > 0;
        }

    }
}
