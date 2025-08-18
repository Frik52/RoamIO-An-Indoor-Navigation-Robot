using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform robot; // assign in inspector
    public RobotController robotController;

    private Vector3 initialPosition = new Vector3(0f, 70f, 0f);
    private Quaternion initialRotation = Quaternion.Euler(90f, 0f, 0f);

    private bool following = false;

    void Start()
    {
        // Set initial top-down angle
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }

    void Update()
    {
        if (robotController == null || robot == null) return;

        if (robotController.IsMoving())
        {
            // Switch to follow camera if not already
            if (!following)
            {
                following = true;
                Debug.Log("📸 Switching to Follow Camera");
            }

            // Smoothly follow robot from behind
            Vector3 followOffset = new Vector3(10, 2, -5);
            Vector3 targetPos = robot.position + followOffset;
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2f);
            transform.LookAt(robot);
        }
        else
        {
            // Switch back to top-down when robot stops
            if (following)
            {
                following = false;
                Debug.Log("📸 Switching to Top-Down Camera");
            }

            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 2f);
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * 2f);
        }
    }
}
