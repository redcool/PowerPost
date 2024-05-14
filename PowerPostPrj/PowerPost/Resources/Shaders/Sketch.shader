Shader "Hidden/PowerPost/Sketch"
{
    Properties
    {
       
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Stencil
        {
            Ref 2
            Comp NotEqual
            pass keep
        }

        Pass
        {
            Name "Sketch"
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/NormalReconstruction.hlsl"
            #define SKIP_DEPTH
            #include "PowerPostLib.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
            
            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
            TEXTURE2D(_SketchTexture);SAMPLER(sampler_SketchTexture);
            // TEXTURE2D_X_FLOAT(_CameraDepthTexture);
            // SAMPLER(sampler_CameraDepthTexture);

            // float SampleSceneDepth(float2 uv)
            // {
            //     return SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, UnityStereoTransformScreenSpaceTex(uv)).r;
            // }

            int _SketchToggle;
            int _DebugToggle;
            half4 _SketchColor;
            float _SketchBlend;
            float _LightInfluence;
            float _TriPlanerPower;
            float _SketchThreshold;
            float _SketchSmooth;
            float _SketchSize;
            float _SketchUpper;
            float _SketchLower;
            float _SketchRange;
            float _SketchTransitionRange;
            float _OriginBrightnessAdjust;
            float _OriginContrastAdjust;
            float _InveseArea;
            float4 _SketchTexture_TexelSize;

            float ColorGray(half3 color)
            {
                return 0.299*color.r + 0.587*color.g + 0.114*color.b;
            }

            float Linear01Depth(float z)
            {
                const float isOrtho = unity_OrthoParams.w;
                const float isPers = 1.0 - unity_OrthoParams.w;
                z *= _ZBufferParams.x;
                return (1.0 - isOrtho * z) / (isPers * z + _ZBufferParams.y);
            }

            half SketchCalculate(float lightness,float texCol)
            {
                
                
                //lightness = saturate(_SketchRange - lightness);
                //lightness = saturate(lightness - _SketchRange);
                //lightness =  _InveseArea ? saturate(lightness - _SketchRange) : saturate(_SketchRange - lightness);
                lightness =  saturate(_SketchRange - lightness);
                lightness = saturate(lightness *  _OriginBrightnessAdjust);
                lightness = saturate(pow(lightness,_OriginContrastAdjust));
                lightness = _InveseArea ? 1 - lightness : lightness;  
                //return lightness;
                //lightness = max(lightness,_SketchDensity);

                half ramp = saturate(smoothstep(_SketchThreshold - _SketchSmooth * 0.5,_SketchThreshold + _SketchSmooth * 0.5 , lightness));
                //return ramp;
                texCol = 1 -texCol;
                half sketch = smoothstep(texCol - _SketchTransitionRange ,texCol,clamp(ramp,texCol -_SketchTransitionRange + ramp * _SketchLower , _SketchUpper));
                //return lightness;
                return  _DebugToggle ? lightness : sketch;
            }

            float4 frag (VaryingsDefault i) : SV_Target
            {
                float screenRatio = _ScreenParams.y / _ScreenParams.x;
                float4 col = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.texcoord );
                //return col;
                float rawDepth = SAMPLE_TEXTURE2D_LOD(_CameraDepthTexture,sampler_CameraDepthTexture,i.texcoord,0);

                float depthWeight =  1 - saturate(Linear01Depth(rawDepth)* 100);
                //return step(0.5,depthWeight);
                depthWeight = smoothstep(0.2,0.5,depthWeight);
                //return depthWeight;
                float3 posWS =  ComputeWorldSpacePosition(i.texcoord.xy,rawDepth,UNITY_MATRIX_I_VP);
                float3 rightVector = ddx(posWS);
                float3 upVector = ddy(posWS);
                float3 normalWS = normalize(cross(upVector,rightVector));
                float NdotL = saturate(dot(normalWS,normalize( _MainLightPosition.xyz)));
                float adjust = _SketchSize;
                float2 offsetUV = i.texcoord.xy  * float2(1,screenRatio); 

                normalWS = abs(normalWS);
                normalWS = pow(normalWS,_TriPlanerPower);
                float3 weights = normalWS/dot(normalWS,1);

                float cx =  SAMPLE_TEXTURE2D(_SketchTexture,sampler_SketchTexture,posWS.yz * adjust);
                float cy =  SAMPLE_TEXTURE2D(_SketchTexture,sampler_SketchTexture,posWS.xz * adjust);
                float cz =  SAMPLE_TEXTURE2D(_SketchTexture,sampler_SketchTexture,posWS.xy * adjust);


                float sketchTex = dot(float3(cx,cy,cz),weights);
                float lightness =Gray(col.rgb);

                float sketch = SketchCalculate(lightness,sketchTex);
                //return sketch;
                //return sketch;
                half3 sketchColor = lerp(half3(1,1,1),_SketchColor, sketch);
                float lightInfluence = ( _InveseArea ? 0 : (NdotL)) * _LightInfluence;
                float finalMask = saturate(sketch - lightInfluence);
                //return finalMask;
                col.rgb = lerp(col.rgb,
                lerp(col.rgb * sketchColor,lerp(col.rgb,_SketchColor,sketch), _SketchBlend ),
                finalMask * (1-depthWeight));

                return _DebugToggle ? finalMask : half4(col.rgb ,1);
            }
            ENDHLSL
        }
    }
}
