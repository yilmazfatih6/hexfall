using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RespawnManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static RespawnManager Instance { get; private set; }
    // Publisher for state exit
    public static event EventHandler OnStateExit;
    // When set to true spawns one bomb hexagon
    private bool spawnBombHexagon = false;
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
        // Subscribe
        RepositionManager.OnStateExit += RepositionManager_OnStateExit;
    }

    private void RepositionManager_OnStateExit(object sender, EventArgs e)
    {
        EnterState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Respawn State");
        Respawn();
    }

    // Exit this state
    private void ExitState()
    {
        Debug.Log("Exit Respawn State");
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void Respawn()
    {
        foreach (var position in GridManager.GridPositions)
        {
            if (GridManager.GetHexagon(position) == null)
            {
                if (spawnBombHexagon)
                {
                    SpawnManager.Instance.SpawnBombHexagon(position);
                    spawnBombHexagon = false;
                }
                else
                    SpawnManager.Instance.SpawnHexagon(position);
            }

        }

        ExitState();
    }

    // Called from score manager in every 1000 score to spawn bomb hexagon.
    public void SpawnBombHexagon()
    {
        Debug.Log("Spawn bomb hexagon.");
        spawnBombHexagon = true;
    }
    #endregion
}
