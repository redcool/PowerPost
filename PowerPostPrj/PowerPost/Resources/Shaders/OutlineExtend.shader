Shader "Hidden/PowerPost/ExtendOutline"
{
    Properties
    {

        _EdgeColor ("Outline Color", Color) = (1, 1, 1, 1)
        _Thickness ("Thickness", Range(0, 5)) = 1

        [Space(15)]
        [Toggle(OUTLINE_USE_DEPTH)]_UseDepth ("Use Depth", Float) = 1
        _DepthThresholdMin ("Min Depth Threshold", Range(0, 1)) = 0
        _DepthThresholdMax ("Max Depth Threshold", Range(0, 1)) = 0.25

        [Space(15)]
        [Toggle(OUTLINE_USE_NORMALS)]_UseNormals ("Use Normals", Float) = 0
        _NormalThresholdMin ("Min Normal Threshold", Range(0, 1)) = 0.5
        _NormalThresholdMax ("Max Normal Threshold", Range(0, 1)) = 1.0

        [Space(15)]
        [Toggle(OUTLINE_USE_COLOR)]_UseColor ("Use Color", Float) = 0
        _ColorThresholdMin ("Min Color Threshold", Range(0, 1)) = 0
        _ColorThresholdMax ("Max Color Threshold", Range(0, 1)) = 0.25

        [Space(15)]
        [Toggle(OUTLINE_ONLY)]_OutlineOnly ("Outline Only", Float) = 0
    }

    SubShader
    {
        //Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}
        LOD 100
        ZTest Always
        ZWrite Off Cull Off

        Stencil
        {
            Ref 2
            Comp NotEqual
            pass keep
        }

        Pass
        {
            Name "Outline"

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/NormalReconstruction.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            #define SKIP_DEPTH
            #include "PowerPostLib.hlsl"

            #pragma vertex VertDefault
            #pragma fragment frag
            #pragma shader_feature OUTLINE_USE_DEPTH
            #pragma shader_feature OUTLINE_USE_NORMALS
            #pragma shader_feature OUTLINE_USE_COLOR
            #pragma shader_feature OUTLINE_ONLY
            #pragma shader_feature RESOLUTION_INVARIANT_THICKNESS

            #pragma multi_compile _ _USE_DRAW_PROCEDURAL

            float _Thickness;
            float _DepthEdgeWidth;
            float _NormalEdgeWidth;
            float _ColorEdgeWidth;
            half4 _EdgeColor;
            float _DepthThresholdMin, _DepthThresholdMax;
            float _NormalThresholdMin, _NormalThresholdMax;
            float _ColorThresholdMin, _ColorThresholdMax;
            float _DistInfluence;
            float _DistInfluenceSmooth;

            TEXTURE2D_X(_CameraColorTexture);
            SAMPLER(sampler_CameraColorTexture);
            TEXTURE2D(_MainTex); SAMPLER(sampler_MainTex);

            //SAMPLER(sampler_LinearClamp);//work
            
            // #define OUTLINE_USE_TRANSPARENT_DEPTH
            /*#ifdef OUTLINE_USE_TRANSPARENT_DEPTH
            TEXTURE2D_X(_CameraTransparentDepthTexture);
            #endif*/

            //float4 _SourceSize;
            float4 _MainTex_TexelSize;

            // Z buffer depth to linear 0-1 depth
            // Handles orthographic projection correctly
            float Linear01Depth(float z)
            {
                const float isOrtho = unity_OrthoParams.w;
                const float isPers = 1.0 - unity_OrthoParams.w;
                z *= _ZBufferParams.x;
                return (1.0 - isOrtho * z) / (isPers * z + _ZBufferParams.y);
            }

            float SampleDepth(float2 uv)
            {

                //float d = SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_LinearClamp, UnityStereoTransformScreenSpaceTex(uv)).r;
                float d = SAMPLE_TEXTURE2D_LOD(_CameraDepthTexture,sampler_CameraDepthTexture,uv,0);
                //#ifdef OUTLINE_USE_TRANSPARENT_DEPTH
                //d += SAMPLE_TEXTURE2D_X(_CameraTransparentDepthTexture, sampler_LinearClamp, UnityStereoTransformScreenSpaceTex(uv)).r;
                //#endif
                return Linear01Depth(d);
            }

            float3 SampleNormals(float2 uv)
            {
                float3 normal = SAMPLE_TEXTURE2D_X(_CameraNormalsTexture, sampler_CameraNormalsTexture, UnityStereoTransformScreenSpaceTex(uv)).xyz;

                #if defined(_GBUFFER_NORMALS_OCT)
                float2 remappedOctNormalWS = Unpack888ToFloat2(normal); // values between [ 0,  1]
                float2 octNormalWS = remappedOctNormalWS.xy * 2.0 - 1.0;    // values between [-1, +1]
                normal = UnpackNormalOctQuadEncode(octNormalWS);
                #endif

                return normal;
            }

            float4 SampleCameraColor(float2 uv)
            {
                return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, UnityStereoTransformScreenSpaceTex(uv));
                //return SAMPLE_TEXTURE2D_X(_CameraColorTexture, sampler_CameraColorTexture, UnityStereoTransformScreenSpaceTex(uv));
            }

            float4 Outline(float2 uv)
            {
                float4 original = SampleCameraColor(uv);

                const float offset_positive = +ceil(_Thickness * 0.5f);
                const float offset_negative = -floor(_Thickness * 0.5f);

                // #if RESOLUTION_INVARIANT_THICKNESS
                // const float screen_ratio = _MainTex_TexelSize.z / _MainTex_TexelSize.w;
                // const float2 texel_size = 1.0 / 800.0 * float2(1.0, screen_ratio);
                // #else
                const float2 texel_size = _MainTex_TexelSize.xy;
                //#endif

                float left = texel_size.x * offset_negative;
                float right = texel_size.x * offset_positive;
                float top = texel_size.y * offset_negative;
                float bottom = texel_size.y * offset_positive;

                const float2 uv0 = uv + float2(left, top);
                const float2 uv1 = uv + float2(right, bottom);
                const float2 uv2 = uv + float2(right, top);
                const float2 uv3 = uv + float2(left, bottom);

                const float d0 = SampleDepth(uv0);
                float distWeight = smoothstep(-(_DistInfluenceSmooth),_DistInfluence,d0);
                //return distWeight;

                #ifdef OUTLINE_USE_DEPTH
                const float d1 = SampleDepth(uv1);
                const float d2 = SampleDepth(uv2);
                const float d3 = SampleDepth(uv3);
                const float depth_threshold_scale = 300.0f;
                //float d = min(length(float2(d1 - d0, d3 - d2)) * depth_threshold_scale + _DepthEdgeWidth * distWeight,0);
                float d = clamp(0,1 - distWeight,saturate(length(float2(d1 - d0, d3 - d2)) *  _DepthEdgeWidth)) ;
                //return clamp(0,1 - distWeight,saturate(length(float2(d1 - d0, d3 - d2)) *  _DepthEdgeWidth)) ;
                d = smoothstep(_DepthThresholdMin, _DepthThresholdMax, d);
                //return d0*300;
                #else
                float d = 0.0f;
                #endif  // OUTLINE_USE_DEPTH
               
                #ifdef OUTLINE_USE_NORMALS
                const float3 n0 = SampleNormals(uv0);
                const float3 n1 = SampleNormals(uv1);
                const float3 n2 = SampleNormals(uv2);
                const float3 n3 = SampleNormals(uv3);

                const float3 nd1 = n1 - n0;
                const float3 nd2 = n3 - n2;
                //float n = sqrt(dot(nd1, nd1) + dot(nd2, nd2)) * _NormalEdgeWidth;
                float n = clamp(0,1 - distWeight,saturate(sqrt(dot(nd1, nd1) + dot(nd2, nd2)) * _NormalEdgeWidth));
                n = smoothstep(_NormalThresholdMin, _NormalThresholdMax , n);
                //return n;
                #else
                float n = 0.0f;
                #endif  // OUTLINE_USE_NORMALS

                //return float4(SampleNormals(uv0),1);

                #ifdef OUTLINE_USE_COLOR
                const float3 c0 = SampleCameraColor(uv0).rgb;
                const float3 c1 = SampleCameraColor(uv1).rgb;
                const float3 c2 = SampleCameraColor(uv2).rgb;
                const float3 c3 = SampleCameraColor(uv3).rgb;

                const float3 cd1 = c1 - c0;
                const float3 cd2 = c3 - c2;
                float c = clamp(0,1 - distWeight,saturate(sqrt(dot(cd1, cd1) + dot(cd2, cd2)) * _ColorEdgeWidth));
                //return 1;
                c = smoothstep(_ColorThresholdMin, _ColorThresholdMax, c);
                //return c;
                #else
                float c = 0;
                #endif  // OUTLINE_USE_COLOR

                const float g = max(d, max(n, c)) ;


                #ifdef OUTLINE_ONLY
                //original.rgb = lerp(1.0 - _EdgeColor.rgb, _EdgeColor.rgb, g * _EdgeColor.a);
                original.rgb = (1 - (1 - original.rgb) * (g * (1 - _EdgeColor.rgb)));
                #endif  // OUTLINE_ONLY

                float4 output;
                //output.rgb = lerp(original.rgb, _EdgeColor.rgb, g * _EdgeColor.a);
                //output.a = original.a;
                output.rgb = lerp(original.rgb,(1-(1 - saturate(original.rgb)) * (  g*(1-_EdgeColor.rgb)))* original.rgb,_EdgeColor.a);
                output.a = 1;
                return  output;
            }

            
            half4 frag(VaryingsDefault input) : SV_Target
            {
                //return half4(SampleCameraColor(input.texcoord).rgb,1);
                float4 c = Outline(input.texcoord);
                return half4(c.rgb,1);
            }

            ENDHLSL
        }
    }
    // FallBack "Diffuse"
}
