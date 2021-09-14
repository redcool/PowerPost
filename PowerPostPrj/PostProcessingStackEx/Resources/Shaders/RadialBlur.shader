Shader "Hidden/Custom/RadialBlur"
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

            #include "PostLib.hlsl"

            TEXTURE2D_SAMPLER2D(_MainTex,sampler_MainTex);

            float2 _Center;
            float _RadiusMin,_RadiusMax;
            float2 _BlurSize; // blurSize,blur size offset
            float _Aspect; // camera rarget's width/height

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float2 dir = i.texcoord - _Center.xy;
                dir.x *= _Aspect;

                float dist = length(dir);
                dist = smoothstep(_RadiusMin,_RadiusMax,dist);

                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);

                float blurSize = _BlurSize.x + _BlurSize.y;
                float4 blurCol = SampleDirBlur(_MainTex,sampler_MainTex,i.texcoord,normalize(dir) * blurSize);

                return lerp(col,blurCol,dist);
            }
            ENDHLSL
        }
    }
}
