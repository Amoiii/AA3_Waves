using System;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public struct SphereC
{
    #region FIELDS
    public Vector3C position;
    public float radius;
    #endregion

    #region PROPIERTIES
    //public static SphereC unitary() { return new SphereC(position, 1); }
    #endregion

    #region CONSTRUCTORS
    public SphereC(Vector3C position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    #endregion

    #region OPERATORS
    public static bool operator !=(SphereC a, SphereC b)
    {
        if (a.position == b.position && a.radius == b.radius)
        {
            return false;
        }
        return true;
    }
    public static bool operator ==(SphereC a, SphereC b)
    {
        if (a.position == b.position && a.radius == b.radius)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region METHODS
    public bool IsInside(Vector3C pos, SphereC sphere)
    {
        Vector3C v = pos - sphere.position;
        if (Mathf.Sqrt(v.x * v.x + v.y * v.y + v.z * v.z) <= sphere.radius)
        {
            return true;
        }
        return false;
    }

    public Vector3C NearestPoint(Vector3C pos, SphereC sphere) // Revisar
    {
        Vector3C v = pos - sphere.position;
        return sphere.position + (v.normalized * sphere.radius);
    }

    public override bool Equals(object obj)
    {
        if (obj is SphereC)
        {
            SphereC other = (SphereC)obj;
            return other == this;
        }
        else
            return false;

    }
    #endregion

    #region FUNCTIONS
    #endregion
}