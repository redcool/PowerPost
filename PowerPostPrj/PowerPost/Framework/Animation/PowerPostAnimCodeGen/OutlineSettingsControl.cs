namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(OutlineSettingsControl))]
    public class OutlineSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as OutlineSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class OutlineSettingsControl : BaseSettingsControl<OutlineSettings>
    {
        // variables
        public int downSamples;
public LayerMask layer;
public Color outlineColor;
public float outlineWidth;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.downSamples.value = downSamples;
settings.layer.value = layer;
settings.outlineColor.value = outlineColor;
settings.outlineWidth.value = outlineWidth;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            downSamples = settings.downSamples.value;
layer = settings.layer.value;
outlineColor = settings.outlineColor.value;
outlineWidth = settings.outlineWidth.value;

        }
    }
}
