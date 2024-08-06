Shader "Hidden/PowerPost/DefaultBlit"
{
    Properties
    {
        
    }
    SubShader
    {
        
        // 0 copy color
        Pass
        {
            // No culling or depth
            Cull Off 
            ZWrite off
            ZTest Always

            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                return col;
            }
            ENDHLSL
        }

        // 1 copy depth(no msaa)
        Pass{
            Cull Off ZWrite on ZTest Always
             ColorMask R
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

            float frag(VaryingsDefault i) : SV_Depth
            {
                float d = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);
                return d;
            }

            ENDHLSL
        }
    }
}
