using Unity.Netcode;
using UnityEngine;

public class NetworkedObjectWithID : NetworkBehaviour
{
    // A static variable to keep track of the next ID
    private static ulong nextID = 1;

    // A NetworkVariable to synchronize the ID across the network
    public NetworkVariable<ulong> objectID = new NetworkVariable<ulong>();

    // This is called when the object is spawned on the server
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            // Assign a unique incremental ID to the object on the server
            objectID.Value = nextID++;
        }

        // Register a callback for when the objectID changes (for clients)
        objectID.OnValueChanged += OnObjectIDChanged;

        // Optional: Do something with the assigned object ID
        Debug.Log($"Object spawned with ID: {objectID.Value}");
    }

    private void OnObjectIDChanged(ulong oldID, ulong newID)
    {
        Debug.Log($"Object ID changed from {oldID} to {newID}");
    }

    // Example of how to instantiate this object on the server
    [ServerRpc(RequireOwnership = false)]
    public void SpawnObjectWithIDServerRpc()
    {
        // Instantiate your prefab and spawn it on the network
        GameObject newObj = Instantiate(Resources.Load("YourPrefab") as GameObject);
        newObj.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        objectID.OnValueChanged -= OnObjectIDChanged;
    }
}
