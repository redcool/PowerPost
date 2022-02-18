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



            half4 frag (VaryingsDefault i) : SV_Target
            {
                half2 depthUV = i.texcoord;
                depthUV.y = 1-depthUV.y;

                half4 mainTex = (SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord));

                half depth = Linear01Depth(SAMPLE_TEXTURE2D(_DepthTex,sampler_DepthTex, depthUV),_ZBufferParams);
                half depthDelta = CalcDepthDeltaAround(_DepthTex,sampler_DepthTex,depth,depthUV,_OutlineWidth * _DepthTex_TexelSize.xy);

                mainTex.xyz = lerp(mainTex,_OutlineColor,smoothstep(0.1,0.2,depthDelta));
                return mainTex;
            }
            ENDHLSL
        }
    }
}
