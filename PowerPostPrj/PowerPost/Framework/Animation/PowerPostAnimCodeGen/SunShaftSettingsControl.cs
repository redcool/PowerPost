namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SunShaftSettingsControl))]
    public class SunShaftSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as SunShaftSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class SunShaftSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public SunShaftSettings settings;
        
        // variables
        public bool useRenderSettingsSun;
public Vector2 sunPos;
public bool hiddenSunShaftBackfaceSun;
public int radialBlurIterations;
public float sunShaftBlurRadius;
public Color sunThreshold;
public Color sunColor;
public float sunShaftIntensity;
public float maxRadius;


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
            settings.useRenderSettingsSun.value = useRenderSettingsSun;
settings.sunPos.value = sunPos;
settings.hiddenSunShaftBackfaceSun.value = hiddenSunShaftBackfaceSun;
settings.radialBlurIterations.value = radialBlurIterations;
settings.sunShaftBlurRadius.value = sunShaftBlurRadius;
settings.sunThreshold.value = sunThreshold;
settings.sunColor.value = sunColor;
settings.sunShaftIntensity.value = sunShaftIntensity;
settings.maxRadius.value = maxRadius;

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
            useRenderSettingsSun = settings.useRenderSettingsSun.value;
sunPos = settings.sunPos.value;
hiddenSunShaftBackfaceSun = settings.hiddenSunShaftBackfaceSun.value;
radialBlurIterations = settings.radialBlurIterations.value;
sunShaftBlurRadius = settings.sunShaftBlurRadius.value;
sunThreshold = settings.sunThreshold.value;
sunColor = settings.sunColor.value;
sunShaftIntensity = settings.sunShaftIntensity.value;
maxRadius = settings.maxRadius.value;

        }
    }
}
