namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SSAOSettingsControl))]
    public class SSAOSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as SSAOSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class SSAOSettingsControl : BaseSettingsControl<SSAOSettings>
    {
        // variables
        public float intensity;
public float radius;
public bool downSample;
public int sampleCount;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.intensity.value = intensity;
settings.radius.value = radius;
settings.downSample.value = downSample;
settings.sampleCount.value = sampleCount;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            intensity = settings.intensity.value;
radius = settings.radius.value;
downSample = settings.downSample.value;
sampleCount = settings.sampleCount.value;

        }
    }
}
