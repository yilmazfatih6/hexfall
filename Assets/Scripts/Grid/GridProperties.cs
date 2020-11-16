using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProperties : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static GridProperties Instance { get; private set; }

    [SerializeField]
    private Vector2 gridSize = new Vector2(8, 9);
    public Vector3 GridSize { get { return gridSize; } }
    #endregion

    #region Functions
    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

}
