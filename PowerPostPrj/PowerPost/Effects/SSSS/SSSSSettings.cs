using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PowerPost
{
    /// <summary>
    /// Screen separate sss.
    /// 
    /// 1 urp'asset renderer 
    ///     1 Filtering opaque (Transparent)layer mask remove target layer.
    ///     2 add RenderObjects 
    ///         1 setup target layer
    ///         2 Overrides enable stencil
    ///         3 value set 5
    ///         4 Compare Function = always
    ///  2 SSSSSettings stencilRef = 5
    /// </summary>
    [Serializable,VolumeComponentMenu("Custom/SSSS")]
    public class SSSSSettings : BasePostExSettings
    {
        public ColorParameter strength = new ColorParameter(Color.white);
        public ColorParameter falloff = new ColorParameter (Color.red );
        public ClampedFloatParameter blurScale = new ClampedFloatParameter(0.1f ,0.01f,20f);
        public ClampedIntParameter stencilRef = new ClampedIntParameter(5,0,255);
        //public IntParameter samples = new IntParameter(25);

        public override BasePostExPass CreateNewInstance()
        {
            return new SSSSPass();
        }

        public override bool IsActive()
        {
            return Mathf.Abs(blurScale.value) > 0.1f;
        }

    }

}