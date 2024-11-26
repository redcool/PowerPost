namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;

    public abstract class BaseSettingsControl<T> : MonoBehaviour
        where T : VolumeComponent
    {
        public float updateCount = 5;
        public bool isAutoActiveSettings;

        float intervalTime = 1;

        public Volume postVolume;
        
        [Header("Temporary Mode")]
        [Tooltip("Create a profile only save in memory, dont need save to disk.")]
        public bool isTemporaryProfile;

        [Header("Volume Parameters")]
        public T settings;
        void Awake()
        {
            RecordVars();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            SetupSettings();

            if(isTemporaryProfile)
            SetupTemporarySettings();

            intervalTime = 1f / updateCount;
            InvokeRepeating(nameof(UpdateVars), 0, intervalTime);

            if (settings && isAutoActiveSettings)
            {
                settings.active=true;
            }
        }

        private void OnDisable()
        {
            if (IsInvoking(nameof(UpdateVars)))
                CancelInvoke(nameof(UpdateVars));

            if (settings && isAutoActiveSettings)
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

        public void SetupTemporarySettings()
        {
            postVolume = gameObject.GetOrAddComponent<Volume>();
            if (! postVolume.profile)
            {
                postVolume.profile = ScriptableObject.CreateInstance<VolumeProfile>();
            }
            if(! postVolume.profile.TryGet(out settings))
            {
                settings = postVolume.profile.Add<T>();
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