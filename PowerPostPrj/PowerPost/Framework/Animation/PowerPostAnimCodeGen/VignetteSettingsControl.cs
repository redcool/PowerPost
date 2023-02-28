namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
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
    public class VignetteSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public VignetteSettings settings;
        
        // variables
        public float intensity;
public float smoothness;
public bool rounded;
public Color color;
public float centerX;
public float centerY;
public float ovalX;
public float ovalY;


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
            settings.intensity.value = intensity;
settings.smoothness.value = smoothness;
settings.rounded.value = rounded;
settings.color.value = color;
settings.centerX.value = centerX;
settings.centerY.value = centerY;
settings.ovalX.value = ovalX;
settings.ovalY.value = ovalY;

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
