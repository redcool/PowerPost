namespace PowerUtilities
{
    using PowerPost;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(RadialBlurSettingKFControl))]
    public class RadialBlurSettingKFControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Record Vars"))
            {
                var inst = target as RadialBlurSettingKFControl;
                inst.RecordVars();
            }
        }
    }
#endif

    public class RadialBlurSettingKFControl : MonoBehaviour
    {
        public float updateCount = 5;
        float intervalTime = 1;

        public Volume postVolume;
        RadialBlurSettings radialBlurSettings;

        [Tooltip("径向模糊")]
        [Header("Radial Blur Layer")]
        public Vector2 center = new Vector2(.5f, .5f);
        public float radiusMin = 0;
        public float radiusMax = 0.1f;
        public float blurSize = 0.1f;
        public bool roundness;

        [Tooltip("开启径向效果")]
        [Header("Radial Layer")]
        public bool radialTexOn;
        public Texture radialTex;
        public Vector2 radialScale = new Vector2(1, 1);
        [Header("Radial Line")]
        public float minRadialIntensity = 0;
        public float maxRadialIntensity = 0.2f;
        public Color radialColor = Color.black;

        [Tooltip("径向层的扰动效果")]
        [Header("Radial Layer Noise")]
        public bool noiseMapOn;
        public Texture noiseMap;
        public Vector4 noiseMapST =new Vector4(1, 1, 0, 0);
        public float noiseMapScale = 1;

        [Tooltip("径向层的溶解")]
        [Header("Radial Layer Attenuation")]
        public bool attenMapOn;
        public Texture attenMap ;
        public Vector4 attenMapST = new Vector4(1, 1, 0, 0);

        [Space(10)]
        public bool attenMap2On;
        public Texture attenMap2 ;
        public Vector4 attenMap2ST = new Vector4(1, 1, 0, 0);

        [Space(10)]
        public float dissolveRate;
        //public bool clipOn;

        [Tooltip("灰度显示")]
        [Header("Gray")]
        public bool isGrayScale;
        public float minGray = 0.1f;
        public float maxGray = 0.2f;
        public Color minColor = (Color.black);
        public Color maxColor = (Color.white);

        [Tooltip("开启底纹效果")]
        [Header("Base Line")]
        public bool isBaseLineOn;
        public Texture baseLineMap ;
        public Vector4 baseLineMap_ST = new Vector4(1, 1, 0, 0);
        public float rotateRate;
        public float baseLineMapIntensity = 0.5f;


        // Start is called before the first frame update
        void OnEnable()
        {
            if (!postVolume)
                postVolume = GetComponent<Volume>();

            if (postVolume && postVolume.profile)
            {
                postVolume.profile.TryGet(out radialBlurSettings);
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
            if (!radialBlurSettings)
                return;

            radialBlurSettings.center.value = center;
            radialBlurSettings.radiusMin.value = radiusMin;
            radialBlurSettings.radiusMax.value = radiusMax;
            radialBlurSettings.blurSize.value= blurSize;
            radialBlurSettings.roundness.value = roundness;

            radialBlurSettings.radialTexOn.value = radialTexOn;
            radialBlurSettings.radialTex.value= radialTex;
            radialBlurSettings.radialScale.value = radialScale;

            radialBlurSettings.minRadialIntensity.value = minRadialIntensity;
            radialBlurSettings.maxRadialIntensity.value = maxRadialIntensity;
            radialBlurSettings.radialColor.value = radialColor;

            radialBlurSettings.noiseMapOn.value = noiseMapOn;
            radialBlurSettings.noiseMap.value = noiseMap;
            radialBlurSettings.noiseMapST.value = noiseMapST;
            radialBlurSettings.noiseMapScale.value = noiseMapScale;

            radialBlurSettings.attenMapOn.value = attenMapOn;
            radialBlurSettings.attenMap.value = attenMap;
            radialBlurSettings.attenMapST.value = attenMapST;

            radialBlurSettings.attenMap2On.value = attenMap2On;
            radialBlurSettings.attenMap2.value= attenMap2;
            radialBlurSettings.attenMap2ST.value = attenMap2ST;

            radialBlurSettings.dissolveRate.value = dissolveRate;

            radialBlurSettings.isGrayScale.value = isGrayScale;
            radialBlurSettings.minGray.value= minGray;
            radialBlurSettings.maxGray.value= maxGray;
            radialBlurSettings.minColor.value= minColor;
            radialBlurSettings.maxColor.value= maxColor;

            radialBlurSettings.isBaseLineOn.value = isBaseLineOn;
            radialBlurSettings.baseLineMap.value = baseLineMap;
            radialBlurSettings.baseLineMap_ST.value = baseLineMap_ST;
            radialBlurSettings.rotateRate.value = rotateRate;
            radialBlurSettings.baseLineMapIntensity.value = baseLineMapIntensity;
        }

        public void RecordVars()
        {
            if (!radialBlurSettings)
            {
                postVolume = GetComponent<Volume>();
                if (postVolume && postVolume.profile)
                {
                    postVolume.profile.TryGet(out radialBlurSettings);
                }
            }

            if (!radialBlurSettings)
                return;

            center = radialBlurSettings.center.value;
            radiusMin = radialBlurSettings.radiusMin.value;
            radiusMax = radialBlurSettings.radiusMax.value;
            blurSize = radialBlurSettings.blurSize.value;
            roundness = radialBlurSettings.roundness.value;

            radialTexOn = radialBlurSettings.radialTexOn.value;
            radialTex = radialBlurSettings.radialTex.value ;
            radialScale = radialBlurSettings.radialScale.value;

            minRadialIntensity = radialBlurSettings.minRadialIntensity.value;
            maxRadialIntensity = radialBlurSettings.maxRadialIntensity.value;
            radialColor = radialBlurSettings.radialColor.value;

            noiseMapOn = radialBlurSettings.noiseMapOn.value;
            noiseMap = radialBlurSettings.noiseMap.value ;
            noiseMapST = radialBlurSettings.noiseMapST.value;
            noiseMapScale = radialBlurSettings.noiseMapScale.value;

            attenMapOn = radialBlurSettings.attenMapOn.value;
            attenMap = radialBlurSettings.attenMap.value;
            attenMapST = radialBlurSettings.attenMapST.value;

            attenMap2On = radialBlurSettings.attenMap2On.value;
            attenMap2 = radialBlurSettings.attenMap2.value;
            attenMap2ST = radialBlurSettings.attenMap2ST.value;

            dissolveRate = radialBlurSettings.dissolveRate.value;

            isGrayScale = radialBlurSettings.isGrayScale.value;
            minGray = radialBlurSettings.minGray.value;
            maxGray = radialBlurSettings.maxGray.value;
            minColor = radialBlurSettings.minColor.value;
            maxColor = radialBlurSettings.maxColor.value;

            isBaseLineOn = radialBlurSettings.isBaseLineOn.value;
            baseLineMap = radialBlurSettings.baseLineMap.value;
            baseLineMap_ST = radialBlurSettings.baseLineMap_ST.value;
            rotateRate = radialBlurSettings.rotateRate.value;
            baseLineMapIntensity = radialBlurSettings.baseLineMapIntensity.value;
        }
    }
}