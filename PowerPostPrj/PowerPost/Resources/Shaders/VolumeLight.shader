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


            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

            #define RAY_MAX_LENGTH 20
            float _StepCount;

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
                
                // if(0.999 < depth) return 0;
                
                float3 worldPos = ComputeWorldSpacePosition(i.texcoord,depth,UNITY_MATRIX_I_VP);
                float3 startPos = _WorldSpaceCameraPos.xyz;
                float3 dir = normalize(worldPos - startPos);
                float rayLength = length(worldPos - startPos);
                rayLength = min(rayLength,RAY_MAX_LENGTH);
                float3 finalPos = startPos + dir * rayLength;

                float3 intensity = 0;
                float2 step = 1.0/_StepCount;
                // step.y *=0.4;

                float2 screenPos = i.texcoord * _ScreenParams.xy;
                // float seed = random(i.texcoord.y * _ScreenParams.y + i.texcoord.x);
                float seed = N21(screenPos);
                // seed = InterleavedGradientNoise(i.texcoord*_ScreenParams.xy,0);
                float3 posOffset = GradientNoise(worldPos + _Time.y)*0.1;

                for(float x=0;x<1;x+=step.x){
                    seed = N21(screenPos * seed);
                    // seed = random(seed);
                    float3 curPos = lerp(startPos,finalPos,x + seed * step.x + posOffset);
                    float atten = GetLightAtten(curPos);

                    float3 light = atten;
                    intensity += light;
                }
                intensity *= step.x;
                return float4(max(0.0,intensity),1);
            }
            ENDHLSL
        }

        //1 blur
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float _BlurSize;

            half4 frag(VaryingsDefault i):SV_TARGET{
                half4 c = KawaseBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize,_BlurSize);
                return c;
            }

            ENDHLSL
        }

        //2 composite
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_LightTex);
            TEXTURE2D(_MainTex);
            half _ReverseLight;
            // half _Intenstiy;
            half4 _Color;

            half4 frag(VaryingsDefault i):SV_TARGET{
                half4 c = 0;
                half4 lightCol = SAMPLE_TEXTURE2D(_LightTex,sampler_linear_clamp,i.texcoord);
                lightCol = lerp(lightCol,1 - lightCol,_ReverseLight);
                lightCol *= _Color * _MainLightColor;
                // lightCol *= ValueNoise(i.texcoord*10 + _Time.x*10);

                half4 mainCol = SAMPLE_TEXTURE2D(_MainTex,sampler_linear_clamp,i.texcoord);
                c.xyz = mainCol.xyz + lightCol;
                return c;
            }

            ENDHLSL

        }

                //3 blur
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float _BlurSize;

            half4 frag(VaryingsDefault i):SV_TARGET{
                // half4 c = KawaseBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize,_BlurSize);
                half4 c = GaussBlur(_MainTex,sampler_MainTex,i.texcoord,float2(_MainTex_TexelSize.x,0) * _BlurSize,true);
                return c;
            }

            ENDHLSL
        }

                Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            float4 _MainTex_TexelSize;
            float _BlurSize;

            half4 frag(VaryingsDefault i):SV_TARGET{
                // half4 c = KawaseBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize,_BlurSize);
                half4 c = GaussBlur(_MainTex,sampler_MainTex,i.texcoord,float2(0,_MainTex_TexelSize.y) * _BlurSize,true);
                return c;
            }

            ENDHLSL
        }
    }
}
