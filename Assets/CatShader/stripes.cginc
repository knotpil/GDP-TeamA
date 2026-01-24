half4 stripe(Varyings IN, half4 BaseColor)
{

    float3 dir = normalize(_StripeDirection.xyz);
    float pos = dot(IN.positionOS, dir);
    
    float fraction = frac((pos + _StripeOffset) * (_StripeCount / _StripeSpacing));
    
    float dist = abs(fraction - 0.5);
    
    float mask = smoothstep(_StripeWidth + _StripeSmooth, _StripeWidth - _StripeSmooth, dist);
    
    return lerp(BaseColor, _StripeColor, mask);
}