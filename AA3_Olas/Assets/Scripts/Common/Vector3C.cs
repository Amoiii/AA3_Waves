using System;

[System.Serializable]
public struct Vector3C
{
    #region FIELDS
    public float x;
    public float y;
    public float z;
    #endregion

    #region PROPIERTIES
    public float r { get => x; set => x = value; }
    public float g { get => y; set => y = value; }
    public float b { get => z; set => z = value; }
    public float magnitude { get { return (float)Math.Sqrt(x * x + y * y + z * z); } }
    public Vector3C normalized { get { return new Vector3C(x / magnitude, y / magnitude, z / magnitude); } }

    public static Vector3C zero { get { return new Vector3C(0, 0, 0); } }
    public static Vector3C one { get { return new Vector3C(1, 1, 1); } }
    public static Vector3C right { get { return new Vector3C(1, 0, 0); } }
    public static Vector3C up { get { return new Vector3C(0, 1, 0); } }
    public static Vector3C forward { get { return new Vector3C(0, 0, 1); } }

    public static Vector3C black { get { return new Vector3C(0, 0, 0); } }
    public static Vector3C white { get { return new Vector3C(1, 1, 1); } }
    public static Vector3C red { get { return new Vector3C(1, 0, 0); } }
    public static Vector3C green { get { return new Vector3C(0, 1, 0); } }
    public static Vector3C blue { get { return new Vector3C(0, 0, 1); } }
    #endregion

    #region CONSTRUCTORS
    public Vector3C(float x = 0, float y = 0, float z = 0)
    {
        this.x = x; this.y = y; this.z = z;
    }

    public Vector3C CreateVector(Vector3C pointA, Vector3C pointB)
    {
        return pointA - pointB;
    }
    #endregion

    #region OPERATORS
    public static Vector3C operator +(Vector3C a)
    {
        return a;
    }
    public static Vector3C operator -(Vector3C a)
    {       
        return new Vector3C(-a.x, -a.y, -a.z);
    }

    public static bool operator ==(Vector3C v1, Vector3C v2)
    {
        if (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(Vector3C v1, Vector3C v2)
    {
        if (v1.x == v2.x && v1.y == v2.y && v1.z == v2.z)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static Vector3C operator +(Vector3C v1, Vector3C v2)
    {
        return new Vector3C(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
    }

    public static Vector3C operator -(Vector3C v1, Vector3C v2)
    {
        return new Vector3C(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
    }

    public static Vector3C operator *(Vector3C v1, Vector3C v2)
    {
        return new Vector3C(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
    }

    public static Vector3C operator /(Vector3C v1, Vector3C v2)
    {
        return new Vector3C(v1.x / v2.x, v1.y / v2.y, v1.z / v2.z);
    }

    public static Vector3C operator *(Vector3C v1, float value)
    {
        return new Vector3C(v1.x * value, v1.y * value, v1.z * value);
    }

    public static Vector3C operator *(float value, Vector3C v1)
    {
        return new Vector3C(v1.x * value, v1.y * value, v1.z * value);
    }

    public static Vector3C operator /(Vector3C v1, float value)
    {
        return new Vector3C(v1.x / value, v1.y / value, v1.z / value);
    }
    #endregion

    #region METHODS
    public Vector3C Normalize(Vector3C vector)
    {
        vector.x /= vector.magnitude;
        vector.y /= vector.magnitude;
        vector.z /= vector.magnitude;

        return new Vector3C(vector.x, vector.y, vector.z);
    }

    public float Magnitude(Vector3C vector)
    {
        return vector.magnitude;
    }

    public override bool Equals(object obj)
    {
        if (obj is Vector3C)
        {
            Vector3C other = (Vector3C)obj;
            return other == this;
        }
        return false;
    }
    #endregion

    #region FUNCTIONS
    public static float Dot(Vector3C v1, Vector3C v2)
    {
        return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
    }
    public static Vector3C Cross(Vector3C v1, Vector3C v2)
    {
        return new Vector3C(v1.y * v2.z - v1.z * v2.y, v1.x * v2.z + v1.z * v2.x, v1.x * v2.y - v1.y * v2.x);
    }
    public static Vector3C ProjectionVector(Vector3C vToProject, Vector3C vToBeProjected)
    {
        Vector3C projection;
        projection = vToBeProjected.Normalize(vToBeProjected) * (Dot(vToProject, vToBeProjected) / vToBeProjected.Magnitude(vToBeProjected));
        return projection;
    }

    public static Vector3C Interpolation3D(Vector3C pointA, Vector3C pointB, Vector3C vector, float t)
    {
        float x = (1 - t) * pointA.x + t * pointB.x;
        float y = (1 - t) * pointA.y + t * pointB.y;
        float z = (1 - t) * pointA.z + t * pointB.z;

        return new Vector3C(x, y, z);
    }
    #endregion
    public static Vector3C Reflect(Vector3C inDirection, Vector3C inNormal)
    {
        return inDirection - 2 * Dot(inDirection, inNormal) * inNormal;
    }
}
