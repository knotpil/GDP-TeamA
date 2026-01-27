Shader "Custom/CatMat"
{
    Properties
    {
        [Header(Base Appearance)]
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        _SpotColor ("Spot Color", Color) = (.8, .5, .3, 1)

        [Header(Shell Settings)]
        _ShellLength("Total Fur Length", Float) = 0.1
        _FurDensity("Fur Density", Float) = 100
        _FurThickness("Fur Thickness", Range(0, 1)) = 1.0
        _GravityStrength("Fur Gravity", Float) = .2
        _TaperExponent("Taper Curve", Range(0.1, 5)) = 1.0
        
        [Header(Noise Settings)]
        [NoScaleOffset] _NoiseMap("Fur Noise Map", 2D) = "white" {}
        _NoiseScale("Noise Scale", Float) = 1.0
        
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
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

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
            
            TEXTURE2D(_NoiseMap);
            SAMPLER(sampler_NoiseMap);

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
                float _NoiseScale;
                float _GravityStrength;
            CBUFFER_END

            // makes the spots "random"
            float hash3D(float3 p) {
                p = frac(p * 0.3183099 + 0.1);
                p *= 17.0;
                return frac(p.x * p.y * p.z * (p.x + p.y + p.z));
            }
            
            // split out function for readability 
            #include "stripes.cginc"

           Varyings vert(Attributes IN) {
                Varyings OUT;
                
                float3 extrusion = IN.normalOS * _CurrentShellHeight * _ShellLength;
                
                float3 gravityDir = float3(0, -1, 0); 
                
                // change gravity from world to object space so it feels like global gravity
                float3 gravityObjSpace = mul((float3x3)GetWorldToObjectMatrix(), gravityDir);
                
                // displacement by height
                float gravityEffect = pow(_CurrentShellHeight, 2.0) * _GravityStrength;
                float3 gravityOffset = gravityObjSpace * gravityEffect;

                float3 finalPosOS = IN.positionOS.xyz + extrusion + (gravityOffset / 1.8);

                OUT.positionHCS = TransformObjectToHClip(finalPosOS);
                OUT.uv = IN.uv;
                OUT.positionOS = IN.positionOS.xyz;
                OUT.heightNormalized = _CurrentShellHeight;
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                
                // shell texturing bits
                //
                // sample a noise texture for shell properties 
                float2 noiseUV = IN.uv * _FurDensity * _NoiseScale;
                float furNoise = SAMPLE_TEXTURE2D(_NoiseMap, sampler_NoiseMap, noiseUV).r;
                
                // as height increases a higher noise value to keep the pixel
                // if its out of the threshold the rest of the shader is skipped
                float threshold = pow(IN.heightNormalized, _TaperExponent);
                
                // if the pixel is out of range from mesh don't bother with it
                // skips the following calculations 
                // this also tapers it based on height
                if (furNoise < threshold + (1.0 - _FurThickness)) discard;

                
                // call the stripe function from stripes.cginc
                half3 color = stripe(IN, _BaseColor);


                // spot calculation happens here
                //
                // objects position is multiplied by spot count to create a grid
                float3 p = IN.positionOS * _SpotCount;
                // what part of the cell grid is the pixel in
                float3 i = floor(p);
                // where in the cell is the pixel 
                float3 f = frac(p);
                float minDist = 1.0;
                for (int z = -1; z <= 1; z++) {
                    for (int y = -1; y <= 1; y++) {
                        for (int x = -1; x <= 1; x++) {
                            float3 neighbor = float3(x, y, z);
                            // create a spot in the next cell with a random position
                            float3 randCenter = neighbor + hash3D(i + neighbor) * _SpotRandomness;
                            // smallest distance away from the spot is kept
                            minDist = min(minDist, distance(f, randCenter));
                        }
                    }
                }
                float spotMask = smoothstep(_SpotSize, _SpotSize - 0.01, minDist);
                // apply the spots to the stripped color
                color = lerp(color, _SpotColor.rgb, spotMask);

                // darken the shell texture based on height
                float ao = lerp(0.2, 1.0, IN.heightNormalized);
                
                // the color itself
                return half4(color * ao, 1.0);
            }
            ENDHLSL
        }
    }
}