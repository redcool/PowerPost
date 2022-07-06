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
            int _Mode;
            float _Weight;
            float _Saturate;
            float _Brightness;
            float _Scale,_Offset;

            float3 ApplyTone(float3 col){
                switch(_Mode){
                    case 0 : return col;
                    case 1 : return AMDTonemapper(col);
                    case 2 : return ACESFilm(col);
                    case 3 : return Reinhard(col);
                    case 4 : return Uncharted2Tonemap(col);
                    case 5 : return TonemapWithWeight(col,_Weight);
                    case 6 : return Exposure(col,_Weight);
                    case 7 : return ACESFitted(col);
                    case 8 : return GTTone(col);
                    default:return 1;
                }
            }

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                // col.xyz = Reinhard(col.xyz);
                col.xyz = ApplyTone(col.xyz);

                // col.xyz = lerp(dot(half3(0.2,0.7,0.07),col.xyz),col.xyz,_Saturate);
                // col.xyz = lerp(0,col.xyz,_Brightness);

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
