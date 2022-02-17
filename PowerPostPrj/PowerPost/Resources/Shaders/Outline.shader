Shader "Hidden/PowerPost/Outline"
{
    Properties
    {
        _OutlineWidth("_OutlineWidth",float) = 1
        _OutlineColor("_OutlineColor",color) = (1,1,1,1)
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

            TEXTURE2D(_DepthTex);SAMPLER(sampler_DepthTex);
            half4 _DepthTex_TexelSize;

            half _OutlineWidth;
            half4 _OutlineColor;

            #define OFFSET_COUNT 4
            static half2 offsets[OFFSET_COUNT] = {
                _DepthTex_TexelSize.xy * half2(-1,0),
                _DepthTex_TexelSize.xy * half2(0,1),
                _DepthTex_TexelSize.xy * half2(1,0),
                _DepthTex_TexelSize.xy * half2(0,-1)
            };

            half CalcDepthDelta(half depth,half2 depthUV,half scale){
                half delta = 0;
                for( int x = 0;x < OFFSET_COUNT ;x++){
                    delta += depth - Linear01Depth(SAMPLE_TEXTURE2D(_DepthTex,sampler_DepthTex, depthUV + offsets[x] * scale),_ZBufferParams);
                }
                return delta;
            }

            half4 frag (VaryingsDefault i) : SV_Target
            {
                half2 depthUV = i.texcoord;
                depthUV.y = 1-depthUV.y;

                half4 mainTex = (SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord));

                half depth = Linear01Depth(SAMPLE_TEXTURE2D(_DepthTex,sampler_DepthTex, depthUV),_ZBufferParams);
                half depthDelta = CalcDepthDelta(depth,depthUV,_OutlineWidth);
// return depthDelta;

                // mainTex.xyz *= lerp(1,_OutlineColor,depthDelta);
                return lerp(mainTex,_OutlineColor,smoothstep(0.1,0.2,depthDelta));
                return mainTex;

            }
            ENDHLSL
        }
    }
}
