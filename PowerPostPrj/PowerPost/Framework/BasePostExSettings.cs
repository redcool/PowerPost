namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    public abstract class BasePostExSettings : VolumeComponent, IPostProcessComponent
    {
        public abstract BasePostExPass CreateNewInstance();

        public abstract bool IsActive();

        public virtual bool IsTileCompatible() => true;
        protected override void OnEnable()
        {
            base.OnEnable();
            PowerPostFeature.AddSetting(this);
        }
    }
}