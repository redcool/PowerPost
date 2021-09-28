Shader "Hidden/PowerPost/SimpleDOF"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}
    }
    HLSLINCLUDE
        #include "PowerPostLib.hlsl"
        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
		float4 _MainTex_TexelSize;

        TEXTURE2D(_DepthRT);SAMPLER(sampler_DepthRT);
        float4 _DepthRT_TexelSize;

    ENDHLSL
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            float _BlurSize;

            float _Distance;
            float _DepthRange; //清晰的范围
            float _Debug; //blur区域显示为 红色

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);
                // float4 blurCol = SampleBox(_MainTex,sampler_MainTex,_DepthRT_TexelSize,i.texcoord,_BlurSize);
                float4 blurCol = SampleGaussian(_MainTex,sampler_MainTex,_DepthRT_TexelSize.xy * _BlurSize,i.texcoord);

                float depth = SAMPLE_TEXTURE2D(_DepthRT,sampler_DepthRT,i.texcoord).r;
                depth = Linear01Depth(depth,_ZBufferParams);

                float rate = abs(depth - _Distance);
                rate = saturate(max(rate-_DepthRange,0)); // flat center point to center line
                rate = smoothstep(0.0001,0.05,rate);

                if(_Debug)
                    blurCol *= float4(1,0,0,1);
                return lerp(col,blurCol,rate);
            }
            ENDHLSL
        }
    }
}
