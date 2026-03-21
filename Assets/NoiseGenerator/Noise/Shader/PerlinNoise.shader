Shader "MyShader/Noise/PerlinNoise"
{
    Properties
    {
        _MainTex("InputTex", 2D) = "white" {}
        _Offset("Offset", Vector) = (0,0,0,0)
        _Scale ("Scale", Float) = 8
        _Amplitude ("Amplitude", Float) = 1
        [Toggle(WHITE_ALPHA)] _WhiteAlpha ("White Alpha", Float) = 0
    }

    SubShader
    {
        Blend One Zero

        Pass
        {
            HLSLPROGRAM
            #include "Includes/MyCommon.hlsl"
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma shader_feature _ WHITE_ALPHA

            sampler2D   _MainTex;
            float4 _MainTex_ST;

            float4 _Offset;
            float _Scale;
            float _Amplitude;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv * _Scale + _Offset.xy;
                float2 i_uv = floor(uv);
                float2 f_uv = frac(uv);

                float2 v1 = random22(i_uv) * 2.0 - 1.0;
                float2 v2 = random22(i_uv + float2(1.0, 0.0)) * 2.0 - 1.0;
                float2 v3 = random22(i_uv + float2(0.0, 1.0)) * 2.0 - 1.0;
                float2 v4 = random22(i_uv + float2(1.0, 1.0)) * 2.0 - 1.0;

                float o1 = lerp(dot(v1, f_uv), dot(v2, f_uv - float2(1.0, 0.0)), interpolation(f_uv.x));
                float o2 = lerp(dot(v3, f_uv - float2(0.0, 1.0)), dot(v4, f_uv - float2(1.0, 1.0)), interpolation(f_uv.x));
                float o = lerp(o1, o2, interpolation(f_uv.y)) + 0.5;
                o = clamp(o, 0.0, 1.0);
                o *= _Amplitude;

            #if WHITE_ALPHA
                return float4(1.0, 1.0, 1.0, o);
            #else
                return float4(o, o, o, 1.0);
            #endif
            }
            ENDHLSL
        }
    }
}
