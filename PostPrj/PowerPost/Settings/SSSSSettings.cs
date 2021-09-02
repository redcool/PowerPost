using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    [Serializable,VolumeComponentMenu("Custom/SSSS")]
    public class SSSSSettings : VolumeComponent,IPostProcessComponent,IPostProcessingSetting
    {
        public ColorParameter strength = new ColorParameter(Color.white);
        public ColorParameter falloff = new ColorParameter (Color.green );
        public FloatParameter blurScale = new FloatParameter (0.1f );
        //public IntParameter samples = new IntParameter(25);

        public PostExPass CreateNewInstance()
        {
            return new SSSSPass();
        }

        public bool IsActive()
        {
            return Mathf.Abs(blurScale.value) > 0.1f;
        }

        public bool IsTileCompatible() => true;

    }

}