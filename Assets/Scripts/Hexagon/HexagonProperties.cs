using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexagonProperties : MonoBehaviour
{
    #region Singleton Pointer
    public static HexagonProperties Instance { get; private set; }
    #endregion

    // List of assignable materials
    [SerializeField]
    private List<Material> materials;
    public List<Material> Materials { get { return materials; } }

    // Hexagon Frame Prefab
    [SerializeField]
    private GameObject hexagonFramePrefab;
    public GameObject HexagonFramePrefab { get { return hexagonFramePrefab; } }

    // Hexagon Bomb Prefab
    [SerializeField]
    private GameObject hexagonBombPrefab;
    public GameObject HexagonBombPrefab { get { return hexagonBombPrefab; } }

    // Hexagon Bomb Counter Prefab
    [SerializeField]
    private GameObject hexagonBombCounterPrefab;
    public GameObject HexagonBombCounterPrefab { get { return hexagonBombCounterPrefab; } }

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
}
