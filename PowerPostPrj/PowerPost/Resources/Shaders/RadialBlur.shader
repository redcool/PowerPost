Shader "Hidden/PowerPost/RadialBlur"
{
    Properties
    {
        // _CameraOpaqueTexture ("Texture", 2D) = "white" {}

        _Center("_Center",vector) = (0.5,0.5,0,0)
        _RadiusMin("_RadiusMin",range(0,1)) = 0
        _RadiusMax("_RadiusMax",range(0,1)) = 1
        _BlurSize("_BlurSize",float) = 0.5
        _RadialST("_RadialST",vector) = (1,1,1,1)

        // noise
        _DissolveRate("_DissolveRate",range(0,1)) = 0
        
        // baseLineMap rotate
        _RotateRate("_RotateRate",range(0,1)) = 0
        _BaseLineMapIntensity("_BaseLineMapIntensity",range(0,1)) = 0.5

        _MinColor("_MinColor",color) = (0,0,0,0)
        _MaxColor("_MaxColor",color) = (1,1,1,1)
        _GrayRange("_GrayRange",vector) = (0,1,0,0)
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
            #pragma multi_compile_local_fragment _ RADIAL_TEX_ON
            #pragma shader_feature_local_fragment _GRAY_SCALE_ON
            #pragma shader_feature_local_fragment _NOISE_MAP_ON
            #pragma shader_feature_local_fragment _BASE_LINE_MAP_ON
            #pragma shader_feature_local_fragment _ATTEN_MAP_ON
            #pragma shader_feature_local_fragment _ATTEN_MAP2_ON

            #include "PowerPostLib.hlsl"
            TEXTURE2D(_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);
            // float4 _CameraOpaqueTexture_TexelSize;

            TEXTURE2D(_BlurRT);SAMPLER(sampler_BlurRT);
            TEXTURE2D(_RadialTex);SAMPLER(sampler_RadialTex);
            TEXTURE2D(_NoiseMap);SAMPLER(sampler_NoiseMap);
            TEXTURE2D(_AttenMap);SAMPLER(sampler_AttenMap);
            TEXTURE2D(_AttenMap2);SAMPLER(sampler_AttenMap2);
            TEXTURE2D(_BaseLineMap);SAMPLER(sampler_BaseLineMap);
            float4 _BaseLineMap_TexelSize;

CBUFFER_START(UnityPerMaterial)
            float2 _Center;
            float _RadiusMin,_RadiusMax;
            float _BlurSize; // blurSize,blur size offset

            float4 _RadialInfo;
            float4 _NoiseMapST;
            float4 _RadialIntensityRange;
            half4 _RadialColor;

            half4 _AttenMap_ST;
            half4 _AttenMap2_ST;
            half _DissolveRate;

            half _RotateRate;
            half _BaseLineMapIntensity;
            half4 _BaseLineMap_ST;

            half4 _MaxColor,_MinColor;
            half4 _GrayRange;
CBUFFER_END
            float _Aspect; // camera rarget's width/height

            #define NUM_SAMPLES 20
            #define _RadialST _RadialInfo.xy
            #define _RadialLength _RadialInfo.z
            #define _NoiseMapScale _RadialInfo.w
            #define _MinRadialIntensity _RadialIntensityRange.x
            #define _MaxRadialIntensity _RadialIntensityRange.y

            #define _MinGray _GrayRange.x
            #define _MaxGray _GrayRange.y

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float2 dir = i.texcoord - _Center.xy;
                dir.x *= _Aspect;

                float dirLen = length(dir);
                float dist = smoothstep(_RadiusMin,_RadiusMax,dirLen);

                half4 mainTex = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,sampler_CameraOpaqueTexture, i.texcoord);
                
                float blurSize = max(0.0001,_BlurSize.x);
                // float4 blurCol = SampleDirBlur(_BlurRT,sampler_BlurRT,i.texcoord,dir / dirLen * blurSize);
                half4 blurCol =0;

                dir = normalize(dir);
                for(int x=0;x<NUM_SAMPLES;x++){
                    half4 c = SAMPLE_TEXTURE2D(_BlurRT,sampler_BlurRT,i.texcoord+(float(x)/NUM_SAMPLES)*dir * _BlurSize);
                    blurCol += c;
                }
                blurCol /= NUM_SAMPLES;

//==================== radial tex
                #if defined(RADIAL_TEX_ON)
                {
                    float2 polarUV = ToPolar((i.texcoord - _Center)*2);
                    polarUV.x = frac(polarUV.x);
                    
                    half dissolve = 1;

                    //使用图控制衰减
                    #if defined(_ATTEN_MAP_ON)
                    {
                        dissolve = SAMPLE_TEXTURE2D(_AttenMap,sampler_AttenMap,polarUV * _AttenMap_ST.xy + _AttenMap_ST.zw).x;

                        #if defined(_ATTEN_MAP2_ON)
                        {
                            half attenMap2 = SAMPLE_TEXTURE2D(_AttenMap2,sampler_AttenMap2,polarUV * _AttenMap2_ST.xy+_AttenMap2_ST.zw).x;
                            dissolve *= attenMap2;
                        }
                        #endif
                    }
                    #endif

                    //dissolve rate
                    half dirLen1 = smoothstep(0.,1,saturate(1-dirLen)); //从内向外衰减
                    dissolve -= dirLen1 * _DissolveRate*4; //组合径向距离

                    polarUV *= _RadialST;
                    // polarUV = lerp(polarUV,i.texcoord,noise);
                    #if defined(_NOISE_MAP_ON)
                    {
                        half noise = SAMPLE_TEXTURE2D(_NoiseMap,sampler_NoiseMap,i.texcoord * _NoiseMapST.xy + _NoiseMapST.zw).x;
                        noise *= _NoiseMapScale;
                        polarUV += noise;
                    }
                    #endif

                    half4 radialTex = SAMPLE_TEXTURE2D(_RadialTex,sampler_RadialTex,polarUV);
                    half radialIntensity = saturate(radialTex.x - .5);
                    radialIntensity = smoothstep(_MinRadialIntensity,_MaxRadialIntensity,radialIntensity);
                    radialIntensity *= dissolve;
                    // return radialIntensity;
                    blurCol = lerp(blurCol,_RadialColor,saturate(radialIntensity));
                }
                #endif
//==================== composite tex
                half4 col = lerp(mainTex,blurCol,dist);

//==================== Gray sclae
                #if defined(_GRAY_SCALE_ON)
                    half gray = dot(half3(0.2,0.7,0.07),col.xyz);
                    gray = smoothstep(_MinGray,_MaxGray,gray);
                    col = lerp(_MinColor,_MaxColor,gray);
                    // return smoothstep(0.,1,gray);
                #endif

//==================== BaseLine
                #if defined(_BASE_LINE_MAP_ON)
                    float2 uvScale = _ScreenParams.xy/(_BaseLineMap_TexelSize.zw * _BaseLineMap_ST.xy);
                    float2 baseLineMapUV = (i.texcoord + _BaseLineMap_ST.zw - 0.5);
                    // rotate
                    baseLineMapUV = ToPolar(baseLineMapUV * uvScale);
                    baseLineMapUV.x += _RotateRate;
                    baseLineMapUV = ToCartesian(baseLineMapUV);
                    baseLineMapUV += 0.5;

                    // return frac(baseLineMapUV.x);
                    half4 baseLineMap = SAMPLE_TEXTURE2D(_BaseLineMap,sampler_BaseLineMap,baseLineMapUV);
                    // return baseLineMap;
                    col = lerp(col,baseLineMap,_BaseLineMapIntensity);
                    // col += baseLineMap * _BaseLineMapIntensity;
                #endif

                return saturate(col);
            }
            ENDHLSL
        }

    }
}
