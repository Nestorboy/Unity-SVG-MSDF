Shader "Nessie/UI/MSDF Unlit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Tint", Color) = (1.0, 1.0, 1.0, 1.0)
        [HideInInspector] _PixelRange ("Pixel Range", Float) = 4.0

        [Enum(UnityEngine.Rendering.CullMode)] _CullMode ("Cull Mode", Int) = 2
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
        }

        Cull [_CullMode]
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Base"
            CGPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag
            #pragma target 2.0

            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "MSDF.cginc"

            struct Attributes
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)

                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _PixelRange;

            Varyings Vert(Attributes v)
            {
                Varyings o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color;

                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            float SampleMainMSDF(float2 uv)
            {
                float2 unitRange = UnitRange(_PixelRange, _MainTex_TexelSize.xy);
                return SampleMSDF(_MainTex, uv, unitRange);
            }

            fixed4 Frag(Varyings i) : SV_Target
            {
                float shape = SampleMainMSDF(i.texcoord);
                half4 color = i.color;
                color.a *= shape;

                UNITY_APPLY_FOG(i.fogCoord, color);
                return color;
            }

            ENDCG
        }
    }
}
