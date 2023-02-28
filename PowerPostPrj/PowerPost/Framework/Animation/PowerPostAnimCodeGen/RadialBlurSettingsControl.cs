namespace PowerUtilities
{
    using PowerPost;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(RadialBlurSettingsControl))]
    public class RadialBlurSettingsControlEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Record Vars"))
            {
                var inst = target as RadialBlurSettingsControl;
                inst.RecordVars();
            }
        }
    }
#endif
    
    [ExecuteInEditMode]
    public class RadialBlurSettingsControl : BaseSettingsControl<RadialBlurSettings>
    {
        // variables
        public Vector2 center;
public float radiusMin;
public float blurSize;
public bool roundness;
public bool radialTexOn;
public Texture radialTex;
public Vector2 radialScale;
public float minRadialIntensity;
public float maxRadialIntensity;
public Color radialColor;
public bool noiseMapOn;
public Texture noiseMap;
public Vector4 noiseMapST;
public float noiseMapScale;
public bool attenMapOn;
public Texture attenMap;
public Vector4 attenMapST;
public bool attenMap2On;
public Texture attenMap2;
public Vector4 attenMap2ST;
public float dissolveRate;
public bool isGrayScale;
public float minGray;
public float maxGray;
public Color minColor;
public Color maxColor;
public bool isBaseLineOn;
public Texture baseLineMap;
public Vector4 baseLineMap_ST;
public float rotateRate;
public float baseLineMapIntensity;


        public override void UpdateVars()
        {
            if (!settings)
                return;
            //settings.baseLineMapIntensity.value = baseLineMapIntensity;
            settings.center.value = center;
settings.radiusMin.value = radiusMin;
settings.blurSize.value = blurSize;
settings.roundness.value = roundness;
settings.radialTexOn.value = radialTexOn;
settings.radialTex.value = radialTex;
settings.radialScale.value = radialScale;
settings.minRadialIntensity.value = minRadialIntensity;
settings.maxRadialIntensity.value = maxRadialIntensity;
settings.radialColor.value = radialColor;
settings.noiseMapOn.value = noiseMapOn;
settings.noiseMap.value = noiseMap;
settings.noiseMapST.value = noiseMapST;
settings.noiseMapScale.value = noiseMapScale;
settings.attenMapOn.value = attenMapOn;
settings.attenMap.value = attenMap;
settings.attenMapST.value = attenMapST;
settings.attenMap2On.value = attenMap2On;
settings.attenMap2.value = attenMap2;
settings.attenMap2ST.value = attenMap2ST;
settings.dissolveRate.value = dissolveRate;
settings.isGrayScale.value = isGrayScale;
settings.minGray.value = minGray;
settings.maxGray.value = maxGray;
settings.minColor.value = minColor;
settings.maxColor.value = maxColor;
settings.isBaseLineOn.value = isBaseLineOn;
settings.baseLineMap.value = baseLineMap;
settings.baseLineMap_ST.value = baseLineMap_ST;
settings.rotateRate.value = rotateRate;
settings.baseLineMapIntensity.value = baseLineMapIntensity;

        }

        public override void RecordVars()
        {
            base.RecordVars();
            if (!settings)
                return;
            
            //rotateRate = settings.rotateRate.value;
            center = settings.center.value;
radiusMin = settings.radiusMin.value;
blurSize = settings.blurSize.value;
roundness = settings.roundness.value;
radialTexOn = settings.radialTexOn.value;
radialTex = settings.radialTex.value;
radialScale = settings.radialScale.value;
minRadialIntensity = settings.minRadialIntensity.value;
maxRadialIntensity = settings.maxRadialIntensity.value;
radialColor = settings.radialColor.value;
noiseMapOn = settings.noiseMapOn.value;
noiseMap = settings.noiseMap.value;
noiseMapST = settings.noiseMapST.value;
noiseMapScale = settings.noiseMapScale.value;
attenMapOn = settings.attenMapOn.value;
attenMap = settings.attenMap.value;
attenMapST = settings.attenMapST.value;
attenMap2On = settings.attenMap2On.value;
attenMap2 = settings.attenMap2.value;
attenMap2ST = settings.attenMap2ST.value;
dissolveRate = settings.dissolveRate.value;
isGrayScale = settings.isGrayScale.value;
minGray = settings.minGray.value;
maxGray = settings.maxGray.value;
minColor = settings.minColor.value;
maxColor = settings.maxColor.value;
isBaseLineOn = settings.isBaseLineOn.value;
baseLineMap = settings.baseLineMap.value;
baseLineMap_ST = settings.baseLineMap_ST.value;
rotateRate = settings.rotateRate.value;
baseLineMapIntensity = settings.baseLineMapIntensity.value;

        }
    }
}
