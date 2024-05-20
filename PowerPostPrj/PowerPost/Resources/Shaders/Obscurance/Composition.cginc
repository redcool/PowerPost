

#include "Common.cginc"

// Gamma encoding (only needed in gamma lighting mode)
float EncodeAO(float x)
{
    float x_g = 1 - max(1.055 * pow(1 - x, 0.416666667) - 0.055, 0);
    // ColorSpaceLuminance.w == 0 (gamma) or 1 (linear)
    return lerp(x_g, x, unity_ColorSpaceLuminance.w);
}

// Geometry-aware bilateral filter (single pass/small kernel)
float4 BlurSmall(sampler2D tex, float2 uv, float2 delta)
{
    float4 p0 = tex2D(tex, uv);
    float4 p1 = tex2D(tex, uv + float2(-delta.x, -delta.y));
    float4 p2 = tex2D(tex, uv + float2(+delta.x, -delta.y));
    float4 p3 = tex2D(tex, uv + float2(-delta.x, +delta.y));
    float4 p4 = tex2D(tex, uv + float2(+delta.x, +delta.y));

    float3 n0 = GetPackedNormal(p0);

    float w0 = 1;
    float w1 = CompareNormal(n0, GetPackedNormal(p1));
    float w2 = CompareNormal(n0, GetPackedNormal(p2));
    float w3 = CompareNormal(n0, GetPackedNormal(p3));
    float w4 = CompareNormal(n0, GetPackedNormal(p4));

    float s;
    s  = GetPackedAO(p0) * w0;
    s += GetPackedAO(p1) * w1;
    s += GetPackedAO(p2) * w2;
    s += GetPackedAO(p3) * w3;
    s += GetPackedAO(p4) * w4;

    return float4(s / (w0 + w1 + w2 + w3 + w4),n0);
}

// Final composition shader
/**
    _MainTex (normal,ao)
    _CameraOpaqueTexture
*/
float3 _AOColor;
float4 frag_composition(v2f i) : SV_Target
{
    float2 delta = _SSAOMask_TexelSize.xy / _Downsample;
    // float4 aoNormal = BlurSmall(_SSAOMask, i.uvAlt, delta);
    float4 aoNormal = tex2D(_SSAOMask,i.uv);
    float ao = aoNormal.x;
    float3 normal = aoNormal.yzw;

    float3 sh = ShadeSH9(float4(normal,1));
    // return sh.xyzx;
    
    float4 color = tex2D(_MainTex, i.uv);
    // color.rgb *= 1 - EncodeAO(ao);
    color.rgb = lerp(color.rgb,color.rgb*_AOColor,ao);
    return color;
}

// Final composition shader (ambient-only mode)
v2f_img vert_composition_gbuffer(appdata_img v)
{
    v2f_img o;
    o.pos = v.vertex * float4(2, 2, 0, 0) + float4(0, 0, 0, 1);
#if UNITY_UV_STARTS_AT_TOP
    o.uv = v.texcoord * float2(1, -1) + float2(0, 1);
#else
    o.uv = v.texcoord;
#endif
    return o;
}

#if !defined(SHADER_API_GLES) // excluding the MRT pass under GLES2

struct CompositionOutput
{
    float4 gbuffer0 : SV_Target0;
    float4 gbuffer3 : SV_Target1;
};

CompositionOutput frag_composition_gbuffer(v2f_img i)
{
    // Workaround: _OcclusionTexture_Texelsize hasn't been set properly
    // for some reasons. Use _ScreenParams instead.
    float2 delta = (_ScreenParams.zw - 1) / _Downsample;
    float ao = BlurSmall(_MainTex, i.uv, delta);

    CompositionOutput o;
    o.gbuffer0 = float4(0, 0, 0, ao);
    o.gbuffer3 = float4((float3)EncodeAO(ao), 0);
    return o;
}

#else

float4 frag_composition_gbuffer(v2f_img i) : SV_Target0
{
    return 0;
}

#endif
