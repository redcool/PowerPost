Shader "Hidden/PowerPost/SSAO_Depth"
{
    Properties
    {
        
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        // ao pass
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

            TEXTURE2D(_DepthTex);SAMPLER(sampler_DepthTex);
            half4 _DepthTex_TexelSize;

            TEXTURE2D(_NoiseMap);SAMPLER(sampler_NoiseMap);

            // Params
float4 _BlurOffset;
float4 _SSAOParams;
float4 _SourceSize;

// SSAO Settings
#define INTENSITY _SSAOParams.x
#define RADIUS _SSAOParams.y
#define DOWNSAMPLE _SSAOParams.z

// GLES2: In many cases, dynamic looping is not supported.
#if defined(SHADER_API_GLES) && !defined(SHADER_API_GLES3)
    #define SAMPLE_COUNT 3
#else
    #define SAMPLE_COUNT _SSAOParams.w
#endif

// Function defines
#define SCREEN_PARAMS        GetScaledScreenParams()
// #define SAMPLE_BASEMAP(uv)   SAMPLE_TEXTURE2D_X(_BaseMap, sampler_BaseMap, UnityStereoTransformScreenSpaceTex(uv));
// #define SAMPLE_BASEMAP_R(uv) SAMPLE_TEXTURE2D_X(_BaseMap, sampler_BaseMap, UnityStereoTransformScreenSpaceTex(uv)).r;


// Constants
// kContrast determines the contrast of occlusion. This allows users to control over/under
// occlusion. At the moment, this is not exposed to the editor because it's rarely useful.
static const float kContrast = 0.6;

// The constant below controls the geometry-awareness of the bilateral
// filter. The higher value, the more sensitive it is.
static const float kGeometryCoeff = 0.8;

// The constants below are used in the AO estimator. Beta is mainly used for suppressing
// self-shadowing noise, and Epsilon is used to prevent calculation underflow. See the
// paper (Morgan 2011 http://goo.gl/2iz3P) for further details of these constants.
static const float kBeta = 0.002;
#define EPSILON         1.0e-4

float4 PackAONormal(float ao, float3 n)
{
    return float4(ao, n * 0.5 + 0.5);
}

float3 GetPackedNormal(float4 p)
{
    return p.gba * 2.0 - 1.0;
}

float GetPackedAO(float4 p)
{
    return p.r;
}

float EncodeAO(float x)
{
    #if UNITY_COLORSPACE_GAMMA
        return 1.0 - max(LinearToSRGB(1.0 - saturate(x)), 0.0);
    #else
        return x;
    #endif
}

float CompareNormal(float3 d1, float3 d2)
{
    return smoothstep(kGeometryCoeff, 1.0, dot(d1, d2));
}

float2 GetScreenSpacePosition(float2 uv)
{
    return uv * SCREEN_PARAMS.xy * DOWNSAMPLE;
}

// Trigonometric function utility
float2 CosSin(float theta)
{
    float sn, cs;
    sincos(theta, sn, cs);
    return float2(cs, sn);
}

// Pseudo random number generator with 2D coordinates
float UVRandom(float u, float v)
{
    float f = dot(float2(12.9898, 78.233), float2(u, v));
    return frac(43758.5453 * sin(f));
}

// Sample point picker
float3 PickSamplePoint(float2 uv, float randAddon, int index)
{
    float2 positionSS = GetScreenSpacePosition(uv);
    float gn = InterleavedGradientNoise(positionSS, index);
    float u = frac(UVRandom(0.0, index + randAddon) + gn) * 2.0 - 1.0;
    float theta = (UVRandom(1.0, index + randAddon) + gn) * TWO_PI;
    return float3(CosSin(theta) * sqrt(1.0 - u * u), u);
}

float RawToLinearDepth(float rawDepth)
{
        return LinearEyeDepth(rawDepth, _ZBufferParams);
}


float SampleAndGetLinearDepth(float2 uv)
{
    float rawDepth = SampleSceneDepth(uv.xy).r;
    return RawToLinearDepth(rawDepth);
}

float3 ReconstructViewPos(float2 uv, float depth, float2 p11_22, float2 p13_31)
{
        float3 viewPos = float3(depth * ((uv.xy * 2.0 - 1.0 - p13_31) * p11_22), depth);
    return viewPos;
}


void SampleDepthNormalView(float2 uv, float2 p11_22, float2 p13_31, out float depth, out float3 normal, out float3 vpos)
{
    depth  = SampleAndGetLinearDepth(uv);
    vpos = ReconstructViewPos(uv, depth, p11_22, p13_31);

    normal = normalize(cross(ddy(vpos), ddx(vpos)));
}

float3x3 GetCoordinateConversionParameters(out float2 p11_22, out float2 p13_31)
{
    float3x3 camProj = (float3x3)unity_CameraProjection;

    p11_22 = rcp(float2(camProj._11, camProj._22));
    p13_31 = float2(camProj._13, camProj._23);

    return camProj;
}
float4 SSAO(float2 uv){
// Parameters used in coordinate conversion
    float2 p11_22, p13_31;
    float3x3 camProj = GetCoordinateConversionParameters(p11_22, p13_31);

    // Get the depth, normal and view position for this fragment
    float depth_o;
    float3 norm_o;
    float3 vpos_o;
    SampleDepthNormalView(uv, p11_22, p13_31, depth_o, norm_o, vpos_o);

    // This was added to avoid a NVIDIA driver issue.
    float randAddon = uv.x * 1e-10;

    float rcpSampleCount = rcp(SAMPLE_COUNT);
    float ao = 0.0;
    for (int s = 0; s < int(SAMPLE_COUNT); s++)
    {

        // Sample point
        float3 v_s1 = PickSamplePoint(uv, randAddon, s);

        // Make it distributed between [0, _Radius]
        v_s1 *= sqrt((s + 1.0) * rcpSampleCount ) * RADIUS;

        v_s1 = faceforward(v_s1, -norm_o, v_s1);
// return v_s1.xyzx;
        float3 vpos_s1 = vpos_o + v_s1;
        // Reproject the sample point
        float3 spos_s1 = mul(camProj, vpos_s1);
return spos_s1.xyzx;        
            float2 uv_s1_01 = clamp((spos_s1.xy * rcp(vpos_s1.z) + 1.0) * 0.5, 0.0, 1.0);

        // Depth at the sample point
        float depth_s1 = SampleAndGetLinearDepth(uv_s1_01);
return depth_s1 /100;
        // Relative position of the sample point
        float3 vpos_s2 = ReconstructViewPos(uv_s1_01, depth_s1, p11_22, p13_31);
        float3 v_s2 = vpos_s2 - vpos_o;

        // Estimate the obscurance value
        float a1 = max(dot(v_s2, norm_o) - kBeta * depth_o, 0.0);
        float a2 = dot(v_s2, v_s2) + EPSILON;
        ao += a1 * rcp(a2);
    }

    // Intensity normalization
    ao *= RADIUS;
return ao;
    // Apply contrast
    ao = PositivePow(ao * INTENSITY * rcpSampleCount, kContrast);
    // return PackAONormal(ao, norm_o);
}
            // Distance-based AO estimator based on Morgan 2011
            // "Alchemy screen-space ambient obscurance algorithm"
            // http://graphics.cs.williams.edu/papers/AlchemyHPG11/
            float4 frag(VaryingsDefault input) : SV_Target
            {
                // return SampleSceneDepth(input.texcoord);
                return SSAO(input.texcoord);
            }
            ENDHLSL
        }
    }
}
