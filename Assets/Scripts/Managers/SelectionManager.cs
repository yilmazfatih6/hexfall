using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    #region Singleton
    // Singleton pointer
    public static SelectionManager Instance { get; private set; }

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    #endregion

    [SerializeField]
    private List<GameObject> selectedHexagons = new List<GameObject>();
    public List<GameObject> SelectedHexagons { get { return selectedHexagons; } }

    public void Select(GameObject hexagon)
    {
        // Assign new selected hexagon to list
        selectedHexagons.Add(hexagon);
        hexagon.GetComponent<Hexagon>().SpawnFrame();
    }

    public void MakeSelection(GameObject hexagon, RaycastHit2D hitInfo)
    {
        // Clear previous selection
        this.ClearSelectedHexagons();
        // Detect input hit area
        SelectionHitAreaManager.Instance.DetectHitArea(hexagon, hitInfo);
        // Select given hexagon
        this.Select(hexagon);
        // Check and find neighbours
        NeighbourSelectionManager.Instance.SelectNeighbours(hexagon);
    }

    public void ClearSelectedHexagons()
    {
        foreach (var selectedHexagon in selectedHexagons)
        {
            if (selectedHexagon != null)
                selectedHexagon.GetComponent<Hexagon>().DestroyFrame();

        }
        selectedHexagons.Clear();
    }

    // Get center of selected hexagons.
    public Vector3 GetSelectionCenter()
    {
        if (selectedHexagons == null)
        {
            Debug.Log("Selected Hexagons are empty! Returning zero vector.");
            return Vector3.zero;
        }

        bool isOnLeft = true;
        foreach(GameObject hexagon in selectedHexagons)
        {
            Debug.Log("Top hexagon" + TopHexagon().transform.position.x);
            if (hexagon != null)
            {
                if (TopHexagon().transform.position.x > hexagon.transform.position.x)
                    isOnLeft = false;
            }
           
        }

        float y = TopHexagon().transform.position.y + NeighbourOffsets.Bottom.y / 2;

        float offsetX = (isOnLeft) ? NeighbourOffsets.TopRight.x / 2 : NeighbourOffsets.TopLeft.x / 2;
        float x = TopHexagon().transform.position.x + offsetX;

        return new Vector3(x,y,0);
    }

    // Return height hexagon (top hexagon) from selected hexagons.
    public GameObject TopHexagon()
    {
        GameObject topHexagon = null;
        float yAxis = -10;
        foreach(GameObject hexagon in selectedHexagons)
        {
            if (hexagon == null) return null;
            if (hexagon.transform.position.y > yAxis)
            {
                topHexagon = hexagon; 
                yAxis = topHexagon.transform.position.y;
            }

        }
        return topHexagon;
    }
}
