namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(VolumeLightingSettingsControl))]
    public class VolumeLightingSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as VolumeLightingSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class VolumeLightingSettingsControl : BaseSettingsControl<VolumeLightingSettings>
    {
        // variables
        public bool enabled;
public bool reverseLight;
public Color color;
public int stepCount;
public int iterators;
public int downSampers;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.enabled.value = enabled;
settings.reverseLight.value = reverseLight;
settings.color.value = color;
settings.stepCount.value = stepCount;
settings.iterators.value = iterators;
settings.downSampers.value = downSampers;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            enabled = settings.enabled.value;
reverseLight = settings.reverseLight.value;
color = settings.color.value;
stepCount = settings.stepCount.value;
iterators = settings.iterators.value;
downSampers = settings.downSampers.value;

        }
    }
}
