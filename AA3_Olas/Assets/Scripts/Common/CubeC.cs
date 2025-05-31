using UnityEngine;

[System.Serializable]
public struct CubeC
{
    #region FIELDS
    public Vector3C position;
    public float scale;
    public Vector3C rotation;
    public float radius;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public CubeC(Vector3C position, float scale, Vector3C rotation)
    {
        this.position = position;
        this.scale = scale;
        this.rotation = rotation;
        this.radius = scale * Mathf.Sqrt(3);
    }
    #endregion

    #region OPERATORS
    public static bool operator !=(CubeC a, CubeC b)
    {
        if (a.position == b.position && a.scale == b.scale && a.rotation == b.rotation && a.radius == b.radius)
        {
            return false;
        }
        return true;
    }
    public static bool operator ==(CubeC a, CubeC b)
    {
        if (a.position == b.position && a.scale == b.scale && a.rotation == b.rotation && a.radius == b.radius)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region METHODS
    public bool IsInside(Vector3C position, CubeC cube) // Revisar (sin rotacion)
    {
        if (position.x <= cube.position.x + (cube.scale / 2) && position.x >= cube.position.x - (cube.scale / 2) &&
            position.y <= cube.position.y + (cube.scale / 2) && position.y >= cube.position.y - (cube.scale / 2) &&
            position.z <= cube.position.z + (cube.scale / 2) && position.z >= cube.position.z - (cube.scale / 2))
        {
            return true;
        }
        return false;
    }
    /*
    public Vector3C NearestPoint(Vector3C position, CubeC cube) // Revisar
    {

        return;
    }
    */
    /*
    public Vector3C IntersectionWithLine() // Revisar
    {
        return;
    }
    */
    public override bool Equals(object obj)
    {
        if (obj is CubeC)
        {
            CubeC other = (CubeC)obj;
            return other == this;
        }
        else
            return false;

    }
    #endregion

    #region FUNCTIONS
    #endregion
}