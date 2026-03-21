#include "HashWithoutSine.hlsl"

float random12(float2 v)
{
    //return frac(sin(dot(v, float2(12.9898, 78.233))) * 43758.5453);
    return hash12(v);
}

float2 random22(float2 v)
{
    //return float2(random12(v), random12(v + float2(10.0f, 10.0f)));
    return hash22(v);
}

float interpolation(float f)
{
    return f * f * f * (f * (6.0 * f - 15.0) + 10.0);
}