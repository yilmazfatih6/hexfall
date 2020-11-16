using System.Collections.Generic;
using UnityEngine;

public static class NeighbourOffsets
{
    public static readonly Vector2 Top = new Vector2(0, Hexagon.shortRadius + Hexagon.margin);
    public static readonly Vector2 Bottom = new Vector2(0, -1 * Hexagon.shortRadius - Hexagon.margin);
    public static readonly Vector2 TopRight = new Vector2(Hexagon.longRadius * .75f + Hexagon.margin, Hexagon.shortRadius / 2 + Hexagon.margin);
    public static readonly Vector2 BottomRight = new Vector2(Hexagon.longRadius * 0.75f + Hexagon.margin, Hexagon.shortRadius / -2 - Hexagon.margin);
    public static readonly Vector2 TopLeft = new Vector2(Hexagon.longRadius * -0.75f - Hexagon.margin, Hexagon.shortRadius / 2- Hexagon.margin);
    public static readonly Vector2 BottomLeft = new Vector2(Hexagon.longRadius * -0.75f - Hexagon.margin, Hexagon.shortRadius / -2 - Hexagon.margin);

    public static List<Vector2> ToList()
    {
        List<Vector2> returnValue = new List<Vector2>();
        // Add sequenltially
        returnValue.Add(TopRight);
        returnValue.Add(Top);
        returnValue.Add(TopLeft);
        returnValue.Add(BottomLeft);
        returnValue.Add(Bottom);
        returnValue.Add(BottomRight);
        return returnValue;
    }
}
