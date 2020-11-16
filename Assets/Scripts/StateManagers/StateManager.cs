using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States
{
    InitialSpawn,
    Input,
    WaitingRotationInput,
    Explosion,
    Rotation,
    SwipingDown,
    Reposition,
    Respawn,
    ExplosionDuringRotate,
    ExplosionDuringRespawn,
    GameOver,
    GameStart,
    CheckingIfGameOver,
}

public class StateManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer.
    public static StateManager Instance { get; private set; }
    // Holds current state.
    public static States State = States.GameStart;
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
