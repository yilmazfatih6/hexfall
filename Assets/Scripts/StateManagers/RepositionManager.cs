using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositionManager : MonoBehaviour
{
    #region Properties
    // Singleton pointer
    public static RepositionManager Instance { get; private set; }

    // Publisher for state exit
    public static event EventHandler OnStateExit;

    // Movement speed for repositioning. 
    private float repositionSpeed = 10f;
    public float RepositionSpeed { get { return repositionSpeed; } }

    // Map for target position. Key is hexagon, value is to target position
    Dictionary<GameObject, Vector3> targetPositions = new Dictionary<GameObject, Vector3>();

    // Checker for whether repositioning is over or not.
    bool isStateOver = true;
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
        ExplosionManager.OnStateExit += ExplosionManager_OnStateExit;
    }

    private void Update()
    {
        if (!isStateOver)
            Reposition();
    }

    private void ExplosionManager_OnStateExit(object sender, ExplosionManager.OnStateExitEventArgs e)
    {
        // Enter state if any hexagon exploded in the previous state.
        if (e.stateToGo == States.Reposition)
            EnterState();
    }

    // Enter to this state
    private void EnterState()
    {
        Debug.Log("Enter Reposition State");

        // Reset variables
        PrepareState();
    }

    // Exit this state
    private void ExitState()
    {
        Debug.Log("Exit Reposition State");
        // Set checkers to false
        isStateOver = true;
        // Clear target positions
        targetPositions.Clear();
        // Publish exit event
        OnStateExit?.Invoke(this, EventArgs.Empty);
    }

    private void PrepareState()
    {
        // Set checkers to false
        isStateOver = false;
        // Clear target positions
        targetPositions.Clear();
    }

    private void Reposition()
    {
        /* Check if target positions are set. 
        * If not assign target positions.
        */
        if (targetPositions.Count == 0)
            AssignTargetPositions();
        else
            Move();

        // Check if repositiong over. If yes exit current state.
        if (isStateOver)
            ExitState();
    }

    private void Move()
    {
        bool isRepositioned = false;
        foreach (var x in targetPositions)
        {
            if (x.Key.transform.position != x.Value)
            {
                x.Key.transform.position = Vector3.MoveTowards(x.Key.transform.position, x.Value, Time.deltaTime * repositionSpeed);
                isRepositioned = true;
            }
        }

        if (isRepositioned == false)
            isStateOver = true;
    }

    void AssignTargetPositions()
    {
        //isRepositioned = true;
        for (int x = 0; x < GridManager.GridPositions2D.Count; x++)
        {
            for (int y = 0; y < GridManager.GridPositions2D[x].Count - 1; y++)
            {
                if (GridManager.GetHexagon(GridManager.GridPositions2D[x][y]) == null)
                {
                    int index = y + 1;
                    while (index < GridManager.GridPositions2D[x].Count)
                    {
                        GameObject topHexagon = GridManager.GetHexagon(GridManager.GridPositions2D[x][index]);
                        if (topHexagon != null)
                        {
                            Vector3 targetPosition =
                                topHexagon.transform.position + new Vector3(NeighbourOffsets.Bottom.x, NeighbourOffsets.Bottom.y, topHexagon.transform.position.z);
                            if (targetPositions.ContainsKey(topHexagon))
                                targetPositions[topHexagon] += new Vector3(NeighbourOffsets.Bottom.x, NeighbourOffsets.Bottom.y, topHexagon.transform.position.z);
                            else
                                targetPositions.Add(topHexagon, targetPosition);
                        }
                        index++;
                    }
                }

            }
        }

        // Change state to Respawn if no target found to move.
        // This is to avoid state lock, when there is no hexagon above of the destroyed hexagon. 
        if (targetPositions.Count == 0)
        {
            Debug.Log("no target");
            ExitState();
        }
    }

    public void SetSpeed(float newSpeed)
    {
        Debug.Log("Setting new reposition speed");
        repositionSpeed = newSpeed;
    }
    #endregion

}
