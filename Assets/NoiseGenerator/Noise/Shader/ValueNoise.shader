Shader "MyShader/Noise/ValueNoise"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
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

            float4      _Color;
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

                float f1 = random12(i_uv);
                float f2 = random12(i_uv + float2(1.0, 0.0));
                float f3 = random12(i_uv + float2(0.0, 1.0));
                float f4 = random12(i_uv + float2(1.0, 1.0));

                float o1 = lerp(f1, f2, interpolation(f_uv.x));
                float o2 = lerp(f3, f4, interpolation(f_uv.x));
                float o = lerp(o1, o2, interpolation(f_uv.y));
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
