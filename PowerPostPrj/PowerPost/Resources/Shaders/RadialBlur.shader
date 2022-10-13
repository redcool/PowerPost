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
            #pragma shader_feature _GRAY_SCALE_ON
            #pragma shader_feature _NOISE_MAP_ON

            #include "PowerPostLib.hlsl"
            TEXTURE2D(_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);
            // float4 _CameraOpaqueTexture_TexelSize;

            TEXTURE2D(_BlurRT);SAMPLER(sampler_BlurRT);
            TEXTURE2D(_RadialTex);SAMPLER(sampler_RadialTex);
            TEXTURE2D(_NoiseMap);SAMPLER(sampler_NoiseMap);
CBUFFER_START(UnityPerMaterial)
            float2 _Center;
            float _RadiusMin,_RadiusMax;
            float _BlurSize; // blurSize,blur size offset

            float4 _RadialInfo;
            float4 _NoiseMapST;

            float _GrayScale;
CBUFFER_END
            float _Aspect; // camera rarget's width/height

            #define NUM_SAMPLES 20
            #define _RadialST _RadialInfo.xy
            #define _RadialLength _RadialInfo.z
            #define _NoiseMapScale _RadialInfo.w

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


                #if defined(RADIAL_TEX_ON)
                half noise = 0;
                #if defined(_NOISE_MAP_ON)
                    noise = SAMPLE_TEXTURE2D(_NoiseMap,sampler_NoiseMap,i.texcoord * _NoiseMapST.xy + _NoiseMapST.zw).x;
                    noise *= _NoiseMapScale;
                #endif
// return noise;
                float2 polarUV = ToPolar((i.texcoord - _Center)*2);
                polarUV *= _RadialST;
                // polarUV = lerp(polarUV,i.texcoord,noise);
                polarUV += noise;

                half4 radialTex = SAMPLE_TEXTURE2D(_RadialTex,sampler_RadialTex,polarUV);

                half4 radialIntensity = saturate(radialTex - 0.5);

                blurCol *= lerp(radialIntensity.x,1,smoothstep(.5,1,dirLen*_RadialLength));
                #endif

                half4 col = lerp(mainTex,blurCol,dist);

                #if defined(_GRAY_SCALE_ON)
                float gray = dot(half3(0.2,0.7,0.07),col.xyz);
                col = pow(gray , _GrayScale);
                // return smoothstep(0.,1,gray);
                #endif

                return saturate(col);
            }
            ENDHLSL
        }

    }
}
