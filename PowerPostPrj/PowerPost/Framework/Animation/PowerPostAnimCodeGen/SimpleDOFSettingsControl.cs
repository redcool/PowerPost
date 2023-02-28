namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
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
    public class SimpleDOFSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public SimpleDOFSettings settings;
        
        // variables
        public float distance;
public float depthRange;
public float blurSize;
public bool debugMode;


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
            settings.distance.value = distance;
settings.depthRange.value = depthRange;
settings.blurSize.value = blurSize;
settings.debugMode.value = debugMode;

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
            distance = settings.distance.value;
depthRange = settings.depthRange.value;
blurSize = settings.blurSize.value;
debugMode = settings.debugMode.value;

        }
    }
}
