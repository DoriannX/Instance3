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

    RaycastHit[] hits = new RaycastHit[5];

    public void Interact()
    {
        Debug.Log("interact");
        int hitCount = Physics.RaycastNonAlloc(playerTransform.position, playerTransform.forward, hits, 5f);

        if (hitCount <= 0)
        {
            Debug.DrawRay(playerTransform.position, playerTransform.forward * 5f, Color.red, 1f);
            return;
        }
        foreach(RaycastHit hit in hits)
        {
            Debug.DrawRay(playerTransform.position, playerTransform.forward * hit.distance, Color.green, 1f);

            Debug.Log(hit.collider.name);

            // --- ArmoryTerminal takes priority ---
            ArmoryTerminal armoryTerminal = hit.collider.GetComponentInParent<ArmoryTerminal>();
            if (armoryTerminal != null)
            {
                Debug.Log("Interacting with ArmoryTerminal.");
                armoryTerminal.TryOpen();
                return;
            }

            // --- Door logic unchanged ---
            DoorSystem doorSystem = hit.collider.GetComponentInParent<DoorSystem>();
            if (doorSystem != null)
            {
                Debug.Log("Interacting with DoorSystem.");
                if (Player.hasKey)
                {
                    Debug.Log("Player has a key. Opening the door.");
                    doorSystem.OpenDoor();
                    player.HasKey(false);
                    SFXManager.instance.PlaySFX("DoorOpen");
                }
                else
                {
                    Debug.Log("Player does not have a key. Cannot open the door.");
                }
                break;
            }
        }
    }
}