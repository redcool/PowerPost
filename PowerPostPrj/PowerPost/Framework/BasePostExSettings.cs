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
        /// <summary>
        /// This pass dont need camera color target(_CameraColorAttachmentA,_CameraColorAttachmentB)
        /// set false,when render a texture for later rendering
        /// </summary>
        /// <returns></returns>
        public virtual bool NeedWriteToTarget() => true;
        /// <summary>
        /// Execute order in powerpost group
        /// </summary>
        public virtual int Order => 0;
        protected override void OnEnable()
        {
            PowerPostFeature.AddSetting(this);
            base.OnEnable();
        }

    }
}