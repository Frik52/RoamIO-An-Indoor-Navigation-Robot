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

    // 🔹 Stuck detection
    private Vector3 lastPosition;
    private float stuckTimer = 0f;
    public float stuckThreshold = 0.05f; // distance threshold
    public float stuckTimeLimit = 1f;    // seconds before recalculating

    private Vector3? goalDestination = null; // remember final goal

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

        lastPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (currentTarget.HasValue)
        {
            MoveAlongPath();
            CheckStuck();
        }
    }

    public void SetTargetPath(Vector3 destination)
    {
        if (grid == null || pathfinding == null)
        {
            Debug.LogWarning("GridManager or Pathfinding not assigned!");
            return;
        }

        // Remember final goal
        goalDestination = destination;

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
                goalDestination = null;
                lineRenderer.positionCount = 0; // clear path
                Debug.Log("🏁 Destination reached.");
            }
        }
    }

    public bool HasGoal()
    {
        return goalDestination.HasValue;
    }

    public void RecalculatePath()
    {
        if (goalDestination.HasValue)
        {
            Debug.Log("🔄 Lidar triggered replanning...");
            SetTargetPath(goalDestination.Value);
        }
    }


    private void CheckStuck()
    {
        float distanceMoved = Vector3.Distance(rb.position, lastPosition);

        if (distanceMoved < stuckThreshold && IsMoving())
        {
            stuckTimer += Time.fixedDeltaTime;

            if (stuckTimer >= stuckTimeLimit)
            {
                Debug.Log("⚠ Robot stuck! Recalculating path...");

                if (goalDestination.HasValue)
                    SetTargetPath(goalDestination.Value);

                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f; // reset if moving
        }

        lastPosition = rb.position;
    }
}
