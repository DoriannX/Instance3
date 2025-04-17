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
        if (!Physics.Raycast(playerTransform.position, playerTransform.forward, out var hit, 5f))
            return;

        // --- ArmoryTerminal takes priority ---
        if (hit.collider.TryGetComponent<ArmoryTerminal>(out var terminal))
        {
            terminal.TryOpen();
            return;
        }

        // --- Door logic unchanged ---
        if (hit.collider.TryGetComponent<DoorSystem>(out var door))
        {
            if (player.hasKey)
            {
                door.OpenDoor();
                player.hasKey = false;
            }
            else
            {
                Debug.Log("You need a key to open this door.");
            }
        }
    }
}