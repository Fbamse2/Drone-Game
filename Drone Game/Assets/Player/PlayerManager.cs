using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public GameObject playerPrefab; // Assign your player prefab in the inspector

    private void Start()
    {
        // Ensure the player prefab is set in the inspector
        if (playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in the inspector.");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // If the server, spawn the player prefab for each connected client
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        // Instantiate the player prefab for the newly connected client
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
    }
}
