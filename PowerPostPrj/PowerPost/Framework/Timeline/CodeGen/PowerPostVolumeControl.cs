///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct GlitchSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
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

        public Type ComponentType => typeof(GlitchSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (GlitchSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.glitchHorizontalIntensity.overrideState = settings.glitchHorizontalIntensity.value != default;
settings.glitchHorizontalIntensity.value = glitchHorizontalIntensity;
settings.scanlineJitter.overrideState = settings.scanlineJitter.value != default;
settings.scanlineJitter.value = scanlineJitter;
settings.jitterBlockSize.overrideState = settings.jitterBlockSize.value != default;
settings.jitterBlockSize.value = jitterBlockSize;
settings.snowFlakeAmplitude.overrideState = settings.snowFlakeAmplitude.value != default;
settings.snowFlakeAmplitude.value = snowFlakeAmplitude;
settings.verticalJump.overrideState = settings.verticalJump.value != default;
settings.verticalJump.value = verticalJump;
settings.horizontalShake.overrideState = settings.horizontalShake.value != default;
settings.horizontalShake.value = horizontalShake;
settings.colorDrift.overrideState = settings.colorDrift.value != default;
settings.colorDrift.value = colorDrift;
settings.colorDriftSpeed.overrideState = settings.colorDriftSpeed.value != default;
settings.colorDriftSpeed.value = colorDriftSpeed;
settings.layer.overrideState = settings.layer.value != default;
settings.layer.value = layer;
settings.stencilRef.overrideState = settings.stencilRef.value != default;
settings.stencilRef.value = stencilRef;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (GlitchSettings)vc;
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
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct OutlineSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public int downSamples;
public LayerMask layer;
public Color outlineColor;
public float outlineWidth;
public float smoothness;

        public Type ComponentType => typeof(OutlineSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (OutlineSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.downSamples.overrideState = settings.downSamples.value != default;
settings.downSamples.value = downSamples;
settings.layer.overrideState = settings.layer.value != default;
settings.layer.value = layer;
settings.outlineColor.overrideState = settings.outlineColor.value != default;
settings.outlineColor.value = outlineColor;
settings.outlineWidth.overrideState = settings.outlineWidth.value != default;
settings.outlineWidth.value = outlineWidth;
settings.smoothness.overrideState = settings.smoothness.value != default;
settings.smoothness.value = smoothness;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (OutlineSettings)vc;
downSamples = settings.downSamples.value;
layer = settings.layer.value;
outlineColor = settings.outlineColor.value;
outlineWidth = settings.outlineWidth.value;
smoothness = settings.smoothness.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct OutlineExtendSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public bool outlineToggle;
public Color outlineColor;
public int thickness;
public bool useDepth;
public bool useNormals;
public bool useColor;
public float depthEdgeWidth;
public float normalEdgeWidth;
public float colorEdgeWidth;
public float distInfluence;
public float distInfluenceSmooth;
public bool outlineOnly;
public bool applyInSceneView;

        public Type ComponentType => typeof(OutlineExtendSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (OutlineExtendSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.outlineToggle.overrideState = settings.outlineToggle.value != default;
settings.outlineToggle.value = outlineToggle;
settings.outlineColor.overrideState = settings.outlineColor.value != default;
settings.outlineColor.value = outlineColor;
settings.thickness.overrideState = settings.thickness.value != default;
settings.thickness.value = thickness;
settings.useDepth.overrideState = settings.useDepth.value != default;
settings.useDepth.value = useDepth;
settings.useNormals.overrideState = settings.useNormals.value != default;
settings.useNormals.value = useNormals;
settings.useColor.overrideState = settings.useColor.value != default;
settings.useColor.value = useColor;
settings.depthEdgeWidth.overrideState = settings.depthEdgeWidth.value != default;
settings.depthEdgeWidth.value = depthEdgeWidth;
settings.normalEdgeWidth.overrideState = settings.normalEdgeWidth.value != default;
settings.normalEdgeWidth.value = normalEdgeWidth;
settings.colorEdgeWidth.overrideState = settings.colorEdgeWidth.value != default;
settings.colorEdgeWidth.value = colorEdgeWidth;
settings.distInfluence.overrideState = settings.distInfluence.value != default;
settings.distInfluence.value = distInfluence;
settings.distInfluenceSmooth.overrideState = settings.distInfluenceSmooth.value != default;
settings.distInfluenceSmooth.value = distInfluenceSmooth;
settings.outlineOnly.overrideState = settings.outlineOnly.value != default;
settings.outlineOnly.value = outlineOnly;
settings.applyInSceneView.overrideState = settings.applyInSceneView.value != default;
settings.applyInSceneView.value = applyInSceneView;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (OutlineExtendSettings)vc;
outlineToggle = settings.outlineToggle.value;
outlineColor = settings.outlineColor.value;
thickness = settings.thickness.value;
useDepth = settings.useDepth.value;
useNormals = settings.useNormals.value;
useColor = settings.useColor.value;
depthEdgeWidth = settings.depthEdgeWidth.value;
normalEdgeWidth = settings.normalEdgeWidth.value;
colorEdgeWidth = settings.colorEdgeWidth.value;
distInfluence = settings.distInfluence.value;
distInfluenceSmooth = settings.distInfluenceSmooth.value;
outlineOnly = settings.outlineOnly.value;
applyInSceneView = settings.applyInSceneView.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct RadialBlurSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public Vector2 center;
public float radiusMin;
public float radiusMax;
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

        public Type ComponentType => typeof(RadialBlurSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (RadialBlurSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.center.overrideState = settings.center.value != default;
settings.center.value = center;
settings.radiusMin.overrideState = settings.radiusMin.value != default;
settings.radiusMin.value = radiusMin;
settings.radiusMax.overrideState = settings.radiusMax.value != default;
settings.radiusMax.value = radiusMax;
settings.blurSize.overrideState = settings.blurSize.value != default;
settings.blurSize.value = blurSize;
settings.roundness.overrideState = settings.roundness.value != default;
settings.roundness.value = roundness;
settings.radialTexOn.overrideState = settings.radialTexOn.value != default;
settings.radialTexOn.value = radialTexOn;
settings.radialTex.overrideState = settings.radialTex.value != default;
settings.radialTex.value = radialTex;
settings.radialScale.overrideState = settings.radialScale.value != default;
settings.radialScale.value = radialScale;
settings.minRadialIntensity.overrideState = settings.minRadialIntensity.value != default;
settings.minRadialIntensity.value = minRadialIntensity;
settings.maxRadialIntensity.overrideState = settings.maxRadialIntensity.value != default;
settings.maxRadialIntensity.value = maxRadialIntensity;
settings.radialColor.overrideState = settings.radialColor.value != default;
settings.radialColor.value = radialColor;
settings.noiseMapOn.overrideState = settings.noiseMapOn.value != default;
settings.noiseMapOn.value = noiseMapOn;
settings.noiseMap.overrideState = settings.noiseMap.value != default;
settings.noiseMap.value = noiseMap;
settings.noiseMapST.overrideState = settings.noiseMapST.value != default;
settings.noiseMapST.value = noiseMapST;
settings.noiseMapScale.overrideState = settings.noiseMapScale.value != default;
settings.noiseMapScale.value = noiseMapScale;
settings.attenMapOn.overrideState = settings.attenMapOn.value != default;
settings.attenMapOn.value = attenMapOn;
settings.attenMap.overrideState = settings.attenMap.value != default;
settings.attenMap.value = attenMap;
settings.attenMapST.overrideState = settings.attenMapST.value != default;
settings.attenMapST.value = attenMapST;
settings.attenMap2On.overrideState = settings.attenMap2On.value != default;
settings.attenMap2On.value = attenMap2On;
settings.attenMap2.overrideState = settings.attenMap2.value != default;
settings.attenMap2.value = attenMap2;
settings.attenMap2ST.overrideState = settings.attenMap2ST.value != default;
settings.attenMap2ST.value = attenMap2ST;
settings.dissolveRate.overrideState = settings.dissolveRate.value != default;
settings.dissolveRate.value = dissolveRate;
settings.isGrayScale.overrideState = settings.isGrayScale.value != default;
settings.isGrayScale.value = isGrayScale;
settings.minGray.overrideState = settings.minGray.value != default;
settings.minGray.value = minGray;
settings.maxGray.overrideState = settings.maxGray.value != default;
settings.maxGray.value = maxGray;
settings.minColor.overrideState = settings.minColor.value != default;
settings.minColor.value = minColor;
settings.maxColor.overrideState = settings.maxColor.value != default;
settings.maxColor.value = maxColor;
settings.isBaseLineOn.overrideState = settings.isBaseLineOn.value != default;
settings.isBaseLineOn.value = isBaseLineOn;
settings.baseLineMap.overrideState = settings.baseLineMap.value != default;
settings.baseLineMap.value = baseLineMap;
settings.baseLineMap_ST.overrideState = settings.baseLineMap_ST.value != default;
settings.baseLineMap_ST.value = baseLineMap_ST;
settings.rotateRate.overrideState = settings.rotateRate.value != default;
settings.rotateRate.value = rotateRate;
settings.baseLineMapIntensity.overrideState = settings.baseLineMapIntensity.value != default;
settings.baseLineMapIntensity.value = baseLineMapIntensity;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (RadialBlurSettings)vc;
center = settings.center.value;
radiusMin = settings.radiusMin.value;
radiusMax = settings.radiusMax.value;
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
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct SimpleBloomSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public float intensity;
public int iterators;
public float threshold;
public float softThreshold;
public Color bloomColor;

        public Type ComponentType => typeof(SimpleBloomSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (SimpleBloomSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.intensity.overrideState = settings.intensity.value != default;
settings.intensity.value = intensity;
settings.iterators.overrideState = settings.iterators.value != default;
settings.iterators.value = iterators;
settings.threshold.overrideState = settings.threshold.value != default;
settings.threshold.value = threshold;
settings.softThreshold.overrideState = settings.softThreshold.value != default;
settings.softThreshold.value = softThreshold;
settings.bloomColor.overrideState = settings.bloomColor.value != default;
settings.bloomColor.value = bloomColor;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (SimpleBloomSettings)vc;
intensity = settings.intensity.value;
iterators = settings.iterators.value;
threshold = settings.threshold.value;
softThreshold = settings.softThreshold.value;
bloomColor = settings.bloomColor.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct SimpleDOFSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public float distance;
public float depthRange;
public float blurSize;
public bool debugMode;

        public Type ComponentType => typeof(SimpleDOFSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (SimpleDOFSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.distance.overrideState = settings.distance.value != default;
settings.distance.value = distance;
settings.depthRange.overrideState = settings.depthRange.value != default;
settings.depthRange.value = depthRange;
settings.blurSize.overrideState = settings.blurSize.value != default;
settings.blurSize.value = blurSize;
settings.debugMode.overrideState = settings.debugMode.value != default;
settings.debugMode.value = debugMode;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (SimpleDOFSettings)vc;
distance = settings.distance.value;
depthRange = settings.depthRange.value;
blurSize = settings.blurSize.value;
debugMode = settings.debugMode.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct SketchSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public bool sketchToggle;
public Color sketchColor;
public Texture sketchTexture;
public float sketchBlend;
public float lightInfluence;
public float triPlanerPower;
public int renderOffset;
public bool DebugMode;
public float sketchArea;
public float originLightness;
public float originContrast;
public bool inveseArea;
public float sketchThreshold;
public float sketchSmooth;
public float sketchSize;
public float sketchTransitionRange;
public float sketchLower;
public float sketchUpper;

        public Type ComponentType => typeof(SketchSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (SketchSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.sketchToggle.overrideState = settings.sketchToggle.value != default;
settings.sketchToggle.value = sketchToggle;
settings.sketchColor.overrideState = settings.sketchColor.value != default;
settings.sketchColor.value = sketchColor;
settings.sketchTexture.overrideState = settings.sketchTexture.value != default;
settings.sketchTexture.value = sketchTexture;
settings.sketchBlend.overrideState = settings.sketchBlend.value != default;
settings.sketchBlend.value = sketchBlend;
settings.lightInfluence.overrideState = settings.lightInfluence.value != default;
settings.lightInfluence.value = lightInfluence;
settings.triPlanerPower.overrideState = settings.triPlanerPower.value != default;
settings.triPlanerPower.value = triPlanerPower;
settings.renderOffset.overrideState = settings.renderOffset.value != default;
settings.renderOffset.value = renderOffset;
settings.DebugMode.overrideState = settings.DebugMode.value != default;
settings.DebugMode.value = DebugMode;
settings.sketchArea.overrideState = settings.sketchArea.value != default;
settings.sketchArea.value = sketchArea;
settings.originLightness.overrideState = settings.originLightness.value != default;
settings.originLightness.value = originLightness;
settings.originContrast.overrideState = settings.originContrast.value != default;
settings.originContrast.value = originContrast;
settings.inveseArea.overrideState = settings.inveseArea.value != default;
settings.inveseArea.value = inveseArea;
settings.sketchThreshold.overrideState = settings.sketchThreshold.value != default;
settings.sketchThreshold.value = sketchThreshold;
settings.sketchSmooth.overrideState = settings.sketchSmooth.value != default;
settings.sketchSmooth.value = sketchSmooth;
settings.sketchSize.overrideState = settings.sketchSize.value != default;
settings.sketchSize.value = sketchSize;
settings.sketchTransitionRange.overrideState = settings.sketchTransitionRange.value != default;
settings.sketchTransitionRange.value = sketchTransitionRange;
settings.sketchLower.overrideState = settings.sketchLower.value != default;
settings.sketchLower.value = sketchLower;
settings.sketchUpper.overrideState = settings.sketchUpper.value != default;
settings.sketchUpper.value = sketchUpper;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (SketchSettings)vc;
sketchToggle = settings.sketchToggle.value;
sketchColor = settings.sketchColor.value;
sketchTexture = settings.sketchTexture.value;
sketchBlend = settings.sketchBlend.value;
lightInfluence = settings.lightInfluence.value;
triPlanerPower = settings.triPlanerPower.value;
renderOffset = settings.renderOffset.value;
DebugMode = settings.DebugMode.value;
sketchArea = settings.sketchArea.value;
originLightness = settings.originLightness.value;
originContrast = settings.originContrast.value;
inveseArea = settings.inveseArea.value;
sketchThreshold = settings.sketchThreshold.value;
sketchSmooth = settings.sketchSmooth.value;
sketchSize = settings.sketchSize.value;
sketchTransitionRange = settings.sketchTransitionRange.value;
sketchLower = settings.sketchLower.value;
sketchUpper = settings.sketchUpper.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct SSAOSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public float intensity;
public float radius;
public bool downSample;
public int sampleCount;
public Color aoColor;

        public Type ComponentType => typeof(SSAOSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (SSAOSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.intensity.overrideState = settings.intensity.value != default;
settings.intensity.value = intensity;
settings.radius.overrideState = settings.radius.value != default;
settings.radius.value = radius;
settings.downSample.overrideState = settings.downSample.value != default;
settings.downSample.value = downSample;
settings.sampleCount.overrideState = settings.sampleCount.value != default;
settings.sampleCount.value = sampleCount;
settings.aoColor.overrideState = settings.aoColor.value != default;
settings.aoColor.value = aoColor;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (SSAOSettings)vc;
intensity = settings.intensity.value;
radius = settings.radius.value;
downSample = settings.downSample.value;
sampleCount = settings.sampleCount.value;
aoColor = settings.aoColor.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct SSSSSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public Color strength;
public Color falloff;
public float blurScale;
public LayerMask layer;
public int stencilRef;

        public Type ComponentType => typeof(SSSSSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (SSSSSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.strength.overrideState = settings.strength.value != default;
settings.strength.value = strength;
settings.falloff.overrideState = settings.falloff.value != default;
settings.falloff.value = falloff;
settings.blurScale.overrideState = settings.blurScale.value != default;
settings.blurScale.value = blurScale;
settings.layer.overrideState = settings.layer.value != default;
settings.layer.value = layer;
settings.stencilRef.overrideState = settings.stencilRef.value != default;
settings.stencilRef.value = stencilRef;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (SSSSSettings)vc;
strength = settings.strength.value;
falloff = settings.falloff.value;
blurScale = settings.blurScale.value;
layer = settings.layer.value;
stencilRef = settings.stencilRef.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct SunShaftSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public bool useRenderSettingsSun;
public Vector2 sunPos;
public bool hiddenSunShaftBackfaceSun;
public int radialBlurIterations;
public float sunShaftBlurRadius;
public Color sunThreshold;
public Color sunColor;
public float sunShaftIntensity;
public float maxRadius;

        public Type ComponentType => typeof(SunShaftSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (SunShaftSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.useRenderSettingsSun.overrideState = settings.useRenderSettingsSun.value != default;
settings.useRenderSettingsSun.value = useRenderSettingsSun;
settings.sunPos.overrideState = settings.sunPos.value != default;
settings.sunPos.value = sunPos;
settings.hiddenSunShaftBackfaceSun.overrideState = settings.hiddenSunShaftBackfaceSun.value != default;
settings.hiddenSunShaftBackfaceSun.value = hiddenSunShaftBackfaceSun;
settings.radialBlurIterations.overrideState = settings.radialBlurIterations.value != default;
settings.radialBlurIterations.value = radialBlurIterations;
settings.sunShaftBlurRadius.overrideState = settings.sunShaftBlurRadius.value != default;
settings.sunShaftBlurRadius.value = sunShaftBlurRadius;
settings.sunThreshold.overrideState = settings.sunThreshold.value != default;
settings.sunThreshold.value = sunThreshold;
settings.sunColor.overrideState = settings.sunColor.value != default;
settings.sunColor.value = sunColor;
settings.sunShaftIntensity.overrideState = settings.sunShaftIntensity.value != default;
settings.sunShaftIntensity.value = sunShaftIntensity;
settings.maxRadius.overrideState = settings.maxRadius.value != default;
settings.maxRadius.value = maxRadius;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (SunShaftSettings)vc;
useRenderSettingsSun = settings.useRenderSettingsSun.value;
sunPos = settings.sunPos.value;
hiddenSunShaftBackfaceSun = settings.hiddenSunShaftBackfaceSun.value;
radialBlurIterations = settings.radialBlurIterations.value;
sunShaftBlurRadius = settings.sunShaftBlurRadius.value;
sunThreshold = settings.sunThreshold.value;
sunColor = settings.sunColor.value;
sunShaftIntensity = settings.sunShaftIntensity.value;
maxRadius = settings.maxRadius.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct ToneMappingSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public float weight;
public float scale;
public float offset;
public float saturate;
public float brightness;
public Texture colorGradingLut;
public bool colorGradingUseLogC;

        public Type ComponentType => typeof(ToneMappingSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (ToneMappingSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.weight.overrideState = settings.weight.value != default;
settings.weight.value = weight;
settings.scale.overrideState = settings.scale.value != default;
settings.scale.value = scale;
settings.offset.overrideState = settings.offset.value != default;
settings.offset.value = offset;
settings.saturate.overrideState = settings.saturate.value != default;
settings.saturate.value = saturate;
settings.brightness.overrideState = settings.brightness.value != default;
settings.brightness.value = brightness;
settings.colorGradingLut.overrideState = settings.colorGradingLut.value != default;
settings.colorGradingLut.value = colorGradingLut;
settings.colorGradingUseLogC.overrideState = settings.colorGradingUseLogC.value != default;
settings.colorGradingUseLogC.value = colorGradingUseLogC;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (ToneMappingSettings)vc;
weight = settings.weight.value;
scale = settings.scale.value;
offset = settings.offset.value;
saturate = settings.saturate.value;
brightness = settings.brightness.value;
colorGradingLut = settings.colorGradingLut.value;
colorGradingUseLogC = settings.colorGradingUseLogC.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct VignetteSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public float intensity;
public float smoothness;
public bool rounded;
public Color color;
public float centerX;
public float centerY;
public float ovalX;
public float ovalY;

        public Type ComponentType => typeof(VignetteSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (VignetteSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.intensity.overrideState = settings.intensity.value != default;
settings.intensity.value = intensity;
settings.smoothness.overrideState = settings.smoothness.value != default;
settings.smoothness.value = smoothness;
settings.rounded.overrideState = settings.rounded.value != default;
settings.rounded.value = rounded;
settings.color.overrideState = settings.color.value != default;
settings.color.value = color;
settings.centerX.overrideState = settings.centerX.value != default;
settings.centerX.value = centerX;
settings.centerY.overrideState = settings.centerY.value != default;
settings.centerY.value = centerY;
settings.ovalX.overrideState = settings.ovalX.value != default;
settings.ovalX.value = ovalX;
settings.ovalY.overrideState = settings.ovalY.value != default;
settings.ovalY.value = ovalY;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (VignetteSettings)vc;
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
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct VolumeLightingSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public bool enabled;
public bool reverseLight;
public Color color;
public int stepCount;
public int iterators;
public int downSampers;

        public Type ComponentType => typeof(VolumeLightingSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (VolumeLightingSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.enabled.overrideState = settings.enabled.value != default;
settings.enabled.value = enabled;
settings.reverseLight.overrideState = settings.reverseLight.value != default;
settings.reverseLight.value = reverseLight;
settings.color.overrideState = settings.color.value != default;
settings.color.value = color;
settings.stepCount.overrideState = settings.stepCount.value != default;
settings.stepCount.value = stepCount;
settings.iterators.overrideState = settings.iterators.value != default;
settings.iterators.value = iterators;
settings.downSampers.overrideState = settings.downSampers.value != default;
settings.downSampers.value = downSampers;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (VolumeLightingSettings)vc;
enabled = settings.enabled.value;
reverseLight = settings.reverseLight.value;
color = settings.color.value;
stepCount = settings.stepCount.value;
iterators = settings.iterators.value;
downSampers = settings.downSampers.value;

        }
    }
}///
/// Generated Code
/// UI : ProjectSettings/PowerUtils/PostControlCodeGen
///
namespace PowerUtilities
{
    //using PowerPost;
    //other ns string
    using PowerPost;
    using System;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.Rendering.Universal;

    [Serializable]
    public struct WhiteBalanceSettings_Data : IvolumeData
    {
        // variables
        public bool isEnable;
        public float temperature;
public float tint;

        public Type ComponentType => typeof(WhiteBalanceSettings);
        public void UpdateSetting(VolumeComponent vc)
        {
            var settings = (WhiteBalanceSettings)vc;
            settings.active = isEnable;
            if(!isEnable) return;
settings.temperature.overrideState = settings.temperature.value != default;
settings.temperature.value = temperature;
settings.tint.overrideState = settings.tint.value != default;
settings.tint.value = tint;

        }

        public void RecordSetting(VolumeComponent vc)
        {
            var settings = (WhiteBalanceSettings)vc;
temperature = settings.temperature.value;
tint = settings.tint.value;

        }
    }
}