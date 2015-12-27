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

        float2 _Interval;           // min, max
        float2 _NoiseAmplitude;
        float2 _NoiseFrequency;
        float2 _NoiseMotion;
        float _RandomSeed;

        float3 _Point0;
        float3 _Point1;
        fixed4 _Color;

        // pseudo random number generator
        float nrand(float2 uv, float salt)
        {
            uv += float2(_RandomSeed, salt);
            return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
        }

        // displacement function
        float displace(float p, float t)
        {
            float offs = _RandomSeed * 1000;
            return
                snoise(float2(_NoiseFrequency.x * p + offs, t * _NoiseMotion.x)) * _NoiseAmplitude.x +
                snoise(float2(_NoiseFrequency.y * p + offs, t * _NoiseMotion.y)) * _NoiseAmplitude.y;
        }

        float intensity(float t0)
        {
            return (nrand(t0 + _RandomSeed, 3) > 0.8) * nrand(t0 + _RandomSeed, 5);
//            return abs(snoise(float2(t * 8 + _RandomSeed, offs))) * (nrand(t2) > 0.5);
        }

        void vert(inout appdata_full v)
        {
            // animation interval
            float interval = lerp(_Interval.x, _Interval.y, nrand(0, 0));

            // absolute time
            float t = _Time.y;

            // start time of the current interval
            float t0 = floor(t / interval) * interval;

            // parameter of the current point
            float lp = v.vertex.x;

            // get displacement of the current point
            float dt = (t - t0) * 20;
            float dx = displace(lp + dt, t0 * 10 + t);
            float dy = displace(lp + dt, t0 * 10 + t - 100);

//            v.vertex.xyz = lerp(_Point0, _Point1, lp) + float3(0, dx, dy);
            float3 p0 = _Point0 + float3(nrand(t0, 4), nrand(t0, 5), nrand(t0, 6)) * 1 - 0.5;
            float3 p1 = _Point1 + float3(nrand(t0, 7), nrand(t0, 8), nrand(t0, 9)) * 1 - 0.5;
            v.vertex.xyz = lerp(p0, p1, lp + dt) + float3(0, dx, dy);

            v.color = _Color * intensity(t0);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            clip(IN.color.r - 0.5);
            o.Emission = IN.color.rgb;
        }

        ENDCG
    } 
    FallBack "Diffuse"
}
