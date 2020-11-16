using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionHitAreaManager : MonoBehaviour
{
    #region Singleton
    // Singleton pointer
    public static SelectionHitAreaManager Instance { get; private set; }

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    #region Properties
    private int hitArea = 1;
    public int HitArea { get { return hitArea; } }
    #endregion

    #region Function
    /* Calculates which area on hexagon is hit.
     * Takes hit hexagon GameObject and hit information HitInfo2D as inputs.
     * Calculates rotation between two points (in degrees).
     * Divides the hexagon into 6 areas, 60 degrees each.
     */
    public int DetectHitArea(GameObject hexagon, RaycastHit2D hitInfo)
    {
        // Calculate distance vector between two points.
        Vector2 distance = hitInfo.point - new Vector2(hexagon.transform.position.x, hexagon.transform.position.y);

        // Calculate angle between to points, in degrees.
        float angleToHitPoint = Mathf.Atan2(distance.y, distance.x) * 57.2957795f; 
        if (angleToHitPoint < 0)
            angleToHitPoint += 360;

        // Divide angle to 60. Then ceil in to int.
        hitArea = Mathf.CeilToInt(angleToHitPoint / 60);
        return hitArea;
    }

    // Switch to next area. This is called when hit are has not enough neighbours.
    public void SwitchToNextArea()
    {
        if (hitArea != 6)
            hitArea += 1;
        else
            hitArea = 1;
    }

    #endregion

}
