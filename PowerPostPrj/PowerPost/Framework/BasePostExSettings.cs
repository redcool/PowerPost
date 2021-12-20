namespace PowerPost
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    /// <summary>
    /// PowerPost setting
    /// </summary>
    public abstract class BasePostExSettings : VolumeComponent, IPostProcessComponent
    {
        public abstract BasePostExPass CreateNewInstance();

        public abstract bool IsActive();

        public virtual bool IsTileCompatible() => true;

        public int ID => 0;
        protected override void OnEnable()
        {
            PowerPostFeature.AddSetting(this);
            base.OnEnable();
        }

    }
}