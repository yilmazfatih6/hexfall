using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RotationManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static RotationManager Instance { get; private set; }
    // Publisher for state exit
    public static event EventHandler<OnStateExitEventArgs> OnStateExit;
    // Event arguments to send on ExitState.
    public class OnStateExitEventArgs : EventArgs
    {
        public bool isOver;
    }
    // Holds target locations for current step of rotation.
    private List<Vector3> TargetLocations = new List<Vector3>();
    // Counter for how many times rotated.
    private int counter = 0;
    // Checker for if rotation is over
    public bool isOver = true;
    // cheker for this steep over
    private bool isStepOver = true;
    // defines if clockwise or not.
    private bool isClockwise = false;
    // Rotation speed
    private float rotationSpeed = 2.5f;

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
        InputManager.OnStateExit += InputManager_OnStateExit;
        ExplosionManager.OnStateExit += ExplosionManager_OnStateExit;
    }

    private void Update()
    {
        if (!isStepOver)
        {
            Rotate();
            Debug.Log("Rotate update");
        }
    }

    private void InputManager_OnStateExit(object sender, InputManager.OnStateExitEventArgs e)
    {
        isClockwise = e.isClockwise;
        EnterState();
    }

    private void ExplosionManager_OnStateExit(object sender, ExplosionManager.OnStateExitEventArgs e)
    {
        // Enter state if any hexagon exploded in the previous state.
        if (e.stateToGo == States.Rotation)
            ContinueState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Rotation State");
        // Reset all variables
        isOver = false;
        isStepOver = false;
        counter = 0;
        TargetLocations.Clear();
    }

    // Continue to this state
    private void ContinueState()
    {
        Debug.Log("Continue Rotation State");
        // Reset only these variables
        isStepOver = false;
        TargetLocations.Clear();
    }

    // Exit this state
    private void ExitState()
    {
        Debug.Log("Exit Rotation State");
        isStepOver = true;
        counter++;
        if (counter == 3)
            isOver = true;
        OnStateExit?.Invoke(this, new OnStateExitEventArgs { isOver = isOver });
    }

    public void Rotate()
    {
        // If none hexagon is selected return.
        List<GameObject> hexagons = SelectionManager.Instance.SelectedHexagons;
        if (hexagons == null || hexagons.Count == 0)
            return;

        AssignTargetLocations();

        // Rotate objects
        if (isClockwise)
        {
            if (hexagons[0].transform.position != TargetLocations[1] && hexagons[1].transform.position != TargetLocations[2] && hexagons[2].transform.position != TargetLocations[0])
            {
                hexagons[0].transform.position = Vector3.MoveTowards(hexagons[0].transform.position, TargetLocations[1], Time.deltaTime * rotationSpeed);
                hexagons[1].transform.position = Vector3.MoveTowards(hexagons[1].transform.position, TargetLocations[2], Time.deltaTime * rotationSpeed);
                hexagons[2].transform.position = Vector3.MoveTowards(hexagons[2].transform.position, TargetLocations[0], Time.deltaTime * rotationSpeed);
            }
            else
                ExitState();
        }
        else
        {
            if (hexagons[0].transform.position != TargetLocations[2] && hexagons[1].transform.position != TargetLocations[0] && hexagons[2].transform.position != TargetLocations[1])
            {
                hexagons[0].transform.position = Vector3.MoveTowards(hexagons[0].transform.position, TargetLocations[2], Time.deltaTime * rotationSpeed);
                hexagons[1].transform.position = Vector3.MoveTowards(hexagons[1].transform.position, TargetLocations[0], Time.deltaTime * rotationSpeed);
                hexagons[2].transform.position = Vector3.MoveTowards(hexagons[2].transform.position, TargetLocations[1], Time.deltaTime * rotationSpeed);
            }
            else
                ExitState();
        }

    }

    private void AssignTargetLocations()
    {
        if (TargetLocations.Count != 0)
            return;

        // If none hexagon is selected return.
        List<GameObject> hexagons = SelectionManager.Instance.SelectedHexagons;
        if (hexagons == null || hexagons.Count == 0)
            UnexpectedExit();

        // Assign selected hexagon's current locations to target locations
        foreach (var selectedHexagon in SelectionManager.Instance.SelectedHexagons)
        {
            if (selectedHexagon == null)
                UnexpectedExit();
            TargetLocations.Add(selectedHexagon.transform.position);
        }
    }

    private void UnexpectedExit()
    {
        Debug.Log("Unexpected exit!");
        isOver = true;
        ExitState();
    }
    #endregion
}
