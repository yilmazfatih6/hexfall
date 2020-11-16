using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static ExplosionManager Instance { get; private set; }

    // Publisher for state exit
    public static event EventHandler<OnStateExitEventArgs> OnStateExit;
    // Event arguments to send on ExitState.
    public class OnStateExitEventArgs : EventArgs
    {
        public States stateToGo;
    }

    // Checker for if anythin exploded. Use to decide next state.
    private bool isAnyExploded = false;
    public bool IsAnyExploded { get { return isAnyExploded; } }
    private bool isCheckingInput = false;
    public bool IsCheckingInput { get { return isCheckingInput; } }
    #endregion

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #region Functions

    private void Start()
    {
        // Substribe to other event publishers
        SpawnManager.OnStateExit += SpawnManager_OnStateExit;
        GameOverManager.OnStateExit += GameOverManager_OnStateExit;
        RotationManager.OnStateExit += RotationManager_OnStateExit;
    }

   
    // Run on Spawn State Exit 
    private void SpawnManager_OnStateExit(object sender, EventArgs e)
    {
        isCheckingInput = false;
        EnterState();
    }

    // Run on Game Over State Exit . After respawn.
    private void GameOverManager_OnStateExit(object sender, EventArgs e)
    {
        isCheckingInput = false;
        EnterState();
    }

    private void RotationManager_OnStateExit(object sender, RotationManager.OnStateExitEventArgs e)
    {
        isCheckingInput = true;
        EnterState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Explosion State");

        Explode();
    }

    // Exit this state
    private void ExitState()
    {
        Debug.Log("Exit Explosion State");
        // Set which state to go.
        States stateToGo;
        if (isAnyExploded)
            stateToGo = States.Reposition;
        else
        {
            if (RotationManager.Instance.isOver)
                stateToGo = States.Input;
            else
                stateToGo = States.Rotation;
        }
        // Publish state exit
        OnStateExit?.Invoke(this, new OnStateExitEventArgs { stateToGo = stateToGo } );
    }

    public void Explode()
    {
        isAnyExploded = false;
        // Explode hexagons
        foreach (GameObject hexagon in HexagonsToExplode())
        {
            if(hexagon != null)
            {
                Destroy(hexagon);
                isAnyExploded = true; // Set checker to true
            }
        }

        
        // Set rotation as over if any exploded. And remove frames.
        if (isAnyExploded)
        {
            RotationManager.Instance.isOver = true;

            // Remove frames
            foreach (GameObject hexagon in SelectionManager.Instance.SelectedHexagons)
            {
                if (hexagon)
                    hexagon.GetComponent<Hexagon>().DestroyFrame();
            }
        }

        ExitState();
    }

    public List<GameObject> HexagonsToExplode()
    {
        List<GameObject> hexagonsToExplode = new List<GameObject>();

        // Get hexagons to explode
        foreach (GameObject hexagon in GridManager.Hexagons)
            hexagonsToExplode.Add(hexagon.GetComponent<Hexagon>().CheckExplosion());

        return hexagonsToExplode;
    }
    #endregion
}
