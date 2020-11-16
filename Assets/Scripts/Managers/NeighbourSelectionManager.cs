using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighbourSelectionManager : MonoBehaviour
{
    #region Properties
    List<Vector2> neighbourPositions = new List<Vector2>();
    #endregion

    // Singleton pointer
    public static NeighbourSelectionManager Instance { get; private set; }

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void SelectNeighbours(GameObject hexagon)
    {
        // Convert hexagon gameobject position to Vector2
        Vector2 selectedHexagonPosition = new Vector2(hexagon.transform.position.x, hexagon.transform.position.y);
        bool repeat = true;
        while (repeat)
        {
            List<GameObject> selectedNeighbours = new List<GameObject>();
            // Calculate which neighbours to select and assign them to neighbourPositions.
            this.AssignNeighboursToSelect();
            foreach (Vector2 neighbourPosition in neighbourPositions)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(selectedHexagonPosition + neighbourPosition, Vector2.down, 0f);
                if (hitInfo && hitInfo.transform.gameObject.tag == "Hexagon")
                {
                    selectedNeighbours.Add(hitInfo.transform.gameObject);
                }
            }

            if (selectedNeighbours.Count < 2)
            {
                SelectionHitAreaManager.Instance.SwitchToNextArea();
                selectedNeighbours.Clear();
            }
            else
            {
                foreach (var selectedNeighbour in selectedNeighbours)
                    SelectionManager.Instance.Select(selectedNeighbour);
                repeat = false;
            }

        }
    }

    private void AssignNeighboursToSelect()
    {
        neighbourPositions.Clear();
        if (SelectionHitAreaManager.Instance.HitArea == 1)
        {
            neighbourPositions.Add(NeighbourOffsets.TopRight);
            neighbourPositions.Add(NeighbourOffsets.BottomRight);
        }
        if (SelectionHitAreaManager.Instance.HitArea == 2)
        {
            neighbourPositions.Add(NeighbourOffsets.Top);
            neighbourPositions.Add(NeighbourOffsets.TopRight);
        }
        if (SelectionHitAreaManager.Instance.HitArea == 3)
        {
            neighbourPositions.Add(NeighbourOffsets.TopLeft);
            neighbourPositions.Add(NeighbourOffsets.Top);
        }
        if (SelectionHitAreaManager.Instance.HitArea == 4)
        {
            neighbourPositions.Add(NeighbourOffsets.BottomLeft);
            neighbourPositions.Add(NeighbourOffsets.TopLeft);
        }
        if (SelectionHitAreaManager.Instance.HitArea == 5)
        {
            neighbourPositions.Add(NeighbourOffsets.Bottom);
            neighbourPositions.Add(NeighbourOffsets.BottomLeft);
        }
        if (SelectionHitAreaManager.Instance.HitArea == 6)
        {
            neighbourPositions.Add(NeighbourOffsets.BottomRight);
            neighbourPositions.Add(NeighbourOffsets.Bottom);
        }
    }

}
