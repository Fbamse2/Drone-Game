using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;               // The target to follow (local player)
    public Vector3 offset = new Vector3(0, 2, -5); // Offset from the player
    public float mouseSensitivity = 2.0f;   // Sensitivity of mouse movement
    public float verticalRotationLimit = 80f; // Limit the vertical rotation

    private float yaw;                       // Horizontal rotation around the Y-axis
    private float pitch;                     // Vertical rotation around the X-axis

    private void Start()
    {
        // Find the local player and set it as the target
        foreach (var player in FindObjectsOfType<PlayerMovement>())
        {
            if (player.IsOwner)  // Find the player that this client owns
            {
                target = player.transform;
                break;
            }
        }

        // Lock the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Get mouse input for camera rotation
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            // Update yaw and pitch based on mouse movement
            yaw += mouseX;
            pitch -= mouseY;

            // Clamp the pitch to prevent flipping the camera
            pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);

            // Apply rotation to the camera
            transform.eulerAngles = new Vector3(pitch, yaw, 0);

            // Update the camera's position to follow the target
            transform.position = target.position + offset;
        }
    }
}
