Shader "Hidden/PowerPost/ToneMapping"
{
    Properties
    {
        _Saturate("_Saturate",float)=1
        _Brightness("_Brightness",float)=1
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
            #include "Lib/ToneMappers.hlsl"
            #include "Lib/ACESFitted.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            TEXTURE2D(_ColorGradingLUT);
            SAMPLER(sampler_linear_clamp);
            // SAMPLER(sampler_ColorGradingLUT);

            int _Mode;
            float _Weight;
            float _Saturate;
            float _Brightness;
            float _Scale,_Offset;

            bool _UseColorGradingLUT;
            bool _ColorGradingUseLogC;
            float3 _ColorGradingLUTParams;// (1 / lut_width, 1 / lut_height, lut_height - 1)

            float3 ApplyTone(float3 col){
                switch(_Mode){
                    case 1 : return AMDTonemapper(col);
                    case 2 : return ACESFilm(col);
                    case 3 : return Reinhard(col);
                    case 4 : return Uncharted2Tonemap(col);
                    case 5 : return TonemapWithWeight(col,_Weight);
                    case 6 : return Exposure(col,_Weight);
                    case 7 : return ACESFitted(col);
                    case 8 : return GTTone(col);
                    default: return col;
                }
            }

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                // float4 colorGradingTex = SAMPLE_TEXTURE2D(_ColorGradingLUT,sampler_ColorGradingLUT,i.texcoord);
                // return colorGradingTex;
                if(_UseColorGradingLUT){
                    col.xyz = ApplyLut2D(_ColorGradingLUT,sampler_linear_clamp,_ColorGradingUseLogC?LinearToLogC(col.xyz):col.xyz,_ColorGradingLUTParams);
                // col.xyz = LogCToLinear(col.xyz);
                // col.xyz = LinearToLogC(col.xyz);
                    return col;
                }
                col.xyz = ApplyTone(col.xyz);

                half3 hsv = RgbToHsv(col.xyz);

                half h = hsv.x * _Scale + _Offset;
                half s = hsv.y * _Saturate;
                half v = hsv.z *  _Brightness;

                col.xyz = HsvToRgb(half3(h,s,v));
                col.xyz = saturate(col.xyz);
                return col;
            }
            ENDHLSL
        }
    }
}
