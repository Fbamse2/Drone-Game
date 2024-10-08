using Unity.Netcode;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public GameObject playerPrefab; // Assign your player prefab in the inspector

    void Awake()
    {
        // Check if the player prefab is assigned
        if (playerPrefab != null)
        {
            // Ensure the prefab has a NetworkObject component
            NetworkObject networkObject = playerPrefab.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                // Register the player prefab with the NetworkManager
                NetworkManager.Singleton.NetworkConfig.PlayerPrefab = playerPrefab;
            }
            else
            {
                Debug.LogError("Player prefab does not have a NetworkObject component.");
            }
        }
        else
        {
            Debug.LogError("Player prefab is not assigned in the inspector.");
        }
    }

    public void StartHost()
    {
        Debug.Log("Starting host...");
        if (!NetworkManager.Singleton.IsListening) // Check if a network session is already running
        {
            NetworkManager.Singleton.StartHost(); // Start the host server
            Debug.Log("Host started.");
            // A player object will be automatically instantiated for the host
        }
        else
        {
            Debug.LogWarning("Network session already running!");
        }
    }

    public void StartClient()
    {
        Debug.Log("Starting client...");
        if (!NetworkManager.Singleton.IsListening) // Ensure the server is not already running
        {
            NetworkManager.Singleton.StartClient(); // Start the client
            Debug.Log("Client started.");
        }
        else
        {
            Debug.LogWarning("Cannot start client; server is running!");
        }
    }
}
