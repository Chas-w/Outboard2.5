Shader "Custom/test"
{
    Properties
    {
        _scale ("noise scale", Range(2, 50)) = 15.5
        _displacement ("displacement", Range(0, 0.75)) = 0.33
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _scale;
            float _displacement;

            #define TAU 6.28318530718


            float rand (float2 uv) {
                return frac(sin(dot(uv.xy, float2(12.9898, 78.233))) * 43758.5453123);
            }

            float value_noise (float2 uv) {
                float2 ipos = floor(uv);
                float2 fpos = frac(uv); 
                
                float o  = rand(ipos);
                float x  = rand(ipos + float2(1, 0));
                float y  = rand(ipos + float2(0, 1));
                float xy = rand(ipos + float2(1, 1));

                float2 smooth = smoothstep(0, 1, fpos);
                return lerp( lerp(o,  x, smooth.x), 
                             lerp(y, xy, smooth.x), smooth.y);
            }

            float fractal_noise (float2 uv) {
                float n = 0;

                n  = (1 / 2.0)  * value_noise( uv * 1);
                n += (1 / 4.0)  * value_noise( uv * 2); 
                n += (1 / 8.0)  * value_noise( uv * 4); 
                n += (1 / 16.0) * value_noise( uv * 8);
                
                return n;
            }

            struct MeshData
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float disp : TEXCOORD1;
               
            };

            Interpolators vert (MeshData v) //moving mesh
            {
                Interpolators o;

                o.disp = sin(((v.uv.x + v.uv.y) * _scale) + _Time.z) * 0.5 + 0.5;

                float fn = fractal_noise(v.uv * _scale);

                v.vertex.xyz += v.normal * fn * (o.disp/1.5) * _displacement;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                
                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            { 
                 //definitions
                float gridSize = 50; //define grid size
                float2 uv = i.uv * gridSize; //define uvs

                 uv = uv*2 - 1; //scale uv
                
                 float fn = fractal_noise(uv);
                 
                float4 time = _Time; //define time
                float3 output = 0; //create output


                //color
                float3 c = float3(1, 1, 1);
                float3 colorB = float3(0.003, 0.003, .01);

              
                //float3 c = lerp(colorA, 4, colorB);
                //

                //stars?? 
                float2 gridUV = frac(uv) * 10 - 5; //create uv grid
                
                float offset = floor(uv.x) + floor(uv.y); //no offset for bouncing ball
                
                
                float t = _Time.z;

                gridUV.x *= sin(t) - fn; //transform on x axis using sin
               
                //took out y axis tranforms cause then it did not "bounce"
                 gridUV.y /= cos(t) - fn;


                float cutoff = 0.5; //cutoff var shrinks ball so it fits in the grid

                
                //output
               // output = c * (0.2 + 0.5);
                output = c + 1.5;
                output *= fractal_noise(i.uv * _scale).rrr;
                output += step(cutoff, 1-length(gridUV));
                output -= fn + step(cutoff, 1-length(gridUV));
                
               

                return float4(output, 1.0);
            }
            ENDCG
        }
    }
}
