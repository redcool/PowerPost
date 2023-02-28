namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ToneMappingSettingsControl))]
    public class ToneMappingSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as ToneMappingSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class ToneMappingSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public ToneMappingSettings settings;
        
        // variables
        public ToneMappingSettings.Mode mode;
public float weight;
public float scale;
public float offset;
public float saturate;
public float brightness;
public Texture colorGradingLut;
public bool colorGradingUseLogC;


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
            settings.mode.value = mode;
settings.weight.value = weight;
settings.scale.value = scale;
settings.offset.value = offset;
settings.saturate.value = saturate;
settings.brightness.value = brightness;
settings.colorGradingLut.value = colorGradingLut;
settings.colorGradingUseLogC.value = colorGradingUseLogC;

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
            mode = settings.mode.value;
weight = settings.weight.value;
scale = settings.scale.value;
offset = settings.offset.value;
saturate = settings.saturate.value;
brightness = settings.brightness.value;
colorGradingLut = settings.colorGradingLut.value;
colorGradingUseLogC = settings.colorGradingUseLogC.value;

        }
    }
}
