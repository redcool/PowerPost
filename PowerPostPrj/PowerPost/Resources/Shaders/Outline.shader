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

            CBUFFER_START(UnityPerMaterial)
            half _OutlineWidth;
            half4 _OutlineColor;
            half _Smoothness;
            CBUFFER_END

            half4 frag (VaryingsDefault i) : SV_Target
            {
                float2 depthUV = i.texcoord;
                // depthUV.y = 1-depthUV.y;

                half4 mainTex = (SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord));

                float depth = Linear01Depth(SAMPLE_TEXTURE2D(_DepthTex,sampler_DepthTex, depthUV).x,_ZBufferParams);
                float depthDelta = CalcDepthDeltaAround(_DepthTex,sampler_DepthTex,depth,depthUV,_OutlineWidth * _DepthTex_TexelSize.xy);
                // float depthDelta = fwidth(depth);
                
                mainTex.xyz = lerp(mainTex,_OutlineColor,smoothstep(0,_Smoothness,depthDelta)).xyz;
                return mainTex;
            }
            ENDHLSL
        }
    }
}
