namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(WhiteBalanceSettingsControl))]
    public class WhiteBalanceSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as WhiteBalanceSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class WhiteBalanceSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public WhiteBalanceSettings settings;
        
        // variables
        public float temperature;
public float tint;


        void Awake()
        {
            RecordVars();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            if (!postVolume)
                postVolume = GetComponent<Volume>();

            if (postVolume && postVolume.profile)
            {
                postVolume.profile.TryGet(out settings);
            }

            intervalTime = 1f / updateCount;
            InvokeRepeating(nameof(UpdateVars), 0, intervalTime);
        }
        private void OnDisable()
        {
            if (IsInvoking(nameof(UpdateVars)))
                CancelInvoke(nameof(UpdateVars));
        }
        void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.temperature.value = temperature;
settings.tint.value = tint;

        }

        public void RecordVars()
        {
            if (!settings)
            {
                postVolume = GetComponent<Volume>();
                if (postVolume && postVolume.profile)
                {
                    postVolume.profile.TryGet(out settings);
                }
            }

            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            temperature = settings.temperature.value;
tint = settings.tint.value;

        }
    }
}
