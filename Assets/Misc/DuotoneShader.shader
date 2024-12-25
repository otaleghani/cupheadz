Shader "Custom/DuotoneShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("First Color", Color) = (1,0,0,0)
        _Color2 ("Second Color", Color) = (0,0,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            // Disable backface culling
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            fixed4 _Color1;
            fixed4 _Color2;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                float luminance = dot(tex.rgb, float3(0.3, 0.59, 0.11));
                fixed4 color = lerp(_Color1, _Color2, luminance);

                // Preserve original texture alpha
                //color.a = tex.a;
                color.a = lerp(_Color1.a, _Color2.a, luminance) * tex.a;

                return color;
            }
            ENDCG
        }
    }
}
