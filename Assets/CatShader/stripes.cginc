half4 stripe(Varyings IN, half4 BaseColor) : SV_Target
{
                
    float localX = IN.positionOS.x + 0.5;
                
    float mask = 0.0;

    for (int i = 0; i < (int)_Count; ++i)
    {
        // max of 1 so we don't divide by 0
        float bandPos = _Offset + (float(i) / max(1.0, _Count));
                    
        // distance from the center of the band
        float dist = abs(localX - bandPos);
                    
        // smooth the bands if wanted
        float stripe = smoothstep(_Width + _Smooth, _Width - _Smooth, dist);
                    
        // accumulate the bigger value
        mask = max(mask, stripe);
    }

    return lerp(BaseColor, _StripeColor, mask);
}