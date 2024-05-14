// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/TestPlaneDeformation"
{
    Properties {
        _MainColor ("Main Color", Color) = (1, 1, 1, 1)
        _DisplacementAmount ("Displacement Amount", Range(0, 10)) = 1
        _MoveSpeed ("Move Speed", Range(0, 10)) = 1
        _NoiseScale ("Noise Scale", Range(0.01, 10)) = 1
        _MaskTexture ("Mask Texture", 2D) = "white" {}
    }

    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
            float3 normal;
        };

        struct VertexOutput {
            float2 uv_MainTex;
            float3 worldPos;
            float3 normal;
        };

        sampler2D _MaskTexture;
        float _DisplacementAmount;
        float _MoveSpeed;
        float _NoiseScale;
        float4 _MainColor;

        float GradientNoise(float2 uv) {
            uv *= _NoiseScale;
            float4 noise = tex2Dlod(_MaskTexture, float4(uv, 0, 0));
            return dot(noise, float4(127.54, 78.54, 43.54, 12.54));
        }

        VertexOutput vert(inout appdata_full v) {
            VertexOutput o;
            o.worldPos = v.vertex.xyz;
            o.uv_MainTex = v.texcoord.xy;
            o.normal = v.normal;

            float2 uv = o.worldPos.xz * 0.1;
            uv.x += _Time.y * _MoveSpeed;
            float noise = GradientNoise(uv);

            // Créer un masque pour le couloir
            float mask = tex2D(_MaskTexture, o.uv_MainTex).r;
            noise *= mask;

            // Déplacer les vertices sur l'axe Y
            o.worldPos.y += noise * _DisplacementAmount;

            // Recalculer la normale
            float3 newNormal = normalize(float3(noise, 1, noise));
            o.normal = newNormal; // Utiliser directement la normale calculée

            return o;
        }

        void surf(Input IN, inout SurfaceOutput o) {
            // Définir la couleur et la normale
            o.Albedo = _MainColor.rgb;
            o.Alpha = _MainColor.a;
            o.Normal = IN.normal; // Utiliser la normale de la structure Input
        }
        ENDCG
    }
    FallBack "Diffuse"
}
