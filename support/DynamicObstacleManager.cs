using UnityEngine;

// This script dynamically activates and deactivates obstacles in the scene.
public class DynamicObstacleManager : MonoBehaviour
{
    // Serializable class to store obstacle info and active timer
    [System.Serializable]
    public class ObstacleData
    {
        public GameObject obstacle;      // Reference to the obstacle GameObject
        [HideInInspector] public float activeTimer; // Tracks how long the obstacle has been active (hidden in Inspector)
    }

    // Array of obstacles to manage, assign in the Inspector
    public ObstacleData[] obstacles;

    // Time interval between trying to activate new obstacles
    public float activationInterval = 3f;

    // Minimum number of obstacles that should always be active
    public int minActiveObstacles = 2;

    // Maximum number of obstacles allowed to be active at the same time
    public int maxActiveObstacles = 20;

    // Duration for which an obstacle stays active before being deactivated
    public float activeDuration = 5f;

    // Timer to track time passed for activation
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure all obstacles are inactive at the start
        foreach (var data in obstacles)
        {
            data.obstacle.SetActive(false); // Deactivate the obstacle
            data.activeTimer = 0f;          // Reset its timer
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer by the time passed since last frame
        timer += Time.deltaTime;

        // Count currently active obstacles
        int activeCount = CountActiveObstacles();

        // ✅ Ensure minimum number of obstacles are always active
        while (activeCount < minActiveObstacles)
        {
            if (ActivaterandomObstacle()) activeCount++; // Try activating a random obstacle
            else break; // No more available inactive obstacles to activate
        }

        // Try activating a new obstacle at regular intervals
        if (timer >= activationInterval)
        {
            timer = 0f; // Reset timer
            ActivateRandomObstacle(); // Activate a new random obstacle
        }

        // Handle active timers: deactivate obstacles that have been active too long
        foreach (var data in obstacles)
        {
            if (data.obstacle.activeSelf) // Only check active obstacles
            {
                data.activeTimer += Time.deltaTime; // Increase the obstacle's active timer

                // If obstacle has been active longer than activeDuration, deactivate it
                if (data.activeTimer >= activeDuration)
                {
                    data.obstacle.SetActive(false); // Deactivate
                    data.activeTimer = 0f;          // Reset timer
                }
            }
        }
    }

    // Activates a random obstacle if maxActiveObstacles is not reached
    void ActivateRandomObstacle()
    {
        // Count currently active obstacles
        int activeCount = 0;
        foreach (var data in obstacles)
        {
            if (data.obstacle.activeSelf) activeCount++;
        }

        // Activate a random obstacle if we haven't reached the maximum
        if (activeCount < maxActiveObstacles)
        {
            int randomIndex = Random.Range(0, obstacles.Length);

            // Activate only if the obstacle is currently inactive
            if (!obstacles[randomIndex].obstacle.activeSelf)
            {
                obstacles[randomIndex].obstacle.SetActive(true);
                obstacles[randomIndex].activeTimer = 0f; // Reset timer when activated
            }
        }
    }

    // Attempts to activate a random inactive obstacle, returns true if successful
    bool ActivaterandomObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Length);

        // Activate only if the obstacle is currently inactive
        if (!obstacles[randomIndex].obstacle.activeSelf)
        {
            obstacles[randomIndex].obstacle.SetActive(true);
            obstacles[randomIndex].activeTimer = 0f; // Reset timer
            return true; // Successfully activated
        }

        return false; // Failed to activate (obstacle was already active)
    }

    // Counts how many obstacles are currently active
    int CountActiveObstacles()
    {
        int count = 0;
        foreach (var data in obstacles)
        {
            if (data.obstacle.activeSelf) count++;
        }
        return count;
    }
}
  *****************************************************************




































using UnityEngine;

public class DynamicObstacleManager : MonoBehaviour
{
    [System.Serializable]
    public class ObstacleData
    {
        public GameObject obstacle;
        [HideInInspector] public float activeTimer;
    }

    public ObstacleData[] obstacles;   // Assign cubes from inspector
    public float activationInterval = 3f;  // Time between activations
    public int minActiveObstacles = 2;
    public int maxActiveObstacles = 20;     // Limit how many can be active
    public float activeDuration = 5f;      // How long an obstacle stays active

    private float timer;

    void Start()
    {
        // Ensure all obstacles are inactive at start
        foreach (var data in obstacles)
        {
            data.obstacle.SetActive(false);
            data.activeTimer = 0f;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        int activeCount = CountActiveObstacles();

        // ✅ Ensure minimum number of obstacles are always active
        while (activeCount < minActiveObstacles)
        {
            if (ActivaterandomObstacle()) activeCount++;
            else break; // No more available obstacles
        }

        // Try activating a new obstacle at intervals
        if (timer >= activationInterval)
        {
            timer = 0f;
            ActivateRandomObstacle();
        }

        // Handle active timers (auto-deactivation)
        foreach (var data in obstacles)
        {
            if (data.obstacle.activeSelf)
            {
                data.activeTimer += Time.deltaTime;

                if (data.activeTimer >= activeDuration)
                {
                    data.obstacle.SetActive(false);
                    data.activeTimer = 0f;
                }
            }
        }
    }

    void ActivateRandomObstacle()
    {
        // Count currently active obstacles
        int activeCount = 0;
        foreach (var data in obstacles)
        {
            if (data.obstacle.activeSelf) activeCount++;
        }

        // If we haven't reached the max, activate a random one
        if (activeCount < maxActiveObstacles)
        {
            int randomIndex = Random.Range(0, obstacles.Length);

            if (!obstacles[randomIndex].obstacle.activeSelf)
            {
                obstacles[randomIndex].obstacle.SetActive(true);
                obstacles[randomIndex].activeTimer = 0f; // reset timer when activated
            }
        }
    }
    
    bool ActivaterandomObstacle()
    {
        int randomIndex = Random.Range(0, obstacles.Length);

        if (!obstacles[randomIndex].obstacle.activeSelf)
        {
            obstacles[randomIndex].obstacle.SetActive(true);
            obstacles[randomIndex].activeTimer = 0f;
            return true;
        }

        return false;
    }

     int CountActiveObstacles()
    {
        int count = 0;
        foreach (var data in obstacles)
        {
            if (data.obstacle.activeSelf) count++;
        }
        return count;
    }
}

