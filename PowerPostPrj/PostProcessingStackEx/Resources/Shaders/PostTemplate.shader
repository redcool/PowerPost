Shader "Hidden/Custom/RadialBlur"
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

            TEXTURE2D_SAMPLER2D(_MainTex,sampler_MainTex);


            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                
                return col;
            }
            ENDHLSL
        }
    }
}
