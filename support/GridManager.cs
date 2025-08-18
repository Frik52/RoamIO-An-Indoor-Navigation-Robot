using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public float nodeRadius = 0.05319f; 
    public LayerMask unwalkableMask;

    [Header("Gizmos")]
    public bool drawGridGizmos = true;

    private Node[,] grid;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;
    private MeshRenderer meshRenderer;
    private Vector3 gridOrigin;
    float buffer = 0.8f;

    [HideInInspector] public Vector2 gridWorldSize;
    [HideInInspector] public List<Node> debugPath;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        nodeDiameter = nodeRadius * 2f;

        InitializeGrid();
    }

    private void InitializeGrid()
    {
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null)
        {
            Debug.LogError("GridManager requires a MeshFilter!");
            return;
        }

        Vector3 meshSize = mf.sharedMesh.bounds.size;
        Vector3 scale = transform.lossyScale;

        gridWorldSize = new Vector2(meshSize.x * scale.x, meshSize.z * scale.z);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        gridOrigin = transform.position - new Vector3(gridWorldSize.x / 2f, 0, gridWorldSize.y / 2f);

        Debug.Log($"Grid initialized: {gridSizeX} x {gridSizeY}");
        CreateGrid();
    }

    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = gridOrigin + Vector3.right * (x * nodeDiameter + nodeRadius)
                                                   + Vector3.forward * (y * nodeDiameter + nodeRadius);

                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius * 2.5f, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);

                if (!walkable)
                    Debug.DrawRay(worldPoint + Vector3.up * 0.1f, Vector3.up, Color.red, 5f);
            }
        }
    }

    /// <summary>
    /// Scans all nodes and updates their walkable status based on obstacles.
    /// Call this before recalculating paths.
    /// </summary>
    public void ScanForObstacles(LayerMask obstacleMask)
    {
        if (grid == null) return;

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = grid[x, y].worldPosition;
                bool blocked = Physics.CheckSphere(worldPoint, nodeRadius + buffer, obstacleMask);
                grid[x, y].walkable = !blocked;
            }
        }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - gridOrigin.x) / gridWorldSize.x;
        float percentY = (worldPosition.z - gridOrigin.z) / gridWorldSize.y;

        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.Clamp(Mathf.RoundToInt((gridSizeX - 1) * percentX), 0, gridSizeX - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt((gridSizeY - 1) * percentY), 0, gridSizeY - 1);

        return grid[x, y];
    }

    public List<Node> GetNeighbours(Node node, bool allowDiagonals = true)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                if (!allowDiagonals && Mathf.Abs(x) + Mathf.Abs(y) > 1) continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    neighbours.Add(grid[checkX, checkY]);
            }
        }

        return neighbours;
    }

    void OnDrawGizmos()
    {
        if (!drawGridGizmos || meshRenderer == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if (grid == null) return;

        foreach (Node n in grid)
        {
            Gizmos.color = n.walkable ? Color.white : Color.red;

            if (debugPath != null && debugPath.Contains(n))
                Gizmos.color = Color.black;

            Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.01f));
        }
    }

    public int MaxSize => gridSizeX * gridSizeY;
}
