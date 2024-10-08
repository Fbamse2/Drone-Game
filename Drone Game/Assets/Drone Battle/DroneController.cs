using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float speed = 5.0f; // Speed at which the player moves
    public float jumpForce = 10.0f; // Force applied when jumping
    public float rotationSpeed = 700.0f; // Speed at which the player rotates
    public float hoverHeight = 2.0f; // Desired height to hover above the ground
    public float hoverForce = 10.0f; // Force to maintain hovering
    public float hoverDampening = 5.0f; // Dampening factor to stabilize hovering
    public LayerMask groundLayer; // Layer mask to determine what is considered ground

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth movement
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Better collision detection
    }

    void FixedUpdate()
    {
        // Hovering logic
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, hoverHeight + 1f, groundLayer))
        {
            float distanceToGround = hit.distance;
            float hoverError = hoverHeight - distanceToGround;
            Vector3 hoverForceVector = Vector3.up * (hoverError * hoverForce - rb.velocity.y * hoverDampening);
            rb.AddForce(hoverForceVector, ForceMode.Acceleration);
        }

        // Get input from horizontal and vertical axes (WASD and arrow keys)
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Create movement vector relative to the object's orientation
        Vector3 moveDirection = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;

        // Move the player
        rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);

        // Rotation
        float rotateInput = Input.GetAxis("Horizontal");
        if (rotateInput != 0)
        {
            float rotation = rotateInput * rotationSpeed * Time.deltaTime;
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, rotation, 0));
        }
    }
}
