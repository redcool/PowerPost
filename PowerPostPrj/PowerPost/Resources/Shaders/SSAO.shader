Shader "Hidden/PowerPost/SSAO"
{
    Properties
    {
        
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        // ao pass
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment frag
            #include "PowerPostLib.hlsl"

            TEXTURE2D(_MainTex);SAMPLER(sampler_MainTex);

            TEXTURE2D(_DepthTex);SAMPLER(sampler_DepthTex);
            half4 _DepthTex_TexelSize;

            TEXTURE2D(_NoiseMap);SAMPLER(sampler_NoiseMap);
            float4 _SSAOParams;

        #define SCREEN_PARAMS GetScaledScreenParams()
        #define SAMPLE_COUNT _SSAOParams.w
        #define INTENSITY _SSAOParams.x
        #define RADIUS _SSAOParams.y
        #define DOWNSAMPLE _SSAOParams.z

        float3x3 CalcProjection(out float2 ps,out float2 pt){
            float3x3 proj = (float3x3)unity_CameraProjection;
            ps = rcp(float2(proj._11,proj._22));;
            pt = float2(proj._13,proj._23);
            return proj;
        }

        float SampleDepth(float2 uv){
            float d = SAMPLE_TEXTURE2D(_CameraDepthTexture,sampler_CameraDepthTexture,uv);
            return LinearEyeDepth(d,_ZBufferParams);
        }

        float3 ReconstructViewPos(float2 uv,float depth,float2 ps,float2 pt){
            float3 vpos = float3((uv.xy *2 -1-pt) * ps,1) * depth; //[0,1] ->[-d,d]
            return vpos;
        }

        void SampleDepthNormalViewPos(float2 uv,float2 ps,float2 pt,out float depth,out float3 normal,out float3 viewPos){
            depth = SampleDepth(uv);
            viewPos = ReconstructViewPos(uv,depth,ps,pt);
            normal = normalize(cross(ddy(viewPos),ddx(viewPos)));
        }

        float N21(float2 uv){
            // return frac(sin(dot(uv,float2(12.789,78.987))) * 43215.45);
                float f = dot(float2(12.9898, 78.233), uv);
            return frac(43758.5453 * sin(f));
        }
        float2 CosSin(float theta){
            float s,c;
            sincos(theta,s,c);
            return float2(c,s);
        }

        float3 PickSamplePoint(float2 uv,float randAdd,int index){
            float2 posSS = uv * SCREEN_PARAMS.xy * DOWNSAMPLE;
            float gn = InterleavedGradientNoise(posSS,index);
            float u = frac(N21(float2(0,index + randAdd)) + gn) * 2 - 1;
            float theta = (N21(float2(1,index + randAdd)) + gn) * TWO_PI;
            return float3(CosSin(theta) * sqrt(1 - u*u),u);
        }

        float SSAO(float2 uv){
            float2 ps,pt;
            float3x3 camProj = CalcProjection(ps,pt);

            float depth;
            float3 normal,viewPos;
            SampleDepthNormalViewPos(uv,ps,pt,depth,normal,viewPos);

            float randAdd = uv.x * 1e-10;
            float rcpSampleCount = rcp(SAMPLE_COUNT);
            float ao=0;
            for(int s=0;s<int(SAMPLE_COUNT);s++){
                float3 ray = PickSamplePoint(uv,randAdd,s);
                ray *= sqrt( (s+1) * rcpSampleCount) * RADIUS;
                ray = faceforward(ray,-normal,ray);

                float3 sampleViewPos = viewPos + ray;
                float3 sampleProjPos = mul(camProj,sampleViewPos);

                float2 sampleUV = clamp((sampleProjPos.xy * rcp(sampleViewPos.z) + 1) * 0.5,0,1);
                float d = SampleDepth(sampleUV);

                float3 vpos = ReconstructViewPos(sampleUV,d,ps,pt);
                float3 dir = vpos - viewPos;

                float a1 = max(dot(dir,normal) - 0.01 * depth,0);
                float a2 = dot(dir,dir) * 0.0001;
                // return a2;
                ao += a1 / a2;
            }
            ao *= RADIUS * INTENSITY * rcpSampleCount;

            return ao;
        }
            float4 frag(VaryingsDefault input) : SV_Target
            {
                float ao = SSAO(input.texcoord);
                return ao;
                
            }
            ENDHLSL
        }
    }
}
