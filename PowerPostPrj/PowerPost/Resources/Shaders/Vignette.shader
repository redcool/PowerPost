Shader "Hidden/PowerPost/Vignette"
{
    Properties
    {
        
    }
    HLSLINCLUDE
        #pragma vertex VertDefault
        #pragma fragment frag
        #include "PowerPostLib.hlsl"

        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

        half _Aspect;
        half2 _Center;
        half _Intensity;
        half3 _VignetteColor;
        half2 _Oval;
        half _Smoothness;
    ENDHLSL

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        // 0 simple vignette
        Pass
        {
            HLSLPROGRAM
            float4 frag (VaryingsDefault i) : SV_Target
            {
                half2 uv = (i.texcoord - _Center)*2 * half2(_Aspect,1) ;
                // apply oval
                uv /= _Oval;
                
                half dist = length(uv) * _Intensity;
                // border smoother
                dist *= (smoothstep(_Smoothness,1,dist));
                // atten
                half dist2 = dist * dist + 1;
                half dist4 = dist2 * dist2;
                half atten = 1.0/dist4;

                // smoother oval during
                atten *= smoothstep(0,1,abs(_Oval.x));
                atten *= smoothstep(0,1,abs(_Oval.y));
                
                half3 attenColor = lerp(_VignetteColor,1,atten);

                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                col.xyz *= atten;
                return col;
            }
            ENDHLSL
        }

        //1 urp vignette
        Pass
        {
            HLSLPROGRAM


            float4 frag (VaryingsDefault i) : SV_Target
            {
                half2 dist = abs(i.texcoord - _Center) * _Intensity;
                dist /= _Oval * _Oval;
                dist.x *= _Aspect;
                half atten = pow(saturate(1 - dot(dist,dist)),_Smoothness);
                atten *= smoothstep(0,1,abs(_Oval.y));
                half3 attenColor = lerp(_VignetteColor,1,atten);

                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                col.xyz *= attenColor;
                return col;
            }
            ENDHLSL
        }
    }
}
