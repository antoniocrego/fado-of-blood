Shader "Custom/URP_DoubleSided_HLSL_Converted"
{
    Properties
    {
        _Color("Albedo Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB) A:Opacity", 2D) = "white" {}
        _Cutoff("Alpha Cutoff", Range(0,1)) = 0.5

        _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Map Scale", Float) = 1.0

        _EmissionColor("Emission Color", Color) = (0,0,0,1)
        _EmissionMap("Emission Map", 2D) = "black" {}
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "RenderType"="Opaque" }
        LOD 300
        Cull Off  // Double-sided rendering

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            // You can adjust blending and ZWrite as needed:
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On

            HLSLPROGRAM
            // Target shader model 3.0 for URP
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ MAIN_LIGHT_SHADOWS

            // Include URP core and lighting functions
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // Vertex input structure
            struct Attributes
            {
                float4 vertex   : POSITION;
                float2 uv       : TEXCOORD0;
                float3 normal   : NORMAL;
                float4 tangent  : TANGENT;
            };

            // Data passed from vertex to fragment shader
            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float3 tangentWS    : TEXCOORD2;
                float3 bitangentWS  : TEXCOORD3;
            };

            // _MainTex_ST is provided automatically by Unity
            float4 _MainTex_ST;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.vertex);
                OUT.uv = IN.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                OUT.normalWS = normalize(TransformObjectToWorldNormal(IN.normal));
                OUT.tangentWS = normalize(TransformObjectToWorldDir(IN.tangent.xyz));
                OUT.bitangentWS = cross(OUT.normalWS, OUT.tangentWS) * IN.tangent.w;
                return OUT;
            }

            sampler2D _MainTex;
            float4 _Color;
            float _Cutoff;

            sampler2D _BumpMap;
            float _BumpScale;

            sampler2D _EmissionMap;
            float4 _EmissionColor;

            half4 frag(Varyings IN) : SV_Target
            {
                // Sample albedo and apply tint
                float4 albedo = tex2D(_MainTex, IN.uv) * _Color;
                clip(albedo.a - _Cutoff);

                // Start with the provided world-space normal
                float3 normalWS = IN.normalWS;
                #if defined(_BumpMap)
                    // Sample and unpack the normal map
                    float3 normalTS = UnpackNormal(tex2D(_BumpMap, IN.uv)) * _BumpScale;
                    // Construct the TBN matrix
                    float3x3 TBN = float3x3(normalize(IN.tangentWS), normalize(IN.bitangentWS), normalWS);
                    normalWS = normalize(mul(normalTS, TBN));
                #endif

                // Fetch main light data from URP's lighting functions
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float NdotL = saturate(dot(normalWS, -lightDir));
                float3 diffuse = albedo.rgb * mainLight.color.rgb * NdotL;

                // Emission from emission map
                float3 emission = tex2D(_EmissionMap, IN.uv).rgb * _EmissionColor.rgb;

                float3 finalColor = diffuse + emission;
                return half4(finalColor, albedo.a);
            }
            ENDHLSL
        }
    }
    FallBack "Universal Forward"
}
