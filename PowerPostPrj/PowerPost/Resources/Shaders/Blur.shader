Shader "Hidden/PowerPost/Blur"
{
    Properties
    {
        
    }

    HLSLINCLUDE

        #include "PowerPostLib.hlsl"

        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
        float _BlurSize;
        int _StepCount;
    ENDHLSL
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always


        //0 blur h
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            half4 frag(VaryingsDefault i):SV_TARGET{
                // half4 c = KawaseBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize,_BlurSize);
                // half4 c = GaussBlur(_MainTex,sampler_MainTex,i.texcoord,float2(_MainTex_TexelSize.x,0) * _BlurSize,true);
                half4 c=0;
                for(int x=0;x<_StepCount;x++){
                    float2 uvOffset = _MainTex_TexelSize.xy * _BlurSize * float2(x-_StepCount/2,0);
                    c += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord + uvOffset);
                }
                return c*rcp(_StepCount);

                return BoxBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize.xy * _BlurSize* float2(1,0),_StepCount);
            }

            ENDHLSL
        }
        //1 blur v
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            half4 frag(VaryingsDefault i):SV_TARGET{
                // half4 c = KawaseBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize,_BlurSize);
                // half4 c = GaussBlur(_MainTex,sampler_MainTex,i.texcoord,float2(0,_MainTex_TexelSize.y) * _BlurSize,true);
                half4 c=0;
                
                for(int x=0;x<_StepCount;x++){
                    float2 uvOffset = _MainTex_TexelSize.xy * _BlurSize * float2(0,x-_StepCount/2);
                    c += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord + uvOffset);
                }
                return c*rcp(_StepCount);

                // return BoxBlur(_MainTex,sampler_MainTex,i.texcoord,_MainTex_TexelSize.xy * _BlurSize* float2(0,1),_StepCount);
            }

            ENDHLSL
        }
    }
}
