#if !defined(POWER_POST_LIB_HLSL)
#define POWER_POST_LIB_HLSL
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Filtering.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
#include "../../../../../PowerShaderLib/Lib/CoordinateSystem.hlsl"

half4 SampleBox(TEXTURE2D_PARAM(tex,state), half4 texel, half2 uv, half delta) {
	half2 p = texel.xy * delta;
	half4 c = SAMPLE_TEXTURE2D(tex, state, uv + half2(-1, -1) * p);
	c += SAMPLE_TEXTURE2D(tex, state, uv + half2(1, -1) * p);
	c += SAMPLE_TEXTURE2D(tex, state, uv + half2(-1, 1) * p);
	c += SAMPLE_TEXTURE2D(tex, state, uv + half2(1, 1) * p);

	return c * 0.25;
}

half4 SampleBox(TEXTURE2D_PARAM(tex,state),half4 texel,half2 uv, half delta,half sideWeight,half centerWeight) {
    half2 p = texel.xy * delta;
    half4 c = SAMPLE_TEXTURE2D(tex,state,uv + half2(-1,-1) * p) * sideWeight;
    c += SAMPLE_TEXTURE2D(tex,state,uv + half2(1,-1) * p) * sideWeight;
    c += SAMPLE_TEXTURE2D(tex,state,uv + half2(-1,1) * p) * sideWeight;
    c += SAMPLE_TEXTURE2D(tex,state,uv + half2(1,1) * p) * sideWeight;
    //c += SAMPLE_TEXTURE2D(tex,state,uv) * centerWeight;
    return c;
}


//----------------
// Gaussian blur
//----------------
const static half gaussWeights[4]={0.00038771,0.01330373,0.11098164,0.22508352};

half4 GaussBlur(TEXTURE2D_PARAM(tex,sampler_tex),half2 uv,half2 offset,bool samplerCenter){
    half4 c = 0;
    if(samplerCenter){
        c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv) * gaussWeights[3];
    }
    c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv + offset) * gaussWeights[2];
    c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv - offset) * gaussWeights[2];

    c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv + offset * 2) * gaussWeights[1];
    c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv - offset * 2) * gaussWeights[1];

    c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv + offset * 3) * gaussWeights[0];
    c += SAMPLE_TEXTURE2D(tex,sampler_tex,uv - offset * 3) * gaussWeights[0];
    return c;
}

half4 SampleGaussian(TEXTURE2D_PARAM(tex,state), half2 texel, half2 uv) {
	half4 c = GaussBlur(tex,state,uv,half2(texel.x,0),true);
	c += GaussBlur(tex,state,uv,half2(0,texel.y),true);
	return c;
}


//----------------
// Dir blur
//----------------
#define DIR_BLUR_SAMPLES 6
const static half dirBlurWeights[DIR_BLUR_SAMPLES] = {-0.03,-0.02,-0.01,0.01,0.02,0.03};
half4 SampleDirBlur(TEXTURE2D_PARAM(tex,state),half2 uv,half2 dir){
    half4 c = 0;
    for(int i=0;i<DIR_BLUR_SAMPLES;i++){
        c += SAMPLE_TEXTURE2D(tex,state,(uv + dir * dirBlurWeights[i]));
    }
    return c / (DIR_BLUR_SAMPLES);
}


half Gray(half3 c){
    return dot(half3(0.2,0.7,0.07),c);
}

half N21(half2 p){
    return frac(sin(dot(p,half2(100,7890)))*12345);
}

half N11(half c,half x=9.456){
    return frac(sin(c+x) * 12345.6789);
}

struct AttributesDefault{
    half4 vertex:POSITION;
    half2 texcoord:TEXCOORD;
};

struct VaryingsDefault{
    half4 vertex:SV_POSITION;
    half2 texcoord:TEXCOORD;
};

/** fullscreen quad
    1
    0 
    -1 0 1
*/
VaryingsDefault VertDefault(AttributesDefault input){
    VaryingsDefault output;
    output.vertex = half4(input.vertex.xy,0,1);
    output.texcoord = input.texcoord;//(input.vertex.xy + 1) * 0.5;
    return output;
}

/** fullscreen triangle
    3
    .
    1    .
    0
    -1 0 1 . 3
*/
const static half2 TRIANGLE_VERTEIES[3] = {-1,-1,-1,3,3,-1};
const static half2 TRIANGLE_UV[3] = {0,0,0,2,2,0};

VaryingsDefault VertDefault1(uint vid:SV_VertexID){
    VaryingsDefault output;
    output.vertex = half4(TRIANGLE_VERTEIES[vid],0,1);
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
static half2 offsets[OFFSET_COUNT] = {
    half2(-1,0),
    half2(0,1),
    half2(1,0),
    half2(0,-1)
};

half CalcDepthDeltaAround(TEXTURE2D_PARAM(depthTex,state),half depth,half2 depthUV,half scale){
    half delta = 0;
    for( int x = 0;x < OFFSET_COUNT ;x++){
        delta += depth - Linear01Depth(SAMPLE_TEXTURE2D(depthTex,state, depthUV + offsets[x] * scale),_ZBufferParams);
    }
    return delta;
}

#endif //POWER_POST_LIB_HLSL