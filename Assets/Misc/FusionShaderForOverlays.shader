Shader "Unlit/FusionShaderForOverlayes"
{
    Properties
    {
        _Blend("Blend Factor", Range(0,1)) = 1.0
        _TVTex("TV Effect Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        // Grab the scene behind the UI panel
        GrabPass { }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _GrabTexture;  // The grabbed background
            sampler2D _TVTex;        // Your current TV-effect frame
            float4 _TVTex_ST;
            float _Blend;            // How much of the TV effect to apply

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv     : TEXCOORD0;
                // Calculate UV for the grabbed texture.
                float2 grabUV : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // For the TV texture – scale/offset if needed
                o.uv = TRANSFORM_TEX(v.uv, _TVTex);

                // For the grab texture, we need to map clip space to [0,1]
                o.grabUV = o.vertex.xy / o.vertex.w * 0.5 + 0.5;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Sample the background scene from the grab pass
                fixed4 background = tex2D(_GrabTexture, i.grabUV);
                // Sample the TV-effect texture (which is being cycled over time)
                fixed4 tvFrame = tex2D(_TVTex, i.uv);

                // **Blend Equation**
                // Here are a few examples of blend modes:
                //
                // 1. Simple linear interpolation (lerp):
                // fixed4 blended = lerp(background, tvFrame, _Blend);
                //
                // 2. Multiply blend:
                // fixed4 blended = lerp(background, background * tvFrame, _Blend);
                //
                // 3. Screen blend:
                // fixed4 blended = lerp(background, 1 - (1 - background) * (1 - tvFrame), _Blend);

                // In this example, we use a multiply blend:
                fixed4 blended = lerp(background, background * tvFrame, _Blend);

                return blended;
            }
            ENDCG
        }
    }
}
