namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(PowerPostSettingsControlTemplate), true)]
    public class PowerPostSettingsControlTemplateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as PowerPostSettingsControlTemplate;
                inst.RecordVars();
            }
        }
    }
#endif

    [ExecuteInEditMode]
    public class PowerPostSettingsControlTemplate : BaseSettingsControl<RadialBlurSettings>
    {
        // variables

        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity; {2}
        }

        public override void RecordVars()
        {
            base.RecordVars();

            if (!settings)
                return;

            //rotateRate = settings.rotateRate.value; {3}
        }
    }
}