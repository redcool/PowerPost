namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(GlitchSettingsControl))]
    public class GlitchSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as GlitchSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class GlitchSettingsControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        [Header("Volume Parameters")]
        public GlitchSettings settings;
        
        // variables
        public float glitchHorizontalIntensity;
public float scanlineJitter;
public float jitterBlockSize;
public float snowFlakeAmplitude;
public float verticalJump;
public float horizontalShake;
public float colorDrift;
public float colorDriftSpeed;
public LayerMask layer;
public int stencilRef;


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
            settings.glitchHorizontalIntensity.value = glitchHorizontalIntensity;
settings.scanlineJitter.value = scanlineJitter;
settings.jitterBlockSize.value = jitterBlockSize;
settings.snowFlakeAmplitude.value = snowFlakeAmplitude;
settings.verticalJump.value = verticalJump;
settings.horizontalShake.value = horizontalShake;
settings.colorDrift.value = colorDrift;
settings.colorDriftSpeed.value = colorDriftSpeed;
settings.layer.value = layer;
settings.stencilRef.value = stencilRef;

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
            glitchHorizontalIntensity = settings.glitchHorizontalIntensity.value;
scanlineJitter = settings.scanlineJitter.value;
jitterBlockSize = settings.jitterBlockSize.value;
snowFlakeAmplitude = settings.snowFlakeAmplitude.value;
verticalJump = settings.verticalJump.value;
horizontalShake = settings.horizontalShake.value;
colorDrift = settings.colorDrift.value;
colorDriftSpeed = settings.colorDriftSpeed.value;
layer = settings.layer.value;
stencilRef = settings.stencilRef.value;

        }
    }
}
