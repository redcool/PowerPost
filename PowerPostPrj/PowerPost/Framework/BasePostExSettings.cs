namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public abstract class BasePostExSettings : VolumeComponent, IPostProcessComponent, IPostProcessingSetting
    {
        public abstract BasePostExPass CreateNewInstance();

        public abstract bool IsActive();

        public virtual bool IsTileCompatible() => true;
    }
}