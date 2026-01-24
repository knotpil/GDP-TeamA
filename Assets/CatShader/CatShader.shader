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
            
            #include "stripes.cginc"

            half4 frag(Varyings IN) : SV_Target
            {
                
                

                return stripe(IN, _BaseColor);
            }
            ENDHLSL
        }
    }
}