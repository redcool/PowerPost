Shader "Hidden/PowerPost/VolumeLight"
{
    Properties
    {
        
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

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);

                float depth = SampleCameraDepthTexture(i.texcoord);
                float3 worldPos = ComputeWorldSpacePosition(i.texcoord,depth,UNITY_MATRIX_I_VP);
                
                return col;
            }
            ENDHLSL
        }
    }
}
