Shader "Hidden/PowerPost/WhiteBalance"
{
    Properties
    {
        // _Temperature("_Temperature",range(-1,1)) = 0
        // _Tint("_Tint",range(-1,1)) = 0
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

            // half _Temperature,_Tint;
            half3 _ColorBalance;

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);

                float3 colorLinear = col.xyz;

                float3 colorLMS = LinearToLMS(colorLinear);
                colorLMS *= _ColorBalance.xyz;
                colorLinear = LMSToLinear(colorLMS);

                col.xyz = colorLinear;
                // Unity_WhiteBalance_float(col.xyz,_Temperature,_Tint,col.xyz/**/);

                return col;
            }
            ENDHLSL
        }
    }
}
