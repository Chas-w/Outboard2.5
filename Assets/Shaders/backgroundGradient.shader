Shader "Custom/backgroundGradient"
{
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float2 uv = i.uv;

                float mix = uv.y + 0.05;

                float3 color1 = float3(1, 0, 0);
                float3 color2 = float3(0, 0, 0.5);
               
                float3 color = lerp(color1, color2, mix);

                 //white noise
                float wn = 0;
                //create uv grid to pixelate
                float wnUV = floor(uv * 128);
                float d = dot(uv, float2(200.234, -73.598));
                wn = frac(sin(d) * 121);

                
                //add white noise over posterized pixels
                color = lerp(color, cos(50 * _Time.z * wn).r, 0.1);

                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
