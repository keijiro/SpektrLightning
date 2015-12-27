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

        float3 _Point0;
        float3 _Point1;

        float3 _Axis0;
        float3 _Axis1;
        float3 _Axis2;

        float2 _Interval;   // min, max
        float2 _Length;     // min, max

        float2 _NoiseAmplitude;
        float2 _NoiseFrequency;
        float2 _NoiseMotion;

        fixed4 _Color;

        // pseudo random number generator
        float nrand01(float seed, float salt)
        {
            float2 uv = float2(seed, salt);
            return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
        }

        float nrand11(float seed, float salt)
        {
            return nrand01(seed, salt) * 2 - 1;
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
            return (nrand01(seed, 30) > 0.9) * nrand01(seed, 31) - 0.01;
        }

        void vert(inout appdata_full v)
        {
            float lp   = v.vertex.x;    // parameter on the line segment
            float seed = v.vertex.y;    // random seed

            // animation interval
            float interval = lerp(_Interval.x, _Interval.y, nrand01(seed, 0));

            float t = _Time.y;          // absolute time
            float tpi = t / interval;
            float t1 = frac(tpi);       // time parameter [0 - 1]
            float t0 = floor(tpi);      // interval count

            // modify lp with segment length parameters
            float seglen = lerp(_Length.x, _Length.y, nrand01(seed + t0, 1));
            lp = lerp(t1, lp, seglen);

            // start point, end point
            float3 p0 = _Point0 + _Axis0 * nrand11(seed + t0, 2) * _NoiseAmplitude.x;
            float3 p1 = _Point1 + _Axis0 * nrand11(seed + t0, 3) * _NoiseAmplitude.x;

            // get displacement of the current point
            float d0 = displace(lp + seed * 100, t0 * 10 + t);
            float d1 = displace(lp - seed * 100, t0 * 10 + t);

            // calculate the position
            v.vertex.xyz = lerp(p0, p1, lp) + _Axis1 * d0 + _Axis2 * d1;

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
