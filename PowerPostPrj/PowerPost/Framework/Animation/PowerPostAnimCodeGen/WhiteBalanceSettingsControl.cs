namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
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
    public class WhiteBalanceSettingsControl : BaseSettingsControl<WhiteBalanceSettings>
    {
        // variables
        public float temperature;
public float tint;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.temperature.value = temperature;
settings.tint.value = tint;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            temperature = settings.temperature.value;
tint = settings.tint.value;

        }
    }
}
