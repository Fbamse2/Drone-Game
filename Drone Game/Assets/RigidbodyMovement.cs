using Unity.Netcode;
using UnityEngine;

public class RigidbodyMovement : NetworkBehaviour
{
    public float speed = 5.0f;
    public float rotationSpeed = 720f; // Degrees per second
    public Camera playerCamera; // Assign your camera in the inspector

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Only allow the local player to control their character
        if (!IsOwner) return;

        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on camera orientation
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Flatten the forward vector to avoid moving up/down
        forward.y = 0;
        right.y = 0;

        // Normalize to avoid diagonal movement being faster
        forward.Normalize();
        right.Normalize();

        // Calculate movement vector
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        if (movement.magnitude > 0.1f)
        {
            // Rotate the player towards the direction of movement
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        // Only allow the local player to control their character
        if (!IsOwner) return;

        // Get input for movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate movement direction based on camera orientation
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Flatten the forward vector to avoid moving up/down
        forward.y = 0;
        right.y = 0;

        // Normalize to avoid diagonal movement being faster
        forward.Normalize();
        right.Normalize();

        // Calculate movement vector
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        // Move the player using Rigidbody physics
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
