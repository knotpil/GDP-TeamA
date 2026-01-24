Shader "Custom/stripes"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [Color] _StripeColor("Stripe Color", Color) = (0, 0, 0, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [PowerSlider(3.0)] _Width("Edge Width", Range(0.0, 1)) = 0.1
        [PowerSlider(3.0)] _Smooth("Smoothness", Range(0.0, 1)) = 0.1
        [PowerSlider(3.0)] _Offset("Offset", Range(0.0, 1)) = 0.1
        [Float] _Count("Count", Float) = 1.0
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
                float4 positionOS : POSITION; // Object Space position
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionOS : TEXCOORD1; // Added to pass local position
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _StripeColor;
                float4 _BaseMap_ST;
                float _Width;
                float _Smooth;
                float _Offset;
                float _Count;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                
                // Store the raw local position to pass to the fragment shader
                OUT.positionOS = IN.positionOS.xyz; 
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
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

                return lerp(_BaseColor, _StripeColor, mask);
            }
            ENDHLSL
        }
    }
}