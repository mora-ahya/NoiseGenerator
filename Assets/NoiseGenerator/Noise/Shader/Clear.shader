Shader "MyShader/Noise/Clear"
{
    Properties
    {
        _MainTex("InputTex", 2D) = "white" {}
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

            sampler2D   _MainTex;
            float4 _MainTex_ST;

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
                return float4(0.0,0.0,0.0,0.0);
            }
            ENDHLSL
        }
    }
}
