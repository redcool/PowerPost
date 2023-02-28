namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(VignetteSettingsControl))]
    public class VignetteSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as VignetteSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class VignetteSettingsControl : BaseSettingsControl<VignetteSettings>
    {
        // variables
        public float intensity;
public float smoothness;
public bool rounded;
public Color color;
public float centerX;
public float centerY;
public float ovalX;
public float ovalY;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.intensity.value = intensity;
settings.smoothness.value = smoothness;
settings.rounded.value = rounded;
settings.color.value = color;
settings.centerX.value = centerX;
settings.centerY.value = centerY;
settings.ovalX.value = ovalX;
settings.ovalY.value = ovalY;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            intensity = settings.intensity.value;
smoothness = settings.smoothness.value;
rounded = settings.rounded.value;
color = settings.color.value;
centerX = settings.centerX.value;
centerY = settings.centerY.value;
ovalX = settings.ovalX.value;
ovalY = settings.ovalY.value;

        }
    }
}
