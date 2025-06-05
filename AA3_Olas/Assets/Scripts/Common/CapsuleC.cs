using System;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public struct CapsuleC
{
    #region FIELDS
    public Vector3C positionA;
    public Vector3C positionB;
    public float radius;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public CapsuleC(Vector3C positionA, Vector3C positionB, float radius)
    {
        this.positionA = positionA;
        this.positionB = positionB;
        this.radius = radius;
    }

    /* // No lo necesitamos por ahora
    public CapsuleC(Vector3C position, float radius, float height, Vector3C rotation) 
    {
        this.radius = radius;
        this.positionA = position;

        this.positionB = position + rotation * height; // Revisar

    }
    */
    #endregion

    #region OPERATORS
    public static bool operator !=(CapsuleC a, CapsuleC b)
    {
        if (a.positionA == b.positionA && a.positionB == b.positionB && a.radius == b.radius)
        {
            return false;
        }
        return true;
    }
    public static bool operator ==(CapsuleC a, CapsuleC b)
    {
        if (a.positionA == b.positionA && a.positionB == b.positionB && a.radius == b.radius)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region METHODS
    public bool IsInside(Vector3C pos, CapsuleC capsule)
    {
        // Cilinder
        // Projection in line
        LineC line = LineC.LineGivenTwoPoints(capsule.positionA, capsule.positionB);
        Vector3C v1 = new Vector3C(capsule.positionA.x - capsule.positionB.x, capsule.positionA.y - capsule.positionB.y, capsule.positionA.z - capsule.positionB.z);
        Vector3C v2 = new Vector3C(pos.x - capsule.positionB.x, pos.y - capsule.positionB.y, pos.z - capsule.positionB.z);
        Vector3C vectorInLine = ((v1) / (v1.Magnitude(v1))) * (Vector3C.Dot(v1, v2)) / (v1.Magnitude(v1));
        Vector3C pointInLine = new Vector3C(capsule.positionB.x + vectorInLine.x, capsule.positionB.y + vectorInLine.y, capsule.positionB.z + vectorInLine.z);

        // Point inside segment?
        Vector3C vecPA = new Vector3C(pointInLine.x - capsule.positionA.x, pointInLine.y - capsule.positionA.y, pointInLine.z - capsule.positionA.z);
        Vector3C vecPB = new Vector3C(pointInLine.x - capsule.positionB.x, pointInLine.y - capsule.positionB.y, pointInLine.z - capsule.positionB.z);
        Vector3C vecAB = new Vector3C(capsule.positionA.x - capsule.positionB.x, capsule.positionA.x - capsule.positionB.y, capsule.positionA.x - capsule.positionB.z);
        if (vecAB.Magnitude(vecAB) > vecPA.Magnitude(vecPA) && vecAB.Magnitude(vecAB) > vecPB.Magnitude(vecPB))
        {
            // Point inside radius?
            Vector3C v = new Vector3C(pointInLine.x - pos.x, pointInLine.y - pos.y, pointInLine.z - pos.z);
            float d = v.Magnitude(v);

            if (d <= capsule.radius)
            {
                return true;
            }
        }

        // Spheres
        SphereC sphA = new SphereC(capsule.positionA, capsule.radius);
        SphereC sphB = new SphereC(capsule.positionB, capsule.radius);
        // Sphere A
        if (sphA.IsInside(pos, sphA))
            return true;
        // Sphere B
        else if (sphB.IsInside(pos, sphB))
            return true;

        return false;
    }
    public bool IsInsideCilinder(Vector3C pos, CapsuleC capsule)
    {
        // Projection in line
        LineC line = LineC.LineGivenTwoPoints(capsule.positionA, capsule.positionB);
        Vector3C v1 = new Vector3C(capsule.positionA.x - capsule.positionB.x, capsule.positionA.y - capsule.positionB.y, capsule.positionA.z - capsule.positionB.z);
        Vector3C v2 = new Vector3C(pos.x - capsule.positionB.x, pos.y - capsule.positionB.y, pos.z - capsule.positionB.z);
        Vector3C vectorInLine = ((v1)/(v1.Magnitude(v1))) * (Vector3C.Dot(v1, v2)) / (v1.Magnitude(v1));
        Vector3C pointInLine = new Vector3C(capsule.positionB.x + vectorInLine.x, capsule.positionB.y + vectorInLine.y, capsule.positionB.z + vectorInLine.z);

        // Point inside segment?
        Vector3C vecPA = new Vector3C(pointInLine.x - capsule.positionA.x, pointInLine.y - capsule.positionA.y, pointInLine.z - capsule.positionA.z);
        Vector3C vecPB = new Vector3C(pointInLine.x - capsule.positionB.x, pointInLine.y - capsule.positionB.y, pointInLine.z - capsule.positionB.z);
        Vector3C vecAB = new Vector3C(capsule.positionA.x - capsule.positionB.x, capsule.positionA.x - capsule.positionB.y, capsule.positionA.x - capsule.positionB.z);
        if (vecAB.Magnitude(vecAB) > vecPA.Magnitude(vecPA) && vecAB.Magnitude(vecAB) > vecPB.Magnitude(vecPB)) 
        {
            // Point inside radius?
            Vector3C v = new Vector3C(pointInLine.x - pos.x, pointInLine.y - pos.y, pointInLine.z - pos.z);
            float d = v.Magnitude(v);

            if (d <= capsule.radius)
            {
                return true;
            } 
        }
        return false;
    }
    
    public override bool Equals(object obj)
    {
        if (obj is CapsuleC)
        {
            CapsuleC other = (CapsuleC)obj;
            return other == this;
        }
        else
            return false;

    }
    #endregion

    #region FUNCTIONS
    #endregion
}