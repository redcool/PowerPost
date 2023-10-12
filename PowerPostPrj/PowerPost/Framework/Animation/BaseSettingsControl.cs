namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public abstract class BaseSettingsControl<T> : MonoBehaviour
        where T : BasePostExSettings
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public T settings;
        //void Awake()
        //{
        //    RecordVars();
        //}

        // Start is called before the first frame update
        void OnEnable()
        {
            SetupSettings();

            intervalTime = 1f / updateCount;
            InvokeRepeating(nameof(UpdateVars), 0, intervalTime);

            if (settings)
            {
                settings.active=true;
            }
        }

        private void OnDisable()
        {
            if (IsInvoking(nameof(UpdateVars)))
                CancelInvoke(nameof(UpdateVars));

            if (settings)
            {
                settings.active=false;
            }
        }

        public void SetupSettings()
        {
            if (!postVolume)
                postVolume = GetComponent<Volume>();

            if (postVolume && postVolume.profile)
            {
                postVolume.profile.TryGet(out settings);
            }
        }

        public virtual void RecordVars()
        {
            if (!settings)
            {
                SetupSettings();
            }
        }
        public abstract void UpdateVars();
    }
}