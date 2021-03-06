#if !defined(DIFFUSE_PROFILE_CGINC)
#define DIFFUSE_PROFILE_CGINC
    #define KERNEL_SIZE 25
    #define DistanceToProjectionWindow 5.671281819617709             //1.0 / tan(0.5 * radians(20));
    #define DPTimes300 1701.384545885313                             //DistanceToProjectionWindow * 300
    float4 _Kernel[KERNEL_SIZE];
    const static float4 _LocalKernel[KERNEL_SIZE] = {
        {1,0.4354057,1,0},{0,0.001171291,0,-3},{0,0.004015044,0,-2.520833},{0,0.00601844,0,-2.083333},{0,0.008431426,0,-1.6875},{0,0.01135324,0,-1.333333},{0,0.0154557,0,-1.020833},{0,0.02155918,0,-0.75},{0,0.03171116,0,-0.5208333},{0,0.04933598,0,-0.3333333},{0,0.05936943,0,-0.1875},{0,0.04844738,0,-0.08333334},{7.519899E-16,0.02542887,7.519899E-16,-0.02083333},{7.519899E-16,0.02542887,7.519899E-16,0.02083333},{0,0.04844738,0,0.08333334},{0,0.05936943,0,0.1875},{0,0.04933598,0,0.3333333},{0,0.03171116,0,0.5208333},{0,0.02155918,0,0.75},{0,0.0154557,0,1.020833},{0,0.01135324,0,1.333333},{0,0.008431426,0,1.6875},{0,0.00601844,0,2.083333},{0,0.004015044,0,2.520833},{0,0.001171291,0,3},
    };

    float4 CalcKernel(int id){
        return _Kernel[id];
        return saturate(_Kernel[id] + _LocalKernel[id]);
    }

    float3 DiffuseProfile(float4 mainColor,sampler2D tex,float2 uv,float2 offset,float sssMask){
        float BlurLength = DistanceToProjectionWindow;
        float2 UVOffset = BlurLength*offset;

        float3 blurColor = mainColor * CalcKernel(0);
        for(int i=1;i<KERNEL_SIZE;i++){
            float4 kernel = CalcKernel(i);
            float2 sssuv = uv + kernel.w * UVOffset;
            float3 sssColor = tex2D(tex,sssuv);
            sssColor = lerp(mainColor,sssColor,sssMask);

            blurColor += kernel.xyz * sssColor;
        }
        return blurColor;
    }

#endif //DIFFUSE_PROFILE_CGINC