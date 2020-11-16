using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    #region Singleton Pointer
    public static GridManager Instance { get; private set; }
    #endregion

    private void Awake()
    {
        // Set this class as Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Returns a list containing list of hexagons in the game.
    public static List<GameObject> Hexagons
    {
        get
        {
            // List to return
            List<GameObject> hexagons = new List<GameObject>();

            // Run loop for all grid tile positions
            foreach (Vector3 position in GridPositions)
            {
                // Ray cast to position.
                RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.zero);
                // Add to list if any hexagon it hit.
                if (raycastHit2D.collider != null && raycastHit2D.transform.gameObject.tag == "Hexagon")
                    hexagons.Add(raycastHit2D.transform.gameObject);
            }

            return hexagons;
        }
    }

    // Return a list of grid tile coordinates
    public static List<Vector3> GridPositions
    {
        get
        {
            // List to return
            List<Vector3> positions = new List<Vector3>();

            // Assign properties to variables for code readablitiy.
            Vector2 gridSize = GridProperties.Instance.GridSize;
            Vector2 offset = new Vector2(NeighbourOffsets.TopRight.x, NeighbourOffsets.Top.y);

            // Run loop for x axis. add %99 percent tolerance.
            for (float x = 0f; x < (gridSize.x * offset.x) * 0.99f; x += offset.x)
            {
                // Run loop for y axis
                for (int y = 0; y < gridSize.y; y++)
                {
                    // Convert it to Vector3
                    Vector3 spawnPosition = new Vector3(x, (y * offset.y), 0);
                    // Swipe y coordinate down to have zig zag shape.
                    spawnPosition.y = ((Mathf.Round(x / offset.x) % 2) == 0) ? spawnPosition.y : spawnPosition.y - offset.y / 2;
                    // Add position to list.
                    positions.Add(spawnPosition);
                }
            }

            return positions;
        }
    }

    // Returns 2x2 Vector3. Each representing grid positions on the game.
    public static List<List<Vector3>> GridPositions2D
    {
        get
        {
            // Assign properties to variables for code readablitiy.
            Vector2 gridSize = GridProperties.Instance.GridSize;
            Vector2 offset = new Vector2(NeighbourOffsets.TopRight.x, NeighbourOffsets.Top.y);

            // List to return
            List<List<Vector3>> positions = new List<List<Vector3>>();

            // Run loop for x axis. add %99 percent tolerance.
            for (float x = 0f; x < (gridSize.x * offset.x) * 0.99f; x += offset.x)
            {
                List<Vector3> columnList = new List<Vector3>();

                // Run loop for y axis
                for (int y = 0; y < gridSize.y; y++)
                {
                    // Convert it to Vector3
                    Vector3 spawnPosition = new Vector3(x, (y * offset.y), 0);
                    // Swipe y coordinate down to have zig zag shape.
                    spawnPosition.y = ((Mathf.Round(x / offset.x) % 2) == 0) ? spawnPosition.y : spawnPosition.y - offset.y / 2;
                    // Add position to column list.
                    columnList.Add(spawnPosition);
                }
                // Add position to list.
                positions.Add(columnList);
            }

            return positions;
        }
    }

    // Returns 2x2 Gameobjects. Each representing hexagons on the game.
    public static List<List<GameObject>> Hexagons2D
    {
        get
        {
            // List to return
            List<List<GameObject>> hexagons = new List<List<GameObject>>();

            // Run loop for all grid tile positions
            foreach (List<Vector3> column in GridPositions2D)
            {
                List<GameObject> hexagonsOnThisColumn = new List<GameObject>();
                foreach (Vector3 position in column)
                {
                    // Ray cast to position.
                    RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.zero);
                    // Add to list if any hexagon it hit.
                    if (raycastHit2D.collider != null && raycastHit2D.transform.gameObject.tag == "Hexagon")
                        hexagonsOnThisColumn.Add(raycastHit2D.transform.gameObject);
                    else
                        hexagonsOnThisColumn.Add(null);
                }
                hexagons.Add(hexagonsOnThisColumn);
            }

            return hexagons;
        }
    }

    // Returns all bomb hexagons in the game.
    public static List<GameObject> BombHexagons
    {
        get
        {
            List<GameObject> bombHexagons = new List<GameObject>();
            foreach (GameObject hexagon in Hexagons)
            {
                if (hexagon.GetComponent<Hexagon>().isBombHexagon)
                    bombHexagons.Add(hexagon);

            }
            return bombHexagons;
        }
    }


    // Gets hexagon from given position. If there is no hexagon at given position returns null
    public static GameObject GetHexagon(Vector3 position)
    {
        RaycastHit2D raycastHit2D = Physics2D.Raycast(position, Vector2.zero);
        if (raycastHit2D.collider != null && raycastHit2D.transform.gameObject.tag == "Hexagon")
            return raycastHit2D.transform.gameObject;
        else
            return null;
    }

    // Get all neighbours for given hexagon.
    public static List<GameObject> GetNeighbours(GameObject hexagon)
    {
        List<GameObject> neighbours = new List<GameObject>();
        foreach (Vector2 offset in NeighbourOffsets.ToList())
        {
            Vector3 offset3 = new Vector3(offset.x, offset.y, hexagon.transform.position.z);
            GameObject neighbour = GridManager.GetHexagon(hexagon.transform.position + offset3);
            neighbours.Add(neighbour);
        }
        return neighbours;
    }
}
