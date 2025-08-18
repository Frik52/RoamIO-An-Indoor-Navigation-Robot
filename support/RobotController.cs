using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LineRenderer))]
public class RobotController : MonoBehaviour
{
    public float speed = 3f;
    public float waypointTolerance = 0.2f;

    [Header("References")]
    public Pathfinding pathfinding; // assign in inspector
    public GridManager grid;        // assign in inspector
    public LayerMask obstacleMask;

    private Queue<Vector3> pathQueue = new Queue<Vector3>();
    private Vector3? currentTarget = null;
    private Rigidbody rb;
    private LineRenderer lineRenderer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.positionCount = 0;
    }

    void FixedUpdate()
    {
        if (currentTarget.HasValue)
        {
            MoveAlongPath();
        }
    }

    public void SetTargetPath(Vector3 destination)
    {
        if (grid == null || pathfinding == null)
        {
            Debug.LogWarning("GridManager or Pathfinding not assigned!");
            return;
        }

        // Update obstacle info
        grid.ScanForObstacles(obstacleMask);

        // Calculate path
        List<Vector3> newPath = pathfinding.FindPath(rb.position, destination);

        if (newPath == null || newPath.Count == 0)
        {
            Debug.LogWarning("⚠ No valid path to destination!");
            return;
        }

        // Enqueue path
        pathQueue.Clear();
        foreach (var pt in newPath) pathQueue.Enqueue(pt);
        currentTarget = pathQueue.Dequeue();

        // Draw line
        lineRenderer.positionCount = newPath.Count;
        for (int i = 0; i < newPath.Count; i++)
            lineRenderer.SetPosition(i, newPath[i] + Vector3.up * 0.05f);

        Debug.Log($"📍 Path set with {newPath.Count} waypoints.");
    }
    public bool IsMoving()
{
    return currentTarget.HasValue || pathQueue.Count > 0;
}

    private void MoveAlongPath()
    {
        if (!currentTarget.HasValue) return;

        Vector3 target = currentTarget.Value;
        Vector3 flatTarget = new Vector3(target.x, rb.position.y, target.z);
        Vector3 direction = (flatTarget - rb.position).normalized;

        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);

        if (Vector3.Distance(rb.position, flatTarget) <= waypointTolerance)
        {
            if (pathQueue.Count > 0)
                currentTarget = pathQueue.Dequeue();
            else
            {
                currentTarget = null;
                lineRenderer.positionCount = 0; // clear path
                Debug.Log("🏁 Destination reached.");
            }
        }
    }
}
