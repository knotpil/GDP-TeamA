half4 stripe(Varyings IN, half4 BaseColor)
{
    // set the xyz of the stripes
    float3 dir = normalize(_StripeDirection.xyz);
    
    // how far in a direction the shader has gone
    float pos = dot(IN.positionOS, dir);
    
    // direction perpendicular to the stripe direction
    float3 waveAxis = cross(dir, float3(0, 1, 0));
    if(length(waveAxis) < 0.01) waveAxis = float3(1, 0, 0);
    float sidePos = dot(IN.positionOS, normalize(waveAxis));
    
    // how wavy the wave is
    float wave = sin(sidePos * _WaveFrequency) * _WaveStrength;
    
    // repeat across the objects surface
    float fraction = frac((pos + _StripeOffset + wave) * (_StripeCount / _StripeSpacing));
    
    // center the stripe 
    float dist = abs(fraction - 0.5);
    
    // set the gradient of the strip edges 
    float mask = smoothstep(_StripeWidth + _StripeSmooth, _StripeWidth - _StripeSmooth, dist);
    
    // return the pixel color
    return lerp(BaseColor, _StripeColor, mask);
}