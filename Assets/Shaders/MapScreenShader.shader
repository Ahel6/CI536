Shader "Unlit/MapScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _QuantizeAmount ("Float with range", Range(0, 30)) = 10
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            int _QuantizeAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                //float lum = (0.2 * col.r + 0.7 * col.g + 0.1 * col.b);

                //lum = (int)(lum * _QuantizeAmount);
                //lum /= _QuantizeAmount;

                //return lum.xxxx;

                col = floor(col * _QuantizeAmount);
                col /= _QuantizeAmount;

                return col.xyzx;
            }
            ENDCG
        }
    }
}