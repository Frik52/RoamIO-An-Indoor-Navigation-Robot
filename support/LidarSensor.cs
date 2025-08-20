
using UnityEngine;

// This script simulates a Lidar sensor for the robot
public class LidarSensor : MonoBehaviour
{
    public float range = 3f;         // Maximum distance the sensor can detect obstacles
    public int rays = 36;            // Number of rays emitted (360° divided into 36 rays = every 10°)
    public LayerMask obstacleMask;   // Layer mask to detect only obstacles

    private RobotController controller; // Reference to the robot's controller

    void Start()
    {
        // Get the RobotController component attached to the same GameObject
        controller = GetComponent<RobotController>();
    }

    void Update()
    {
        bool obstacleDetected = false; // Flag to check if any obstacle is detected

        // Emit rays in a 360° circle around the robot
        for (int i = 0; i < rays; i++)
        {
            float angle = (360f / rays) * i;                  // Angle for current ray
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward; // Direction vector for ray

            // Perform raycast to detect obstacles
            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range, obstacleMask))
            {
                obstacleDetected = true;                     // Obstacle detected
                Debug.DrawRay(transform.position, dir * hit.distance, Color.red); // Draw red ray for detected obstacle
            }
            else
            {
                Debug.DrawRay(transform.position, dir * range, Color.green);      // Draw green ray for clear path
            }
        }

        // If obstacle detected and the robot has a goal, trigger path replanning
        if (obstacleDetected && controller.HasGoal())
        {
            controller.RecalculatePath(); // Call method in RobotController to recalculate path
        }
    }
}
*****************************************************************************























using UnityEngine;

public class LidarSensor : MonoBehaviour
{
    public float range = 3f;
    public int rays = 36; // 360° / 10° = 36 rays
    public LayerMask obstacleMask;

    private RobotController controller;

    void Start()
    {
        controller = GetComponent<RobotController>();
    }

    void Update()
    {
        bool obstacleDetected = false;

        for (int i = 0; i < rays; i++)
        {
            float angle = (360f / rays) * i;
            Vector3 dir = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position, dir, out RaycastHit hit, range, obstacleMask))
            {
                obstacleDetected = true;
                Debug.DrawRay(transform.position, dir * hit.distance, Color.red);
            }
            else
            {
                Debug.DrawRay(transform.position, dir * range, Color.green);
            }
        }

        // If obstacle detected ahead → replan path
        if (obstacleDetected && controller.HasGoal())
        {
            controller.RecalculatePath();
        }
    }
}


