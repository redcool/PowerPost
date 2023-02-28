namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
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
    public class SunShaftSettingsControl : BaseSettingsControl<SunShaftSettings>
    {
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


        public override void UpdateVars()
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

        public override void RecordVars()
        {
            base.RecordVars();
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
