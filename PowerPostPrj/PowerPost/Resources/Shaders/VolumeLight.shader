Shader "Hidden/PowerPost/VolumeLight"
{
    Properties
    {
        
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE

            #include "../../../../../PowerShaderLib/UrpLib/URP_MainLightShadows.hlsl"
            #include "../../../../../PowerShaderLib/Lib/NoiseLib.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Random.hlsl"
// #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

            #define RAY_MAX_LENGTH 20
            float _StepCount;
            float _Intenstiy;

            float GetLightAtten(float3 pos){
                float4 shadowPos = TransformWorldToShadowCoord(pos);
                // float atten = MainLightRealtimeShadow(shadowPos);
                float atten = CalcShadow(shadowPos,pos);
                return atten;
            }

            #define random(seed) sin(seed * 641.5467987313875 + 1.943856175)

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);

                float depth = GetScreenDepth(i.texcoord);
                // depth = trunc(depth*10000)/10000;
                float3 worldPos = ComputeWorldSpacePosition(i.texcoord,depth,UNITY_MATRIX_I_VP);
                float3 startPos = _WorldSpaceCameraPos.xyz;
                float3 dir = normalize(worldPos - startPos);
                float rayLength = length(worldPos - startPos);
                rayLength = min(rayLength,RAY_MAX_LENGTH);
                float3 finalPos = startPos + dir * rayLength;

                float3 intensity = 0;
                float2 step = 1.0/_StepCount;
                step.y *=0.4;

                float seed = random(i.texcoord.y * _ScreenParams.y + i.texcoord.x);
                // seed = N21(i.texcoord * _ScreenParams.xy * 1);
                // seed = InterleavedGradientNoise(i.texcoord*_ScreenParams.xy,0);
                for(float i=0;i<1;i+=step.x){
                    seed = random(seed);
                    float3 curPos = lerp(startPos,finalPos,i + seed * step.y);
                    float atten = GetLightAtten(curPos) * _Intenstiy;

                    float3 light = atten;
                    intensity += light;
                }
                intensity *= rcp(_StepCount);
                // col.xyz *= lerp(1,intensity , _Intenstiy);
                col.xyz += intensity;
                
                return col;
            }
            ENDHLSL
        }
    }
}
