using UnityEngine;
using Armory;

public class PlayerInteract : MonoBehaviour
{
    private Player player;
    private Transform playerTransform;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerTransform = transform;
    }

    public void Interact()
    {
        Debug.Log("interact");
        if (!Physics.Raycast(playerTransform.position, playerTransform.forward, out var hit, 5f))
        {
            Debug.DrawRay(playerTransform.position, playerTransform.forward * 5f, Color.red, 1f);
            return;
        }
        Debug.DrawRay(playerTransform.position, playerTransform.forward * hit.distance, Color.green, 1f);
        
        Debug.Log(hit.collider.name);

        // --- ArmoryTerminal takes priority ---
        if (hit.collider.TryGetComponent<ArmoryTerminal>(out var terminal))
        {
            Debug.Log("Interacting with ArmoryTerminal.");
            terminal.TryOpen();
            return;
        }

        // --- Door logic unchanged ---
        if (hit.collider.TryGetComponent<DoorSystem>(out var door))
        {
            Debug.Log("Interacting with DoorSystem.");
            if (player.hasKey)
            {
                Debug.Log("Player has a key. Opening the door.");
                door.OpenDoor();
                player.hasKey = false;
            }
            else
            {
                Debug.Log("Player does not have a key. Cannot open the door.");
            }
        }
    }
}