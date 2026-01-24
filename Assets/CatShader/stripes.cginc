half4 stripe(Varyings IN, half4 BaseColor)
{
    float3 dir = normalize(_StripeDirection.xyz);
    
    float pos = dot(IN.positionOS, dir);
    
    float3 waveAxis = cross(dir, float3(0, 1, 0));

    if(length(waveAxis) < 0.01) waveAxis = float3(1, 0, 0); 
    
    float sidePos = dot(IN.positionOS, normalize(waveAxis));

    float wave = sin(sidePos * _WaveFrequency) * _WaveStrength;
    
    float fraction = frac((pos + _StripeOffset + wave) * (_StripeCount / _StripeSpacing));
    
    float dist = abs(fraction - 0.5);
    float mask = smoothstep(_StripeWidth + _StripeSmooth, _StripeWidth - _StripeSmooth, dist);
    
    return lerp(BaseColor, _StripeColor, mask);
}