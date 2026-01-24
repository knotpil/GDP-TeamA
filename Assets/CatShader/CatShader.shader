Shader "Custom/ProceduralStripesAndSpots"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _StripeColor("Stripe Color", Color) = (0, 0, 0, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _StripeWidth("Edge Width", Range(0.0, 1)) = 0.1
        _StripeSmooth("Smoothness", Range(0.0, 1)) = 0.1
        _StripeOffset("Offset", Range(0.0, 1)) = 0.1
        _StripeCount("Count", Float) = 1.0
        _StripeDirection("Stripe Direction", Vector) = (0, 1, 0, 0)
        _StripeSpacing("Stripe Spacing", Float) = 1.0
        
        _SpotColor ("Spot Color", Color) = (.8, .5, .3, 1)
        _SpotCount ("Spot Density", Float) = 5
        _SpotSize ("Spot Size", Range(0, 1)) = .1
        _SpotThreshold ("Spot Sharpness", Range(0, 0.1)) = 0.01
        _SpotRandomness ("Spot Randomness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _StripeColor;
                float4 _BaseMap_ST;
                float3 _StripeDirection;
                float _StripeSpacing;
                float _StripeWidth;
                float _StripeSmooth;
                float _StripeOffset;
                float _StripeCount;
            
                half4 _SpotColor;
                float _SpotCount;
                float _SpotSize;
                float _SpotThreshold;
                float _SpotRandomness;
            CBUFFER_END

            // A fast, deterministic 3D Hash function
            float hash3D(float3 p)
            {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.positionOS = IN.positionOS.xyz; 
                return OUT;
            }
            
            #include "stripes.cginc"

            half4 frag(Varyings IN) : SV_Target
            {
                // 1. Calculate Stripes
                half4 color = stripe(IN, _BaseColor);

                // 2. Calculate Procedural Spots (Grid-based jitter)
                float3 p = IN.positionOS * _SpotCount;
                float3 i = floor(p); // Cell ID
                float3 f = frac(p);  // Position inside cell
                
                float minDist = 1.0;

                // Check 3x3x3 neighborhood for the nearest "dot center"
                for (int z = -1; z <= 1; z++) {
                    for (int y = -1; y <= 1; y++) {
                        for (int x = -1; x <= 1; x++) {
                            float3 neighbor = float3(x, y, z);
                            // Generate a random center for the spot in this cell
                            float3 randCenter = neighbor + hash3D(i + neighbor) * _SpotRandomness;
                            float d = distance(f, randCenter);
                            minDist = min(minDist, d);
                        }
                    }
                }

                // 3. Create the mask
                float spotMask = smoothstep(_SpotSize, _SpotSize - _SpotThreshold, minDist);

                // 4. Blend spots on top of stripes
                color.rgb = lerp(color.rgb, _SpotColor.rgb, spotMask);

                return color;
            }
            ENDHLSL
        }
    }
}