namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(SSSSSettingsControl))]
    public class SSSSSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as SSSSSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class SSSSSettingsControl : BaseSettingsControl<SSSSSettings>
    {
        // variables
        public Color strength;
public Color falloff;
public float blurScale;
public LayerMask layer;
public int stencilRef;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.strength.value = strength;
settings.falloff.value = falloff;
settings.blurScale.value = blurScale;
settings.layer.value = layer;
settings.stencilRef.value = stencilRef;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            strength = settings.strength.value;
falloff = settings.falloff.value;
blurScale = settings.blurScale.value;
layer = settings.layer.value;
stencilRef = settings.stencilRef.value;

        }
    }
}
