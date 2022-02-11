#if !defined(POST_PROCESSING_COMMON_EX_HLSL)
#define POST_PROCESSING_COMMON_EX_HLSL

float4 _VignetteOval;

half3 ApplyVignetteOval(half3 input, float2 uv, float2 center, float intensity, float roundness, float smoothness, half3 color)
{
    center = UnityStereoTransformScreenSpaceTex(center);
    float2 dist = abs(uv - center) * intensity;
    dist /= float2(_VignetteOval.x* _VignetteOval.x,_VignetteOval.y*_VignetteOval.y);
#if defined(UNITY_SINGLE_PASS_STEREO)
    dist.x /= unity_StereoScaleOffset[unity_StereoEyeIndex].x;
#endif

    dist.x *= roundness;
    float vfactor = pow(saturate(1.0 - dot(dist, dist)), smoothness);
    float fading = smoothstep(0,1,abs(_VignetteOval.y));
    vfactor *= fading;
    
    return input * lerp(color, (1.0).xxx, vfactor);
}

#endif //POST_PROCESSING_COMMON_EX_HLSL