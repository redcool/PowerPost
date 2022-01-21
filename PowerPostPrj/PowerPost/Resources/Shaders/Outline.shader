Shader "Hidden/PowerPost/Outline"
{
    Properties
    {
        // _BlurTex("_BlurTex",2d)=""{}
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

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            TEXTURE2D(_BlurTex);SAMPLER(sampler_BlurTex);

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                float4 blurTex = SAMPLE_TEXTURE2D(_BlurTex,sampler_BlurTex,i.texcoord);

                return lerp(col, float4(1,0,0,0),blurTex.x+blurTex.y+blurTex.z);
            }
            ENDHLSL
        }
    }
}
