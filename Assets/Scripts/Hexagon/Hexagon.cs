using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Hexagon : MonoBehaviour
{
    #region Properties
    // Hexagon properties
    public static readonly float longRadius = 1;
    public static readonly float shortRadius = 0.866f;
    public static readonly float margin = 0.1f;

    // Defines bomb hexagon or not
    public bool isBombHexagon { get; private set; }

    // Number of actions left before explosion.
    private int hexagonBombCounter = 6;
    public int HexagonBombCounter { get {return hexagonBombCounter; } }

    // Pointer to hexagon frame
    private GameObject hexagonFrame = null;

    // Pointer to hexagon bomb
    private GameObject hexagonBomb = null;

    // Pointer to hexagon bomb counter text
    private GameObject hexagonBombCounterText = null;

    #endregion

    #region Functions

    private void Awake()
    {
        AssignRandomMaterial();
    }

    //  Assigns random material based on random number generation
    private void AssignRandomMaterial()
    {
        int randomNumber = Random.Range(0, 5);
        GetComponent<Renderer>().material = HexagonProperties.Instance.Materials[randomNumber];
    }

    // Sets this hexagon as bomb hexagon
    public void SetAsBombHexagon()
    {
        //Debug.Log("Setting as bomb hexagon.");
        isBombHexagon = true;

        // Spawn bomb prefab
        Vector3 positionToSpawn = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.01f);
        hexagonBomb = Instantiate(HexagonProperties.Instance.HexagonBombPrefab, positionToSpawn, Quaternion.identity);
        hexagonBomb.transform.SetParent(transform);

        // Spawn bomb counter prefab
        positionToSpawn = new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.02f);
        hexagonBombCounterText = Instantiate(HexagonProperties.Instance.HexagonBombCounterPrefab, positionToSpawn, Quaternion.identity);
        hexagonBombCounterText.transform.SetParent(transform);
        hexagonBombCounterText.GetComponentInChildren<TextMeshProUGUI>().text = hexagonBombCounter.ToString();
    }

    public void DecreaseBombCounter()
    {
        if (isBombHexagon)
        {
            hexagonBombCounter--;
            hexagonBombCounterText.GetComponentInChildren<TextMeshProUGUI>().text = hexagonBombCounter.ToString();
        }
    }

    // Checks if this hexagon must be exploded. If yes return gameObject, otherwise null.
    public GameObject CheckExplosion()
    {
        List<GameObject> neighbours = GridManager.GetNeighbours(gameObject);
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i] == null)
                continue;

            // check if neighbour has same color. if yes check next neighbour.
            if (GetComponent<Renderer>().material.color == neighbours[i].GetComponent<Renderer>().material.color)
            {
                // Index for next neighbour. Just a little modification to avoid out of index.
                int nextIndex = (i + 1) > 5 ? 0 : (i + 1);

                // continue if next neighbour is null. This mean this hexagon is on  the edge.
                if (neighbours[nextIndex] == null)
                    continue;

                // return gameObject if this neighbour has same color too.
                if (GetComponent<Renderer>().material.color == neighbours[nextIndex].GetComponent<Renderer>().material.color)
                    return gameObject;
            }
        }
        return null;
    }

    // Spawns hexagon frame. Called when this object is selected
    public void SpawnFrame()
    {
        hexagonFrame = Instantiate(HexagonProperties.Instance.HexagonFramePrefab, transform.position, Quaternion.identity);
        hexagonFrame.transform.SetParent(transform);
    }

    // Destorys hexagon frame. Called when this object is deselected
    public void DestroyFrame()
    {
        Destroy(hexagonFrame);
    }

    public Vector3 MoveTowards(Vector3 origin, Vector3 target, float maxDistanceDelta)
    {
        Vector3 positionToSet = Vector3.MoveTowards(origin, target, Time.deltaTime);

        // Set hexagon game object position.
        transform.position = positionToSet;

        // Return position set.
        return positionToSet;
    }

    private void OnDestroy()
    {
        //Debug.Log("Hexagon on destroy!");
        // Increase score on destroy if scoring is not stopped.
        if(!ScoreManager.IsScoreStopped)
            ScoreManager.IncreaseScore();
    }
    #endregion
}

