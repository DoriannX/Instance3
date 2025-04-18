using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Player player;
    private Transform playerTransform;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerTransform = GetComponent<Transform>();
    }

    public void Interact()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerTransform.position, playerTransform.forward, out hit, 5f))
        {
            DoorSystem doorSystem = hit.collider.GetComponent<DoorSystem>();

            if (doorSystem != null)
            {
                if (player.hasKey)
                {
                    doorSystem.OpenDoor();
                    player.hasKey = false;
                }
                else
                {
                    Debug.Log("You need a key to open this door.");
                }
            }
        }
    }
}
