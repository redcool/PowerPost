namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
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
    public class OutlineSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public OutlineSettings settings;
        
        // variables
        public int downSamples;
public LayerMask layer;
public Color outlineColor;
public float outlineWidth;


        void Awake()
        {
            RecordVars();
        }

        // Start is called before the first frame update
        void OnEnable()
        {
            if (!postVolume)
                postVolume = GetComponent<Volume>();

            if (postVolume && postVolume.profile)
            {
                postVolume.profile.TryGet(out settings);
            }

            intervalTime = 1f / updateCount;
            InvokeRepeating(nameof(UpdateVars), 0, intervalTime);
        }
        private void OnDisable()
        {
            if (IsInvoking(nameof(UpdateVars)))
                CancelInvoke(nameof(UpdateVars));
        }
        void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.downSamples.value = downSamples;
settings.layer.value = layer;
settings.outlineColor.value = outlineColor;
settings.outlineWidth.value = outlineWidth;

        }

        public void RecordVars()
        {
            if (!settings)
            {
                postVolume = GetComponent<Volume>();
                if (postVolume && postVolume.profile)
                {
                    postVolume.profile.TryGet(out settings);
                }
            }

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
