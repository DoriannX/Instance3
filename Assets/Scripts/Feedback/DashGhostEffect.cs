using UnityEngine;

public class DashGhostEffect : MonoBehaviour
{
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private float ghostLifetime = 0.3f;

    // Call this method to trigger a ghost effect.
    public void TriggerGhost()
    {
        if (ghostPrefab != null)
        {
            // Instantiate the ghost at the player's position and rotation.
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            // Optionally, adjust the scale or any parameters on the ghost here.
            Destroy(ghost, ghostLifetime);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: No ghost prefab assigned for dash effect.");
        }
    }
}