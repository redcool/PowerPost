#if !defined(POWER_POST_LIB_HLSL)
#define POWER_POST_LIB_HLSL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

#include "../../../../../PowerShaderLib/Lib/CoordinateSystem.hlsl"
#include "../../../../../PowerShaderLib/Lib/ScreenTextures.hlsl"
#include "../../../../../PowerShaderLib/Lib/NoiseLib.hlsl"
#include "../../../../../PowerShaderLib/Lib/BlurLib.hlsl"

SAMPLER(sampler_linear_clamp);

float Gray(float3 c){
    return dot(float3(0.2,0.7,0.07),c);
}

struct AttributesDefault{
    float4 vertex:POSITION;
    float2 texcoord:TEXCOORD;
};

struct VaryingsDefault{
    float4 vertex:SV_POSITION;
    float2 texcoord:TEXCOORD;
};

/** fullscreen quad
    1
    0 
    -1 0 1
*/
VaryingsDefault VertDefault(AttributesDefault input){
    VaryingsDefault output;
    output.vertex = float4(input.vertex.xy,0,1);
    output.texcoord = input.texcoord;//(input.vertex.xy + 1) * 0.5;
    #if defined(UNITY_UV_STARTS_AT_TOP)
        output.texcoord.y = 1 - output.texcoord.y;
    #endif
    return output;
}

/** fullscreen triangle
    3
    .
    1    .
    0
    -1 0 1 . 3
*/
const static float2 TRIANGLE_VERTEIES[3] = {-1,-1,-1,3,3,-1};
const static float2 TRIANGLE_UV[3] = {0,0,0,2,2,0};

VaryingsDefault VertFullscreenTriangle(uint vid:SV_VertexID){
    VaryingsDefault output;
    output.vertex = float4(TRIANGLE_VERTEIES[vid],0,1);
    output.texcoord = TRIANGLE_UV[vid];
    return output;
}

/**
    计算四周深度差之和
        .
    . depth .
        .
    
*/
#define OFFSET_COUNT 4
static float2 offsets[OFFSET_COUNT] = {
    float2(-1,0),
    float2(0,1),
    float2(1,0),
    float2(0,-1)
};

float CalcDepthDeltaAround(TEXTURE2D_PARAM(depthTex,state),float depth,float2 depthUV,float scale){
    float delta = 0;
    for( int x = 0;x < OFFSET_COUNT ;x++){
        delta += depth - Linear01Depth(SAMPLE_TEXTURE2D(depthTex,state, depthUV + offsets[x] * scale).x,_ZBufferParams);
    }
    return delta;
}

#endif //POWER_POST_LIB_HLSL