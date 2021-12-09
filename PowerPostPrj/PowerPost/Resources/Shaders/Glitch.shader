Shader "Hidden/PowerPost/Glitch"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        _StencilRef("_StencilRef",int) = 6
    }

    HLSLINCLUDE
        #include "PowerPostLib.hlsl"
        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        stencil{
            ref [_StencilRef]
            comp equal
            pass keep
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            float4 _ScanlineJitter;//(z : block size,w : intensity)

            float2 _VerticalJump;
            float2 _SnowFlake; // frequency,amplitude
            float _HorizontalShake;
            float2 _ColorDrift;

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float2 snowFlake = (N21(i.texcoord * _SnowFlake.x )) * _SnowFlake.y;

                float u = i.texcoord.x;
                float v = i.texcoord.y;
                float intensity = _ScanlineJitter.w;

                float jitterBlockSize = _ScanlineJitter.z;
                float jitter = N21(float2(v * jitterBlockSize,_Time.x)) *2-1;
                // jitter *= _ScanlineJitter.w;
                jitter *= step(_ScanlineJitter.y,abs(jitter)) * _ScanlineJitter.x;
// return jitter;
                float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);

                float hshake = N21(float2(_Time.x,2)-0.5) * _HorizontalShake;
                float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

                float u1 = (jitter + snowFlake.x + hshake) * intensity;
                float u2 = (jitter + snowFlake.x + hshake + drift ) * intensity;
                float4 c1 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,frac(float2(u + u1,jump)));
                float4 c2 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,frac(float2(u + u2 ,jump)));
                return float4(c1.r,c2.g,c1.b,1);
            }
            ENDHLSL
        }

        pass{
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            
            float4 frag (VaryingsDefault i) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);
            }
            ENDHLSL
        }
    }
}
