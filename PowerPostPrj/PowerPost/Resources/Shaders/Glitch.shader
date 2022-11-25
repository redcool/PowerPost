Shader "Hidden/PowerPost/Glitch"
{
    Properties
    {
        _StencilRef("_StencilRef",int) = 6
    }

    HLSLINCLUDE
        #include "PowerPostLib.hlsl"
        TEXTURE2D(_CameraOpaqueTexture);SAMPLER(sampler_CameraOpaqueTexture);
        float4 _CameraOpaqueTexture_TexelSize;
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        stencil{
            ref [_StencilRef]
            comp equal
            pass keep
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag

            float4 _ScanlineJitter;//(z : block size,w : intensity)

            float2 _VerticalJump;
            float2 _SnowFlake; // frequency,amplitude
            float _HorizontalShake;
            float2 _ColorDrift;

            // variable names
            #define _JitterIntensity _ScanlineJitter.x
            #define _JitterThreshold _ScanlineJitter.y
            #define _JitterBlockSize _ScanlineJitter.z

            #define _Intensity _ScanlineJitter.w

            #define _SnowFlakePeriod _SnowFlake.x
            #define _SnowFlakeIntensity _SnowFlake.y

            #define _VerticalJumpIntensity _VerticalJump.x
            #define _VerticalJumpTime _VerticalJump.y

            #define _ColorDriftSpeed _ColorDrift.y
            #define _ColorDriftIntensity _ColorDrift.x

            float4 frag(VaryingsDefault i) : SV_Target
            {
                float u = i.texcoord.x;
                float v = i.texcoord.y;

                float2 snowFlake = (N21(i.texcoord * _SnowFlakePeriod)) * _SnowFlakeIntensity;
                float jitter = N21(float2(v * _JitterBlockSize,_Time.x)) *2-1;
                jitter *= step(_JitterThreshold,abs(jitter)) * _JitterIntensity;
// return jitter;

                float jump = lerp(v, frac(v + _VerticalJumpTime), _VerticalJumpIntensity);

                float hshake = N21(float2(_Time.x,2)-0.5) * _HorizontalShake;
                float drift = sin(jump + _ColorDriftSpeed) * _ColorDriftIntensity;

                float u1 = (jitter + snowFlake.x + hshake) * _Intensity;
                float u2 = (jitter + snowFlake.x + hshake + drift ) * _Intensity;
                float4 c1 = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,sampler_CameraOpaqueTexture,frac(float2(u + u1,jump)));
                float4 c2 = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,sampler_CameraOpaqueTexture,frac(float2(u + u2 ,jump)));
                return float4(c1.r,c2.g,c1.b,1);
            }

            float4 frag1(VaryingsDefault i):sv_target{
                float u = i.texcoord.x;
                float v = i.texcoord.y;
                float2 snowFlake = N21(float2(u+_Time.x*0.001,v));

                float jitter = N21(float2(_Time.x,v));
                float jump = lerp(v,frac(v+ _Time.y),.1);
// return jump;
                float hu = (frac(u*100 + _Time.y)<0.1);
                
                float hshake = N21(float2(_Time.x*hu,1));
                float drift = sin(jump + _Time.y*100);
                
                float u1 = jitter + jump + hshake;
                float u2 = u1 + drift;
                
                float4 c1 = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,sampler_CameraOpaqueTexture,float2(u+u1*0.01,jump));
                float4 c2 = SAMPLE_TEXTURE2D(_CameraOpaqueTexture,sampler_CameraOpaqueTexture,float2(u+u2*0.01,jump));
                return float4(c2.x,c1.y,c2.b,1);
            }
            ENDHLSL
        }

    }
}
