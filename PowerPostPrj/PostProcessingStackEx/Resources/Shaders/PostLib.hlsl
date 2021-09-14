#ifndef POSTLIB_HLSL
#define POSTLIB_HLSL

#include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

float4 SampleBox(TEXTURE2D_ARGS(tex,state), float4 texel, float2 uv, float delta) {
	float2 p = texel.xy * delta;
	float4 c = SAMPLE_TEXTURE2D(tex, state, uv + float2(-1, -1) * p);
	c += SAMPLE_TEXTURE2D(tex, state, uv + float2(1, -1) * p);
	c += SAMPLE_TEXTURE2D(tex, state, uv + float2(-1, 1) * p);
	c += SAMPLE_TEXTURE2D(tex, state, uv + float2(1, 1) * p);

	return c * 0.25;
}

float4 SampleBox(TEXTURE2D_ARGS(tex,state),float4 texel,float2 uv, float delta,float sideWeight,float centerWeight) {
    float2 p = texel.xy * delta;
    float4 c = SAMPLE_TEXTURE2D(tex,state,uv + float2(-1,-1) * p) * sideWeight;
    c += SAMPLE_TEXTURE2D(tex,state,uv + float2(1,-1) * p) * sideWeight;
    c += SAMPLE_TEXTURE2D(tex,state,uv + float2(-1,1) * p) * sideWeight;
    c += SAMPLE_TEXTURE2D(tex,state,uv + float2(1,1) * p) * sideWeight;
    //c += SAMPLE_TEXTURE2D(tex,state,uv) * centerWeight;
    return c;
}


//----------------
// Gaussian blur
//----------------
const static float gaussWeights[4]={0.00038771,0.01330373,0.11098164,0.22508352};

float4 GaussBlur(TEXTURE2D_ARGS(tex,sampler_tex),float2 uv,float2 offset,bool samplerCenter){
    float4 c = 0;
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

float4 SampleGaussian(TEXTURE2D_ARGS(tex,state), float2 texel, float2 uv) {
	float4 c = GaussBlur(tex,state,uv,float2(texel.x,0),true);
	c += GaussBlur(tex,state,uv,float2(0,texel.y),true);
	return c;
}


//----------------
// Dir blur
//----------------
#define DIR_BLUR_SAMPLES 10
const static float dirBlurWeights[DIR_BLUR_SAMPLES] = {-0.08,-0.05,-0.03,-0.02,-0.01,0.01,0.02,0.03,0.05,0.08};
float4 SampleDirBlur(TEXTURE2D_ARGS(tex,state),float2 uv,float2 dir){
    float4 c = 0;
    for(int i=0;i<DIR_BLUR_SAMPLES;i++){
        c += SAMPLE_TEXTURE2D(tex,state,frac(uv + dir * dirBlurWeights[i]));
    }
    return c * rcp(DIR_BLUR_SAMPLES);
}



float Gray(float3 c){
    return dot(float3(0.2,0.7,0.07),c);
}

float N21(float2 p,float frequency,float amplitude){
    return frac( sin(dot(p,float2(123,7890)) * frequency) *amplitude);
}

float N21(float2 p){
    return frac(sin(dot(p,float2(100,7890)))*12345);
}

#endif // POSTLIB_HLSL
