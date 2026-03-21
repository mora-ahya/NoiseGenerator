Shader "MyShader/Noise/fBm"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
        _BaseTex("BaseTex", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
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
            sampler2D   _BaseTex;
            float4 _BaseTex_ST;

            float _Scale;

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
                float2 uv = i.uv;
                float4 resCol = tex2D(_MainTex, uv) * _Scale;
                float4 sourceCol = tex2D(_BaseTex, uv);
                
            #if WHITE_ALPHA
                return float4(1.0, 1.0, 1.0, resCol.a + sourceCol.a);
            #else
                float o = resCol.r + sourceCol.r;
                return float4(o, o, o, 1.0);
            #endif
            }
            ENDHLSL
        }
    }
}
