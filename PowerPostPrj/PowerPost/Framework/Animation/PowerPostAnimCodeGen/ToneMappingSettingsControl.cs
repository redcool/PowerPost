namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ToneMappingSettingsControl))]
    public class ToneMappingSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as ToneMappingSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class ToneMappingSettingsControl : BaseSettingsControl<ToneMappingSettings>
    {
        // variables
        public ToneMappingSettings.Mode mode;
public float weight;
public float scale;
public float offset;
public float saturate;
public float brightness;
public Texture colorGradingLut;
public bool colorGradingUseLogC;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.mode.value = mode;
settings.weight.value = weight;
settings.scale.value = scale;
settings.offset.value = offset;
settings.saturate.value = saturate;
settings.brightness.value = brightness;
settings.colorGradingLut.value = colorGradingLut;
settings.colorGradingUseLogC.value = colorGradingUseLogC;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            mode = settings.mode.value;
weight = settings.weight.value;
scale = settings.scale.value;
offset = settings.offset.value;
saturate = settings.saturate.value;
brightness = settings.brightness.value;
colorGradingLut = settings.colorGradingLut.value;
colorGradingUseLogC = settings.colorGradingUseLogC.value;

        }
    }
}
