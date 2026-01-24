Shader "Custom/ShellStripesAndSpots"
{
    Properties
    {
        [Header(Base Appearance)]
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _SpotColor ("Spot Color", Color) = (.8, .5, .3, 1)

        [Header(Shell Settings)]
        _ShellLength("Total Fur Length", Float) = 0.1
        _FurDensity("Fur Density", Float) = 100
        _FurThickness("Fur Thickness", Range(0, 1)) = 0.5
        _TaperExponent("Taper Curve", Range(0.1, 5)) = 1.0
        
        [Header(Stripe Settings)]
        _StripeColor("Stripe Color", Color) = (0, 0, 0, 1)
        _StripeWidth("Edge Width", Range(0.0, 1)) = 0.1
        _StripeSmooth("Smoothness", Range(0.0, 1)) = 0.1
        _StripeOffset("Offset", Range(0.0, 1)) = 0.1
        _StripeCount("Count", Float) = 1.0
        _StripeDirection("Stripe Direction", Vector) = (0, 1, 0, 0)
        _StripeSpacing("Stripe Spacing", Float) = 1.0
        _WaveFrequency("_WaveFrequency", Float) = 0.0
        _WaveStrength("_WaveStrength", Float) = 0.0
        
        [Header(Spot Settings)]
        _SpotCount ("Spot Density", Float) = 5
        _SpotSize ("Spot Size", Range(0, 1)) = .1
        _SpotRandomness ("Spot Randomness", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            Name "ShellPass"
            AlphaToMask On
            Cull Off 

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionOS : TEXCOORD1;
                float heightNormalized : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _StripeColor; 
                half4 _SpotColor;
                float3 _StripeDirection;
                float _StripeSpacing;
                float _StripeWidth;
                float _StripeSmooth;
                float _StripeOffset;
                float _StripeCount;
                float _WaveStrength;
                float _WaveFrequency;
                float _ShellLength;
                float _FurDensity;
                float _FurThickness;
                float _TaperExponent;
                float _SpotCount;
                float _SpotSize;
                float _SpotRandomness;
                float _CurrentShellHeight; 
            CBUFFER_END

            float hash3D(float3 p) {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }

            Varyings vert(Attributes IN) {
                Varyings OUT;
                float3 extrudedPos = IN.positionOS.xyz + (IN.normalOS * _CurrentShellHeight * _ShellLength);
                
                OUT.positionHCS = TransformObjectToHClip(extrudedPos);
                OUT.uv = IN.uv;
                OUT.positionOS = IN.positionOS.xyz;
                OUT.heightNormalized = _CurrentShellHeight;
                return OUT;
            }
            
            #include "stripes.cginc"

            half4 frag(Varyings IN) : SV_Target {

                float2 furUV = IN.uv * _FurDensity;
                float2 localUV = frac(furUV) * 2.0 - 1.0;
                float distToCenter = length(localUV);
                

                float threshold = _FurThickness * (1.0 - pow(IN.heightNormalized, _TaperExponent));
                

                if (distToCenter > threshold) discard;

                half3 color = stripe(IN, _BaseColor);


                float3 p = IN.positionOS * _SpotCount;
                float3 i = floor(p);
                float3 f = frac(p);
                float minDist = 1.0;
                for (int z = -1; z <= 1; z++) {
                    for (int y = -1; y <= 1; y++) {
                        for (int x = -1; x <= 1; x++) {
                            float3 neighbor = float3(x, y, z);
                            float3 randCenter = neighbor + hash3D(i + neighbor) * _SpotRandomness;
                            minDist = min(minDist, distance(f, randCenter));
                        }
                    }
                }
                float spotMask = smoothstep(_SpotSize, _SpotSize - 0.01, minDist);
                color = lerp(color, _SpotColor.rgb, spotMask);

                float ao = lerp(0.3, 1.0, IN.heightNormalized);
                
                return half4(color * ao, 1.0);
            }
            ENDHLSL
        }
    }
}