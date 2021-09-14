Shader "Hidden/Custom/Glitch"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
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

            #include "PostLib.hlsl"

            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;
            float2 _ScanlineJitter;

            float2 _VerticalJump;
            float2 _SnowFlake; // frequency,amplitude
            float _HorizontalShake;
            float2 _ColorDrift;

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float2 snowFlake = (N21(i.texcoord * _SnowFlake.x )) * _SnowFlake.y;

                float u = i.texcoord.x;
                float v = i.texcoord.y;
                float jitter = N21(float2(v,_Time.x)) * 2-1;
                jitter *= step(_ScanlineJitter.y,abs(jitter)) * _ScanlineJitter.x;

                float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);

                float hshake = N21(float2(_Time.x,2)-0.5) * _HorizontalShake;
                float drift = sin(jump + _ColorDrift.y) * _ColorDrift.x;

                float4 c1 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,frac(float2(u + jitter + snowFlake.x + hshake,jump)));
                float4 c2 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,frac(float2(u + jitter + snowFlake.y + hshake + drift,jump)));
                return float4(c1.r,c2.g,c1.b,1);
            }
            ENDHLSL
        }
    }
}
