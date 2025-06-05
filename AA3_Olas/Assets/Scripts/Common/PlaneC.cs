[System.Serializable]
public struct PlaneC
{
    #region FIELDS
    public Vector3C position;
    public Vector3C normal;
    #endregion

    #region PROPIERTIES
    #endregion

    #region CONSTRUCTORS
    public PlaneC(Vector3C position, Vector3C normal)
    {
        this.position = position;
        this.normal = normal;
    }
    public PlaneC(Vector3C pointA, Vector3C pointB, Vector3C pointC)
    {
        this.position = new Vector3C();
        this.normal = new Vector3C();
    }
    public PlaneC(Vector3C n, float D)
    {
        this.position = new Vector3C();
        this.normal = new Vector3C();
    }
    #endregion

    #region OPERATORS
    #endregion

    #region METHODS
    public Vector3C IntersectionWithLine(PlaneC plane, LineC line) 
    {
        Vector3C intersectionPoint = new Vector3C(
                line.origin.x + ((plane.normal.x * (plane.position.x - line.origin.x)) / (plane.normal.x * line.direction.x)) * line.direction.x,
                line.origin.y + ((plane.normal.y * (plane.position.y - line.origin.y)) / (plane.normal.y * line.direction.y)) * line.direction.y,
                line.origin.z + ((plane.normal.z * (plane.position.z - line.origin.z)) / (plane.normal.z * line.direction.z)) * line.direction.z
              );

        return intersectionPoint;
    } 
    #endregion

    #region FUNCTIONS
    #endregion

}