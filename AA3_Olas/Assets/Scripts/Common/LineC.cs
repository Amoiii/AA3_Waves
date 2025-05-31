using System;

[System.Serializable]
public struct LineC
{
    #region FIELDS
    public Vector3C origin;
    public Vector3C direction;
    #endregion

    #region CONSTRUCTORS
    public LineC(Vector3C origin, Vector3C direction)
    {
        this.origin = origin;
        this.direction = direction;
    }
    public Vector3C CreateLine(Vector3C pointA, Vector3C pointB)
    {
        Vector3C direction = pointB - pointA;
        Vector3C normalizedDirection = direction.Normalize(direction);

        return pointA + normalizedDirection;
    }
    #endregion

    #region OPERATORS

    public static bool operator ==(LineC l1, LineC l2)
    {
        if (l1.origin == l2.origin && l1.direction == l2.direction)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator !=(LineC l1, LineC l2)
    {
        if (l1.origin == l2.origin && l1.direction == l2.direction)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    #endregion

    #region METHODS
    public LineC NearestPointToPoint(Vector3C point, LineC line)
    {
        //Creamos el vector desde el punto origen hasta el punto exterior
        Vector3C pointToOrigin = new Vector3C(point.x - line.origin.x, point.y - line.origin.y, point.z - line.origin.z);
        Vector3C dotProductResult = new Vector3C(pointToOrigin.x * line.direction.x, pointToOrigin.y * line.direction.y, pointToOrigin.x * line.direction.x);

        return new LineC();
    }
    #endregion

    #region FUNCTIONS
    public static LineC LineGivenTwoPoints (Vector3C pointA, Vector3C pointB)
    {
        Vector3C vector = new Vector3C();
        
        vector = vector.CreateVector(pointA, pointB);

        LineC line = new LineC(pointA, vector);

        return new LineC();
    }
    #endregion

}