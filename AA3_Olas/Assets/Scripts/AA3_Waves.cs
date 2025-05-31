using System;

[System.Serializable]
public class AA3_Waves
{
    [System.Serializable]
    public struct Settings
    {
        public float waterDensity;
        public float gravity;
    }
    public Settings settings;

    [System.Serializable]
    public struct WavesSettings
    {
        public float amplitude;
        public float frequency;
        public float phase;
        public Vector3C direction;
        public float speed;
    }

    [System.Serializable]
    public struct BuoySettings
    {
        public float radius;
        public float mass;
    }

    public BuoySettings buoySettings;

    public SphereC buoy;

    public WavesSettings[] wavesSettings;
    public struct Vertex
    {
        public Vector3C originalPosition;
        public Vector3C position;
        public Vertex(Vector3C _position)
        {
            this.position = _position;
            this.originalPosition = _position;
        }
    }
    public Vertex[] points;
    public float elapsedTime;
    private float buoyVelocity = 0.0f;

    public void Update(float dt)
    {
        elapsedTime += dt;
        for (int i = 0; i < points.Length; i++)
        {
            Vector3C original = points[i].originalPosition;
            Vector3C newPosition = original;


            float x = original.x;
            float y = 0.0f;
            float z = original.z;

            foreach (var wave in wavesSettings)
            {
                float k = 2.0f * (float)Math.PI / wave.frequency;
                float w = wave.speed * elapsedTime;
                float dotProduct = k * Vector3C.Dot(new Vector3C(x, 0, z), wave.direction) + wave.phase;

                x += wave.amplitude * k * (float)Math.Cos(dotProduct + w) * wave.direction.x;
                y += wave.amplitude * (float)Math.Sin(dotProduct + w);
                z += wave.amplitude * k * (float)Math.Cos(dotProduct + w) * wave.direction.z;
            }

            newPosition.x = x;
            newPosition.y = y;
            newPosition.z = z;
            points[i].position = newPosition;
        }

        BuoyPosition(dt);
    }

    private void BuoyPosition(float dt)
    {
        float waveHeight = GetWaveHeight(buoy.position.x, buoy.position.z);

        float submergedDepth = Math.Max(0, waveHeight - buoy.position.y);
        submergedDepth = Math.Min(submergedDepth, buoySettings.radius);

        float submergedBuoyVolume = (float)((Math.PI * submergedDepth * submergedDepth) * (3 * buoy.radius - submergedDepth) / 3.0);

        float buoyancyForce = settings.waterDensity * settings.gravity * submergedBuoyVolume;

        float buoyForce = buoySettings.mass * settings.gravity;
        float netForce = buoyancyForce - buoyForce;

        float acceleration = netForce / buoySettings.mass;

        buoyVelocity += acceleration * dt;
        buoy.position.y += buoyVelocity * dt;
    }

    public float GetWaveHeight(float x, float z)
    {
        float y = 0.0f;
        foreach (var wave in wavesSettings)
        {
            float k = 2.0f * (float)Math.PI / wave.frequency;
            float w = wave.speed * elapsedTime;
            float dotProduct = k * Vector3C.Dot(new Vector3C(x, 0, z), wave.direction) + wave.phase;
            y += wave.amplitude * (float)Math.Sin(dotProduct + w);
        }
        return y;
    }

    public void Debug()
    {
        if (points != null)
            foreach (var item in points)
            {
                item.originalPosition.Print(0.05f);
                item.position.Print(0.05f);
            }
        buoy.Print(Vector3C.red);
    }
}