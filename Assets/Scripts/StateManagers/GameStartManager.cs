using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static GameStartManager Instance { get; private set; }

    // Publisher for state exit
    public event EventHandler OnStateExit;

    public bool isGameStarted = false;
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

    public void StartGame()
    {
        if(!isGameStarted)
        {
            Debug.Log("Start game.");
            EnterState();
        }else
        {
            Debug.Log("Game is already started.");

        }
    }

    private void EnterState()
    {
        Debug.Log("Enter Game Start State");
        // Make configuration to start game
        Configure();
        // Exit state.
        ExitState();
    }

    private void ExitState()
    {
        Debug.Log("Exit Game Start State");
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void Configure()
    {
        isGameStarted = true;
        InputManager.Instance.isFirstInput = true;
        UserInterfaceManager.Instance.HideGameOverScreen();
        ScoreManager.ResetScore();
        RepositionManager.Instance.SetSpeed(10);
    }


    #endregion
}
