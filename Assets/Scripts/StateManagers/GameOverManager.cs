using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static GameOverManager Instance { get; private set; }
    // Publisher for state exit
    public static event EventHandler OnStateExit;
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
        RespawnManager.OnStateExit += RespawnManager_OnStateExit;
    }

    private void RespawnManager_OnStateExit(object sender, EventArgs e)
    {
        EnterState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Game Over State");
        SetGameOver();
    }

    // Exit this state
    private void ExitState()
    {
        Debug.Log("Exit Game Over State");
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private bool IsGameOver()
    {
        // Check counter for bomb hexagons.
        foreach (GameObject hexagon in GridManager.BombHexagons)
        {
            if (ExplosionManager.Instance.IsCheckingInput)
                hexagon.GetComponent<Hexagon>().DecreaseBombCounter();

            if (hexagon.GetComponent<Hexagon>().HexagonBombCounter == 0)
                return true;
        }

        /*
        // Check if any move left.
        if (!AnyMoveLeft())
        {
            Debug.Log("No more move left. Game is over...");
            return true;
        }
        else
        {
            Debug.Log("Moves left. Game is going on...");
        }
        */
        return false;
    }

    private void SetGameOver()
    {
        if (IsGameOver())
        {
            // Stop score
            ScoreManager.StopScore();

            // Destroy all hexagons.
            foreach (var hexagon in GridManager.Hexagons)
                Destroy(hexagon);

            // Display game over screen.
            UserInterfaceManager.Instance.DisplayGameOverScreen();

            // set game state 
            GameStartManager.Instance.isGameStarted = false;

            Debug.Log("Game Over");
        }
        else
        {
            ExitState();
        }
    }

    public bool AnyMoveLeft()
    {
        Debug.Log("Checking if any move left...");
        foreach (var hexagon in GridManager.Hexagons)
        {
            foreach (var neighbour in GridManager.GetNeighbours(hexagon))
            {
                if (neighbour == null)
                    continue;

                // Swipe positions
                hexagon.transform.position = neighbour.transform.position;

                // Check if explodes
                if (hexagon.GetComponent<Hexagon>().CheckExplosion() != null)
                    return true;

                // Swipe back positions
                hexagon.transform.position = neighbour.transform.position;
            }

        }
        return false;
    }

    #endregion
}
