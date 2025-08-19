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
