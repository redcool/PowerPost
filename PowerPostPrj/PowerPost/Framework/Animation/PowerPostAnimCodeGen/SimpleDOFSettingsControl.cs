namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SimpleDOFSettingsControl))]
    public class SimpleDOFSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as SimpleDOFSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class SimpleDOFSettingsControl : BaseSettingsControl<SimpleDOFSettings>
    {
        // variables
        public float distance;
public float depthRange;
public float blurSize;
public bool debugMode;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.distance.value = distance;
settings.depthRange.value = depthRange;
settings.blurSize.value = blurSize;
settings.debugMode.value = debugMode;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            distance = settings.distance.value;
depthRange = settings.depthRange.value;
blurSize = settings.blurSize.value;
debugMode = settings.debugMode.value;

        }
    }
}
