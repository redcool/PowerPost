Shader "Hidden/PowerPost/Default"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
        
    }

    HLSLINCLUDE
        #include "PowerPostLib.hlsl"
        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
        float4 _MainTex_TexelSize;
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

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
