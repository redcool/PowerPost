Shader "Hidden/PowerPost/SunShaft"
{
    Properties
    {
        
    }
    HLSLINCLUDE
        #include "PowerPostLib.hlsl"
        #define SAMPLES_INT 6
        #define SAMPLES_FLOAT 6.0

        TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);
        TEXTURE2D(_BlurRT);SAMPLER(sampler_BlurRT);
		half4 _MainTex_TexelSize;

        TEXTURE2D(_CameraDepthTexture);SAMPLER(sampler_CameraDepthTexture);
        half4 _CameraDepthTexture_TexelSize;



        half4 _SunPos;
        half4 _SunThreshold;
        half4 _SunColor;
        half4 _BlurRadius4;
        


        struct VaryingsRadialBlur{
            half4 vertex:SV_POSITION;
            half2 texcoord:TEXCOORD;
            half2 blurVector:TEXCOORD1;
        };

        VaryingsRadialBlur vertRadial(AttributesDefault input){
            VaryingsRadialBlur output;
            output.vertex = float4(input.vertex.xy,0,1);
            output.texcoord = input.texcoord;//(input.vertex.xy + 1) * 0.5;
            output.blurVector = (_SunPos.xy - input.texcoord.xy) * _BlurRadius4.xy;
            return output;
        }

        half4 fragRadial(VaryingsRadialBlur i):SV_Target{
            half4 color = 0;

            for(int x =0;x< SAMPLES_INT;x++){
                color += SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);
                i.texcoord.xy += i.blurVector;
            }
            return color / SAMPLES_FLOAT;

            // color = SampleDirBlur(_MainTex,sampler_MainTex,i.texcoord,i.blurVector*50);
            
            return color;
        }

        half TransformColor(half4 c){
            return dot(saturate(c.rgb - _SunThreshold.rgb),1);
        }


        half4 frag (VaryingsDefault i) : SV_Target
        {
            half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex,i.texcoord);
            half4 col2 = SAMPLE_TEXTURE2D(_BlurRT,sampler_BlurRT,i.texcoord);
            // return col2;
            col2 = saturate(col2 * _SunColor);
            // col = col + col2 - (col*col2);
            col = col + col2;
            return col;
        }

        half4 fragDepth(VaryingsDefault i) : SV_Target{
            half4 tex = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex,i.texcoord);
            half2 dir = _SunPos.xy - i.texcoord;
            half dist = saturate(_SunPos.w - length(dir));

            half depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,sampler_CameraDepthTexture,i.texcoord);
            depth = Linear01Depth(depth,_ZBufferParams);

            half4 col = 0;
            if(depth > 0.99){
                col = TransformColor(tex) * dist;
            }
            return col ;
        }
    ENDHLSL    
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        // 0 blend 
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            ENDHLSL
        }
        //1 radial blur
        Pass{
            HLSLPROGRAM
            #pragma vertex vertRadial
            #pragma fragment fragRadial
            ENDHLSL
        }
        // 2 depth
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment fragDepth
            ENDHLSL
        }
    }
}
