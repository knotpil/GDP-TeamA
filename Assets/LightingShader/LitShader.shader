Shader "Custom/LitShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }
        
        Pass
        {
            Name "ForwardPass"
            Tags {"LightMode" = "UniversalForward"}
            
            HLSLPROGRAM
            #define _SPECULAR_COLOR
            #pragma vertex Vertex
            #pragma fragment Fragment
            #pragma shader_feature _FORWARD_PLUS
            #pragma shader_feature_fragment _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma shader_feature_fragment _ADDITIONAL_LIGHT_SHADOWS
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
            half4 _BaseColor;
            CBUFFER_END

            struct Attributes
            {
                float3 positionLS : POSITION;
                float3 normalsLS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS :TEXCOORD1;
            };

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionLS);
                output.normalWS = TransformObjectToWorldNormal(input.normalsLS);
                output.positionWS = TransformObjectToWorld(input.positionLS);
                return output;
            }

            half4 Fragment(Varyings v) : SV_Target
            {
                InputData lighting = (InputData)0;
                lighting.positionWS = v.positionWS;
                lighting.normalWS = normalize(v.normalWS);
                lighting.viewDirectionWS = GetWorldSpaceViewDir(v.positionWS);
                lighting.shadowCoord = TransformWorldToShadowCoord(v.positionWS);
                
                SurfaceData surface = (SurfaceData) 0;
                surface.albedo = _BaseColor;
                surface.alpha = 1;
                surface.smoothness = .9;
                surface.specular = .9;
                return UniversalFragmentBlinnPhong(lighting, surface) + unity_AmbientSky;
            }
            
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags {"LightMode" = "ShadowCaster" }
             
             HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float3 _lightDirection;

            struct Attributes
            {
                float3 positionLS : POSITION;
                float3 normalsLS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
            };

            float4 GetShadowPositionHClip(Attributes input)
            {
                float3 positionWS
                float4 positionCS = 0;
                return positionCS;
            }

            Varyings Vertex(Attributes input)
            {
                Varyings output;
                output.positionCS = GetShadowPositionHClip(input);
                return output;
            }

            half4 Fragment(Varyings v) : SV_Target
            {
                return 0;
            }
            
            ENDHLSL
        }
    }
}
