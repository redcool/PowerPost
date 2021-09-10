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

            #include "../PostLib.hlsl"

            TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
            float4 _MainTex_TexelSize;
            float2 _ScanlineJitter;

            float2 _SnowFlake; // frequency,amplitude

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float2 snowFlake = N21(i.texcoord * _SnowFlake.x) * _SnowFlake.y;

                float u = i.texcoord.x;
                float v = i.texcoord.y;
                float jitter = N21(float2(v,_Time.x)) * 2-1;
                jitter *= step(_ScanlineJitter.y,abs(jitter)) * _ScanlineJitter.x;
                
                float4 src1 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,frac(float2(u + jitter + snowFlake.x,v)));
                float4 src2 = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,frac(float2(u + jitter + snowFlake.y,v)));
                return float4(src1.r,src2.g,src1.b,1);
            }
            ENDHLSL
        }
    }
}
