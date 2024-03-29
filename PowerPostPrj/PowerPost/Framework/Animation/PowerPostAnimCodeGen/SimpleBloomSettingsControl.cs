namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SimpleBloomSettingsControl))]
    public class SimpleBloomSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as SimpleBloomSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class SimpleBloomSettingsControl : BaseSettingsControl<SimpleBloomSettings>
    {
        // variables
        public float intensity;
public int iterators;
public float threshold;
public float softThreshold;
public Color bloomColor;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.intensity.value = intensity;
settings.iterators.value = iterators;
settings.threshold.value = threshold;
settings.softThreshold.value = softThreshold;
settings.bloomColor.value = bloomColor;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            intensity = settings.intensity.value;
iterators = settings.iterators.value;
threshold = settings.threshold.value;
softThreshold = settings.softThreshold.value;
bloomColor = settings.bloomColor.value;

        }
    }
}
