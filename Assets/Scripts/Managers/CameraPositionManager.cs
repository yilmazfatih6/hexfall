using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Set position
        float x = (GridProperties.Instance.GridSize.x * Hexagon.longRadius * 0.75f) / 2;
        float y = (GridProperties.Instance.GridSize.y * Hexagon.shortRadius) / 2;
        transform.position = new Vector3(x, y, transform.position.z);

        // Set size
        this.GetComponent<Camera>().orthographicSize = GridProperties.Instance.GridSize.x - 1;
    }


}
