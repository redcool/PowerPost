using PowerPost;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace PowerPost
{
    [Serializable, VolumeComponentMenu("PowerPostEx/WhiteBalance")]
    public class WhiteBalanceSettings : BasePostExSettings
    {
        public ClampedFloatParameter temperature = new ClampedFloatParameter(0, -100, 100);
        public ClampedFloatParameter tint = new ClampedFloatParameter(0, -100, 100);
        public override BasePostExPass CreateNewInstance()
        {
            return new WhiteBalancePass();
        }

        public override bool IsActive()
        {
            return !Mathf.Approximately(temperature.value, 0)
                || !Mathf.Approximately(tint.value, 0);
        }
    }
}