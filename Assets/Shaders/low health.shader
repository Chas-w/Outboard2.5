Shader "examples/week 10/low health"
{
    Properties
    {
        //must use _MainTex
        _MainTex ("render texture", 2D) = "white" {}
        
        _saturation("saturation", Range(0.0, 1.0)) = 0.5
        _vr("vignette radius", Range(0.0, 1.0)) = 1.0
        _vs("vignette softness", Range(0.0, 1.0)) = 0.5

        _color("color", Color) = (1, 1, 1, 1)

        //adding in flowy blood
    }


    SubShader
    {
        //no culling
        Cull Off
        //dont write to depth buffer
        ZWrite Off
        //specify how render pipeline handles depth testing 
        //want to always render no mater what happens in depth testing
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;

            float _saturation;
            float _vr;
            float _vs;

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

            float4 frag (Interpolators i) : SV_Target
            {
                float3 color = 0;
                float2 uv = i.uv;


                //sample tex w UVs 
                color = tex2D(_MainTex, i.uv);
                //desaturate image
                color = lerp(color.rrr, color, _saturation);

                //vignette effect
                float distFromCenter = distance(uv.xy, float2(0.5, 0.5));
                float vignette = smoothstep(_vr, _vr - _vs, distFromCenter);


                //add vignette
                color = saturate(color * vignette);

                //screen tint
                color *= _color;
                
                //color *= color.r * sin(_Time.y * 4) * 0.5 + 1;


                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
