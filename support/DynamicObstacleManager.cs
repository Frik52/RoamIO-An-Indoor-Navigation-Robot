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
