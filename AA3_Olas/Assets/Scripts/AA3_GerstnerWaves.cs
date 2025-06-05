using System;
using UnityEngine;

[System.Serializable]
public class AA3_GerstnerWaves
{
    [System.Serializable]
    public struct Settings
    {
        public float waterDensity;
        public float gravity;
    }
    public Settings settings;

    [System.Serializable]
    public struct GerstnerSettings
    {
        public float amplitude;    // A
        public float wavelength;   // L
        public float phase;        // φ
        public Vector3C direction; // dirección (Vector3C)
        public float speed;        // c (velocidad de propagación)
    }

    [System.Serializable]
    public struct BuoySettings
    {
        public float radius;
        public float mass;
    }

    public BuoySettings buoySettings;
    public SphereC buoy;

    // Array ondas Gerstner
    public GerstnerSettings[] wavesSettings;

    // Cada vértice de la malla:
    public struct Vertex
    {
        public Vector3C originalPosition; // posición original
        public Vector3C position;         // posición deformada

        public Vertex(Vector3C _pos)
        {
            originalPosition = _pos;
            position = _pos;
        }
    }

    public Vertex[] points;
    public float elapsedTime;
    private float buoyVelocity = 0.0f;

  
    public void Update(float dt)
    {
        elapsedTime += dt;

        // Recorremos la malla
        for (int i = 0; i < points.Length; i++)
        {
            Vector3C p0 = points[i].originalPosition; // (x0, y0=0, z0)
            Vector3C newPos = p0;

            float x0 = p0.x;
            float z0 = p0.z;
            float y = 0f;

            // Aplicar superposición de N ondas Gerstner
            foreach (var wave in wavesSettings)
            {
                // k = 2π / L
                float k = 2.0f * (float)Math.PI / wave.wavelength;
                // θ = k·(d·(x0, z0)) + φ - ωt  con ω = k·c
                float dot = Vector3C.Dot(new Vector3C(x0, 0, z0), wave.direction.normalized);
                float theta = k * dot + wave.phase - (k * wave.speed * elapsedTime);

                // Desfase en X y Z: 
                //  X += A * (d.x / |d|) * cos(θ) / k
                //  Z += A * (d.z / |d|) * cos(θ) / k
                float cosTheta = (float)Math.Cos(theta);
                Vector3C dirNorm = wave.direction.normalized;
                x0 += dirNorm.x * (wave.amplitude * cosTheta / k);
                z0 += dirNorm.z * (wave.amplitude * cosTheta / k);

                // Altura Y: A * sin(θ)
                y += wave.amplitude * (float)Math.Sin(theta);
            }

            newPos.x = x0;
            newPos.y = y;
            newPos.z = z0;
            points[i].position = newPos;
        }

        
        BuoyPosition(dt);
    }

    private void BuoyPosition(float dt)
    {
        float waveHeight = GetWaveHeight(buoy.position.x, buoy.position.z);

        // Profundidad sumergida
        float submergedDepth = Math.Max(0, waveHeight - buoy.position.y);
        submergedDepth = Math.Min(submergedDepth, buoySettings.radius);

        //(π * d^2 * (3R - d)) / 3
        float R = buoySettings.radius;
        float d = submergedDepth;
        float submergedVolume = (float)((Math.PI * d * d) * (3 * R - d) / 3.0);

        // Fuerza de flotación: ρ·g·V_sub
        float buoyancyForce = settings.waterDensity * settings.gravity * submergedVolume;
        // Peso de la boya: m·g
        float weightForce = buoySettings.mass * settings.gravity;
        float netForce = buoyancyForce - weightForce;

        float acceleration = netForce / buoySettings.mass;
        buoyVelocity += acceleration * dt;
        buoy.position.y += buoyVelocity * dt;
    }


    public float GetWaveHeight(float x, float z)
    {
        float y = 0f;
        foreach (var wave in wavesSettings)
        {
            float k = 2.0f * (float)Math.PI / wave.wavelength;
            float dot = Vector3C.Dot(new Vector3C(x, 0, z), wave.direction.normalized);
            float theta = k * dot + wave.phase - (k * wave.speed * elapsedTime);
            y += wave.amplitude * (float)Math.Sin(theta);
        }
        return y;
    }

    public void Debug()
    {
        if (points != null)
        {
            foreach (var v in points)
            {
                v.originalPosition.Print(0.05f);
                v.position.Print(0.05f);
            }
        }
        buoy.Print(Vector3C.red);
    }
}
