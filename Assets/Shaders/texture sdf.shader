Shader "examples/week 10/texture sdf"
{
    Properties {

        _MainTex ("render texture", 2D) = "white"{}
        [NoScaleOffset]_tex ("texture", 2D) = "white" {}
        _threshold ("threshold", Range(0,1)) = 0.5
        _softness ("softness", Range(0,1)) = 0
        _outlineThreshold ("outline threshold", Range(0,1)) = 0
        _color("Color", Color) = (1, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _tex;
            float _threshold;
            float _softness;
            float _outlineThreshold;
            float4 _color;


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

            #define TAU 6.2831530718

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = 0;
                float2 uv = i.uv;
                float3 mt = tex2D(_MainTex, uv);


                //center mask
                uv = i.uv * 2 - 1;
                float radius = 0.3;
                float d = length(uv) - radius;

                //allow threshold to be modified over time
                float threshold = _threshold;
                
                //sdf
                float df = tex2D(_tex, i.uv).r;

                float shape = smoothstep(threshold, threshold + _softness, df);
                float outline = smoothstep(_outlineThreshold, _outlineThreshold + _softness, df);
                color = lerp(0, 1, outline);

                //more translucent in the middle
                shape -= (1 - d) * 2 -1 ;
                //add color
                color *= _color;

                //create wn
                //white noise
                float wn = 0;
                //create uv grid to pixelate
                float wnUV = floor(uv * 128);
                float wd = dot(uv, float2(200.234, -73.598));
                wn = frac(sin(wd) * 121);
                float op = cos(50 * _Time.z * wn).r;

                color = lerp(color, op, 0.05);

                color = mt * color;

                return float4(color, shape);
            }
            ENDCG
        }
    }
}
