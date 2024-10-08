using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;           // Speed of the player movement
    private Vector3 moveDirection;         // Direction of player movement
    private Rigidbody rb;                  // Rigidbody for physics-based movement
    public Transform cameraTransform;      // Reference to the player's camera

    // NetworkVariable to synchronize the player's position
    private NetworkVariable<Vector3> networkedPosition = new NetworkVariable<Vector3>(default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    private void Awake()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Find the main camera if no camera is assigned in the inspector
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        // Only allow input for the local player (client that owns this object)
        if (IsOwner)
        {
            // Process player input (WASD / Arrow Keys)
            ProcessInputs();

            // Move the player locally
            Move();

            // Send the movement input to the server for synchronization
            SendMovementServerRpc(moveDirection);
        }
        else
        {
            // If not the owner, update the position from the NetworkVariable
            rb.MovePosition(networkedPosition.Value);
        }
    }

    // Capture WASD input and calculate movement relative to camera direction
    private void ProcessInputs()
    {
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right Arrow
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down Arrow

        // Calculate the forward and right direction relative to the camera
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // We only want movement on the XZ plane, so zero out Y component
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        // Combine input with camera direction to get the desired movement direction
        moveDirection = cameraForward * moveZ + cameraRight * moveX;
        moveDirection.Normalize();  // Normalize the movement vector to prevent faster diagonal movement
    }

    private void FixedUpdate()
    {
        // Physics-based movement is handled in FixedUpdate
        if (IsOwner)
        {
            Move();
        }
    }

    private void Move()
    {
        // Use Rigidbody's MovePosition for smooth movement with physics
        Vector3 newPosition = transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // Update the position locally for the player
        rb.MovePosition(newPosition);

        // Update the NetworkVariable with the new position for all clients to sync
        networkedPosition.Value = newPosition;
    }

    // ServerRpc to synchronize movement direction with the server
    [ServerRpc]
    private void SendMovementServerRpc(Vector3 movement)
    {
        // The server moves the player
        MoveServerSide(movement);
    }

    // Server-side movement handling
    private void MoveServerSide(Vector3 movement)
    {
        // Calculate the new position based on the movement
        Vector3 newPosition = transform.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Update the position on the server and sync it via NetworkVariable
        rb.MovePosition(newPosition);
        networkedPosition.Value = newPosition;
    }
}
