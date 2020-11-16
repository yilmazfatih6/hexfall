using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static InputManager Instance { get; private set; }

    // Publisher for state exit
    public static event EventHandler<OnStateExitEventArgs> OnStateExit;
    // Event arguments to send on ExitState.
    public class OnStateExitEventArgs : EventArgs
    {
        public bool isClockwise;
    }
    private bool isStateActive = false;
    public bool isFirstInput = true;
    private Vector3 initialTouchPosition;
    private Vector3 finalTouchPosition;
    private bool isSelectedHexagon = false;
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
        ExplosionManager.OnStateExit += InputManager_OnStateExit;
    }

    private void InputManager_OnStateExit(object sender, ExplosionManager.OnStateExitEventArgs e)
    {
        // Enter state if any hexagon exploded in the previous state.
        if (e.stateToGo == States.Input)
            EnterState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Input State");
        isStateActive = true;

        // Some configs to make before user makes first input.
        ConfigureForFirstInput();
    }

    // Exit this state
    private void ExitState(bool isClockwise)
    {
        Debug.Log("Exit Input State");
        isStateActive = false;
        OnStateExit?.Invoke(this, new OnStateExitEventArgs { isClockwise = isClockwise });
    }

    // Update is called once per frame
    private void Update()
    {
        // Handles left click.
        if (isStateActive)
        {
            OnTouch();
        }
    }

    /* Check if left clicked, if not return
     * Check if hit a hexagon, if not return 
     * Check if current game state is WaitingInput, if not return 
     * Check if hit hexagon is selected currently, if not call selection. If selected call rotation
     */
    

    private void OnTouch()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            // Return if left mouse button is not up
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Began))
            {
                // Get mouse poition in 3D world
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Initial position
                initialTouchPosition = position;

                if (position == null)
                    return;

                // Ray cast 2D 
                RaycastHit2D hitInfo = Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.zero, 0f);

                // Check if hit Hexagon. If not return. 
                if (hitInfo.collider == null || hitInfo.transform.gameObject.tag != "Hexagon")
                    return;

                // If selected hit object is selected before. If not make selection.
                isSelectedHexagon = SelectionManager.Instance.SelectedHexagons.Contains(hitInfo.transform.gameObject);
                if (!isSelectedHexagon)
                    SelectionManager.Instance.MakeSelection(hitInfo.transform.gameObject, hitInfo);

            }

            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended))
            {
                finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (isSelectedHexagon)
                {
                    isSelectedHexagon = false;
                    ExitState(IsClockWise());
                }
            }
        }
    }

    // Check if rotation should be clock wise. 
    private bool IsClockWise()
    {
        Vector3 setInitialToOrigin = (initialTouchPosition - SelectionManager.Instance.GetSelectionCenter());
        Vector3 setFinalToOrigin = (finalTouchPosition - SelectionManager.Instance.GetSelectionCenter());
        int initialRegion = PositionToCoorRegion(setInitialToOrigin);
        int finalRegion = PositionToCoorRegion(setFinalToOrigin);
        bool isClockwise = true;
        if (initialRegion > finalRegion)
            isClockwise = false;
        if (initialRegion == 1 && finalRegion == 4)
            isClockwise = false;
        if (initialRegion == 4 && finalRegion == 1)
            isClockwise = false;
        return isClockwise;
    }

    // Return coordinate region for given position
    private int PositionToCoorRegion(Vector3 setInitialToOrigin)
    {
        if (setInitialToOrigin.x >= 0 && setInitialToOrigin.y >= 0)
            return 1;
        if (setInitialToOrigin.x < 0 && setInitialToOrigin.y >= 0)
            return 4;
        if (setInitialToOrigin.x < 0 && setInitialToOrigin.y < 0)
            return 3;
        if (setInitialToOrigin.x >= 0 && setInitialToOrigin.y < 0)
            return 2;
        Debug.Log("Error on PositionToCoorRegion");
        return 0;
    }

    

    // Some configs to make before user makes first input.
    public void ConfigureForFirstInput()
    {
        if (isFirstInput)
        {
            Debug.Log("Making first input configuration");
            UserInterfaceManager.Instance.DisplayInGameUI();
            ScoreManager.StartScore();
            RepositionManager.Instance.SetSpeed(2.5f);
            isFirstInput = false;
        }

    }
    #endregion
}
