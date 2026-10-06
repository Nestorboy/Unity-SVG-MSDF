#if !defined(MSDF_INCLUDED)
#define MSDF_INCLUDED

// Based on Chlumsky msdfgen example. See: https://github.com/Chlumsky/msdfgen

float Median(float r, float g, float b)
{
    return max(min(r, g), min(max(r, g), b));
}

float2 Sqr(float2 x)
{
    return x * x;
}

float2 UnitRange(float pixelRange, float2 texelSize)
{
    return pixelRange * texelSize;
}

float ScreenPxRange(float2 uv, float2 unitRange)
{
    float2 screenTexSize = rsqrt(Sqr(ddx(uv)) + Sqr(ddy(uv)));
    return max(0.5 * dot(unitRange, screenTexSize), 1.0);
}

float SampleMSDF(sampler2D msdfTex, float2 uv, float2 unitRange)
{
    float3 msd = tex2Dlod(msdfTex, float4(uv, 0.0, 0.0)).rgb;
    float sd = Median(msd.r, msd.g, msd.b);
    float screenPxDistance = ScreenPxRange(uv, unitRange) * (sd - 0.5);
    return saturate(screenPxDistance + 0.5);
}

#endif