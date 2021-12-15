Shader "Hidden/PowerPost/ToneMapping"
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
            #include "Lib/ToneMappers.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            int _Mode;
            float _Weight;

            float3 ApplyTone(float3 col){
                switch(_Mode){
                    case 0 : return col;
                    case 1 : return AMDTonemapper(col);
                    case 2 : return ACESFilm(col);
                    case 3 : return Reinhard(col);
                    case 4 : return Uncharted2Tonemap(col);
                    case 5 : return TonemapWithWeight(col,_Weight);
                    default:return 1;
                }
            }

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord);
                // col.xyz = Reinhard(col.xyz);
                col.xyz = ApplyTone(col.xyz);

                return col;
            }
            ENDHLSL
        }
    }
}
