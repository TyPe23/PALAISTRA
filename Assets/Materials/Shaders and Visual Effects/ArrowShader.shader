Shader "Unlit/Arrow"
{
    Properties
    {
        _MainTex('Main Texture', 2D) = 'white' {}
        _OffestAmout('Offset Amount', Range(0, 10)) = 0;
        _AnimationSpeed('Animation Speed', Range(0, 3)) = 0;
        _Color('Color', Color) = (1, 1, 1, 1);
    }
    SubShader
    {
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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            sampler2D _MainTex;
            float _AnimationSpeed;
            float _OffsetAmount;

            v2f vert(appdata v)
            {
                v2f o;
                v.vertex.y += cos(_Time.y * _AnimationSpeed + v.vertex.y * _OffsetAmount);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 pixelColor = tex2D(_MainTexture, i,uv)

                return pixel * _Color
            }
            ENDCG
        }
    }
}
