using UnityEngine;

// This script controls the camera to switch between a top-down view and a follow view for the robot.
public class CameraController : MonoBehaviour
{
    // Reference to the robot's Transform component, assigned in the Unity Inspector
    public Transform robot;

    // Reference to the RobotController script to check if the robot is moving
    public RobotController robotController;

    // Initial camera position (top-down view)
    private Vector3 initialPosition = new Vector3(0f, 70f, 0f);

    // Initial camera rotation (looking straight down)
    private Quaternion initialRotation = Quaternion.Euler(90f, 0f, 0f);

    // Flag to track if the camera is currently following the robot
    private bool following = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set the camera's initial position to the top-down view
        transform.position = initialPosition;

        // Set the camera's initial rotation to look straight down
        transform.rotation = initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Safety check: if robot or robotController is not assigned, do nothing
        if (robotController == null || robot == null) return;

        // Check if the robot is moving
        if (robotController.IsMoving())
        {
            // If the camera is not already in follow mode
            if (!following)
            {
                following = true; // Set follow mode to true
                Debug.Log("📸 Switching to Follow Camera"); // Debug message in console
            }

            // Define the offset position for following the robot (behind and slightly above)
            Vector3 followOffset = new Vector3(10, 2, -5);

            // Calculate the target position by adding the offset to the robot's current position
            Vector3 targetPos = robot.position + followOffset;

            // Smoothly move the camera towards the target position using linear interpolation
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 2f);

            // Make the camera look at the robot's current position
            transform.LookAt(robot);
        }
        else
        {
            // If the camera was following, but the robot has stopped
            if (following)
            {
                following = false; // Set follow mode to false
                Debug.Log("📸 Switching to Top-Down Camera"); // Debug message in console
            }

            // Smoothly move the camera back to the initial top-down position
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * 2f);

            // Smoothly rotate the camera back to the initial top-down rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, Time.deltaTime * 2f);
        }
    }
}
For better understand- with comments
















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

