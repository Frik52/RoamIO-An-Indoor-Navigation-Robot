using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GridManager grid;

    void Awake()
    {
        if (!grid) grid = GetComponent<GridManager>();
    }

    /// <summary>
    /// Finds a path from startPos to targetPos using A*.
    /// Ensures no path goes through obstacles.
    /// </summary>
    public List<Vector3> FindPath(Vector3 startPos, Vector3 targetPos, bool allowDiagonals = true)
{
    if (grid == null)
    {
        Debug.LogError("GridManager not assigned to Pathfinding!");
        return null;
    }

    // 🔄 Always refresh obstacles before computing
    grid.ScanForObstacles(grid.unwalkableMask);

    Node startNode = grid.NodeFromWorldPoint(startPos);
    Node targetNode = grid.NodeFromWorldPoint(targetPos);

    if (startNode == null || targetNode == null) return null;
    if (!startNode.walkable || !targetNode.walkable) return null;

    List<Node> openSet = new List<Node> { startNode };
    HashSet<Node> closedSet = new HashSet<Node>();

    startNode.gCost = 0;
    startNode.hCost = Heuristic(startNode, targetNode);
    startNode.parent = null;

    while (openSet.Count > 0)
    {
        Node current = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].fCost < current.fCost ||
                (openSet[i].fCost == current.fCost && openSet[i].hCost < current.hCost) ||
                (openSet[i].fCost == current.fCost && openSet[i].hCost == current.hCost && Random.value > 0.5f))
            {
                current = openSet[i];
            }
        }

        openSet.Remove(current);
        closedSet.Add(current);

        if (current == targetNode)
        {
            List<Node> nodePath = RetracePath(startNode, targetNode);
            grid.debugPath = nodePath;
            return ToWorldPoints(nodePath);
        }

        foreach (Node neighbour in grid.GetNeighbours(current, allowDiagonals))
        {
            if (!neighbour.walkable || closedSet.Contains(neighbour))
                continue;

            int moveCost = (neighbour.gridX != current.gridX && neighbour.gridY != current.gridY) ? 14 : 10;
            int tentativeG = current.gCost + moveCost;

            if (!openSet.Contains(neighbour) || tentativeG < neighbour.gCost)
            {
                neighbour.gCost = tentativeG;
                neighbour.hCost = Heuristic(neighbour, targetNode);
                neighbour.parent = current;

                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
            }
        }
    }

    // ❌ No path found
    grid.debugPath = null;
    return null;
}


    int Heuristic(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);
        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);
        return 14 * min + 10 * (max - min);
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node current = endNode;

        while (current != startNode)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Add(startNode);
        path.Reverse();
        return path;
    }
   


    List<Vector3> ToWorldPoints(List<Node> path)
    {
        List<Vector3> pts = new List<Vector3>();
        foreach (Node n in path) pts.Add(n.worldPosition);
        return pts;
    }
}
