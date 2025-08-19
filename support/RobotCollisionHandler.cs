using UnityEngine;

public class RobotCollisionHandler : MonoBehaviour
{
    private RobotController robotMovement;

    void Start()
    {
        robotMovement = GetComponent<RobotController>(); // your movement script
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Collision with dynamic object! Replanning path...");

            // Request path recalculation
            robotMovement.RecalculatePath();
        }
    }
}
