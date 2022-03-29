#if !defined(COLORS_HLSL)
#define COLORS_HLSL

// Converts linear RGB to LMS
// Full float precision to avoid precision artefact when using ACES tonemapping
float3 LinearToLMS(float3 x)
{
    const real3x3 LIN_2_LMS_MAT = {
        3.90405e-1, 5.49941e-1, 8.92632e-3,
        7.08416e-2, 9.63172e-1, 1.35775e-3,
        2.31082e-2, 1.28021e-1, 9.36245e-1
    };

    return mul(LIN_2_LMS_MAT, x);
}

// Full float precision to avoid precision artefact when using ACES tonemapping
float3 LMSToLinear(float3 x)
{
    const real3x3 LMS_2_LIN_MAT = {
        2.85847e+0, -1.62879e+0, -2.48910e-2,
        -2.10182e-1,  1.15820e+0,  3.24281e-4,
        -4.18120e-2, -1.18169e-1,  1.06867e+0
    };

    return mul(LMS_2_LIN_MAT, x);
}

#endif //COLORS_HLSL