Shader "Hidden/Lightning"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard vertex:vert nolightmap
        #pragma target 3.0

        #include "SimplexNoise2D.cginc"

        struct Input {
            fixed4 color : COLOR;
        };

        float2 _Interval;   // min, max
        float2 _Length;     // min, max

        float2 _NoiseAmplitude;
        float2 _NoiseFrequency;
        float2 _NoiseMotion;

        float4 _Point0;     // x, y, z, radius
        float4 _Point1;     // x, y, z, radius
        fixed4 _Color;

        // pseudo random number generator
        float nrand(float seed, float salt)
        {
            float2 uv = float2(seed, salt);
            return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
        }

        // random point distributed in a unit cube
        float3 random_point(float seed, float salt)
        {
            float rx = nrand(seed, salt);
            float ry = nrand(seed, salt + 1);
            float rz = nrand(seed, salt + 2);
            return float3(rx, ry, rz) - 0.5;
        }

        // displacement function
        float displace(float p, float t)
        {
            return
                snoise(float2(_NoiseFrequency.x * p, t * _NoiseMotion.x)) * _NoiseAmplitude.x +
                snoise(float2(_NoiseFrequency.y * p, t * _NoiseMotion.y)) * _NoiseAmplitude.y;
        }

        // vertex intensity function
        float intensity(float seed)
        {
            return (nrand(seed, 30) > 0.9) * nrand(seed, 31) - 0.01;
        }

        void vert(inout appdata_full v)
        {
            float lp   = v.vertex.x;    // parameter on the line segment
            float seed = v.vertex.y;    // random seed

            // animation interval
            float interval = lerp(_Interval.x, _Interval.y, nrand(seed, 0));

            float t = _Time.y;          // absolute time
            float tpi = t / interval;
            float t1 = frac(tpi);       // time parameter [0 - 1]
            float t0 = floor(tpi);      // interval count

            // modify lp with segment length parameters
            float seglen = lerp(_Length.x, _Length.y, nrand(seed + t0, 1));
            lp = lerp(t1, lp, seglen);

            // start point, end point
            float3 p0 = _Point0.xyz + random_point(seed + t0, 10) * _Point0.w * 2;
            float3 p1 = _Point1.xyz + random_point(seed + t0, 20) * _Point1.w * 2;

            // get displacement of the current point
            float dx = displace(lp + seed * 100, t0 * 10 + t);
            float dy = displace(lp - seed * 100, t0 * 10 + t);

            // calculate the position
            v.vertex.xyz = lerp(p0, p1, lp) + float3(0, dx, dy);

            // intensity at this vertex
            v.color = _Color * intensity(t0 + seed);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            clip(IN.color.r);
            o.Emission = IN.color.rgb;
        }

        ENDCG
    } 
    FallBack "Diffuse"
}
