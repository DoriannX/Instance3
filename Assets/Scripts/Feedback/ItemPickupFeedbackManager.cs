using UnityEngine;

public class ItemPickupFeedbackManager : MonoBehaviour
{
    // Class names follow PascalCase per convention.
    public static ItemPickupFeedbackManager Instance { get; private set; }

    private void Awake()
    {
        // Set up a singleton for easy access (if allowed for global managers)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        Item.ItemDrop.onItemPickedUp += HandleItemPickupFeedback;
    }

    private void OnDisable()
    {
        Item.ItemDrop.onItemPickedUp -= HandleItemPickupFeedback;
    }

    /// <summary>
    /// Handles the feedback for an item pickup: plays the sound and instantiates VFX.
    /// </summary>
    /// <param name="item">The item that was picked up.</param>
    private void HandleItemPickupFeedback(Item.ItemDrop item)
    {
        // Use SFXManager (which is assumed to be set up) to play the pickup SFX.
        if (!string.IsNullOrEmpty(item.PickupSfxName) && SFXManager.instance != null)
        {
            SFXManager.instance.PlaySFX(item.PickupSfxName);
        }

        // Instantiate the pickup VFX (if any) at the item's position.
        if (item.PickupVfxPrefab != null)
        {
            GameObject vfx = Instantiate(item.PickupVfxPrefab, item.transform.position, Quaternion.identity);
            Destroy(vfx, 1f); // Destroy the VFX after 1 second.
        }
    }
}