// Example spots.cginc content
half4 spots(float3 osPos, float3 osNormal, TEXTURE2D_PARAM(noiseTex, noiseSampler)) {
    float3 blending = abs(osNormal);
    blending /= (blending.x + blending.y + blending.z);

    float3 coords = osPos * _SpotCount;
    
    // Using SAMPLE_TEXTURE2D instead of tex2D
    float nX = SAMPLE_TEXTURE2D(noiseTex, noiseSampler, coords.yz).r;
    float nY = SAMPLE_TEXTURE2D(noiseTex, noiseSampler, coords.xz).r;
    float nZ = SAMPLE_TEXTURE2D(noiseTex, noiseSampler, coords.xy).r;

    float noise = nX * blending.x + nY * blending.y + nZ * blending.z;
    float mask = smoothstep(1.0 - _SpotSize, 1.0 - _SpotSize + _SpotThreshold, noise);

    return half4(_SpotColor.rgb, mask);
}