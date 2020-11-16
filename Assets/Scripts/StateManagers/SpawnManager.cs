using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static SpawnManager Instance { get; private set; }

    // Publisher for state exit
    public static event EventHandler OnStateExit;

    [SerializeField]
    private GameObject hexagonPrefab = null;
    public GameObject HexagonPrefab { get { return hexagonPrefab; } }
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

    private void Start()
    {
        // Substribe
        GameStartManager.Instance.OnStateExit += GameStartManager_OnStateExit;
    }

    // Subscriber for Game Start State Exit 
    private void GameStartManager_OnStateExit(object sender, EventArgs e)
    {
        EnterState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Spawn State");
        InitialSpawn();
    }

    // Exit this state
    private void ExitState()
    {
        Debug.Log("Exit Spawn State");
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    // Called on game start to spawn hexagons for the first time.
    private void InitialSpawn()
    {
        // Run loop for each grid tile position and spawn hexagon.
        foreach (Vector3 position in GridManager.GridPositions)
            SpawnHexagon(position);

        // Exit state
        ExitState();
    }

    // Spawns a hexagon at the given position.
    public GameObject SpawnHexagon(Vector3 positionToSpawn)
    {
        GameObject spawnedHexagon = Instantiate(hexagonPrefab, positionToSpawn, Quaternion.identity);
        return spawnedHexagon;
    }

    // Spawns a hexagon at the given position. Sets it as bomb hexagon.
    public GameObject SpawnBombHexagon(Vector3 positionToSpawn)
    {
        GameObject spawnedHexagon = Instantiate(hexagonPrefab, positionToSpawn, Quaternion.identity);
        spawnedHexagon.GetComponent<Hexagon>().SetAsBombHexagon();
        return spawnedHexagon;
    }
    #endregion

}
