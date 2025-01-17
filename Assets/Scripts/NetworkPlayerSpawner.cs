using UnityEngine;
using Photon.Pun;

public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab; // Reference to the instantiated network player prefab

    // Called when the player successfully joins a room
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom(); // Call the base method to maintain default Photon behavior

        // Ensure the player prefab is not already spawned (avoid duplicate spawns)
        if (spawnedPlayerPrefab == null)
        {
            // Instantiate the network player prefab at the spawner's position & rotation
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("Network Player", transform.position, transform.rotation);
        }
        else
        {
            Debug.LogWarning("Player prefab already spawned!"); // Debug warning to detect unexpected behavior
        }
    }

    // Called when the player leaves the room
    public override void OnLeftRoom()
    {
        base.OnLeftRoom(); // Call the base method to maintain default Photon behavior

        // Destroy the network player prefab when leaving to clean up objects
        if (spawnedPlayerPrefab != null)
        {
            PhotonNetwork.Destroy(spawnedPlayerPrefab);
            spawnedPlayerPrefab = null; // Prevent reference to a destroyed object
        }
    }
}
