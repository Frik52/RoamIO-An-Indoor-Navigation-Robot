using UnityEngine;
using UnityEngine.InputSystem; // ✅ new Input System
using System.Collections.Generic;

public class ClickToSetDestination : MonoBehaviour
{
    [Header("References")]
    public Camera mainCamera;
    public GridManager grid;
    public Pathfinding pathfinding;
    public RobotController robot;

    [Header("Masks")]
    public LayerMask floorMask;     // Layer for the floor/ground
    public LayerMask obstacleMask;  // Layer for obstacles

    void Awake()
    {
        if (!mainCamera) mainCamera = Camera.main;
    }

    void Update()
    {
        // ✅ Check for left mouse click using new Input System
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Mouse click detected");

            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, floorMask))
            {
                Debug.Log("Raycast hit at: " + hit.point);

                // 1. Update grid obstacle states
                if (grid != null)
                {
                    grid.ScanForObstacles(obstacleMask);
                    Debug.Log("Grid scanned for obstacles");
                }

                // 2. Pathfinding
                Vector3 start = robot.transform.position;
                Vector3 target = hit.point;

                List<Vector3> path = pathfinding.FindPath(start, target, true);

                // 3. Send path to robot
                if (path != null && path.Count > 0)
                {
                    robot.SetTargetPath(target);
                    Debug.Log("✅ Path set with " + path.Count + " waypoints.");
                }
                else
                {
                    Debug.LogWarning("⚠ No valid path found!");
                }
            }
            else
            {
                Debug.LogWarning("⚠ Raycast did not hit floor. Check floorMask layer!");
            }
        }
    }
}
