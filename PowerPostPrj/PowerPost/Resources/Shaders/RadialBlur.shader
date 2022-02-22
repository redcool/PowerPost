Shader "Hidden/PowerPost/RadialBlur"
{
    Properties
    {
        // _MainTex ("Texture", 2D) = "white" {}

        _Center("_Center",vector) = (0.5,0.5,0,0)
        _RadiusMin("_RadiusMin",range(0,1)) = 0
        _RadiusMax("_RadiusMax",range(0,1)) = 1
        _BlurSize("_BlurSize",float) = 0.5
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
            float4 _MainTex_TexelSize;

            TEXTURE2D(_BlurRT);SAMPLER(sampler_BlurRT);
CBUFFER_START(UnityPerMaterial)
            float2 _Center;
            float _RadiusMin,_RadiusMax;
            float _BlurSize; // blurSize,blur size offset
CBUFFER_END
            float _Aspect; // camera rarget's width/height
            float4 frag (VaryingsDefault i) : SV_Target
            {
                float2 dir = i.texcoord - _Center.xy;
                dir.x *= _Aspect;

                float dirLen = length(dir);
                float dist = smoothstep(_RadiusMin,_RadiusMax,dirLen);

                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);

                float blurSize = max(0.0001,_BlurSize.x);
                float4 blurCol = SampleDirBlur(_BlurRT,sampler_BlurRT,i.texcoord,dir / dirLen * blurSize);

                return lerp(col,blurCol,dist);
            }
            ENDHLSL
        }

    }
}
