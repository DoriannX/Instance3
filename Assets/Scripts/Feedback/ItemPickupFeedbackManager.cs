using Item.Drops;
using UnityEngine;

public class ItemPickupFeedbackManager : MonoBehaviour
{
    public static ItemPickupFeedbackManager Instance { get; private set; }
    
    [SerializeField] private GameObject genericPickupVfx;

    private void Awake()
    {
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
    /// Handles the feedback for an item pickup. It plays an SFX and instantiates a VFX,
    /// choosing different feedback based on the item type.
    /// </summary>
    /// <param name="item">The item that was picked up.</param>
    private void HandleItemPickupFeedback(Item.ItemDrop item)
    {
        // Determine pickup SFX name based on the type of item.
        string pickupSfxName = item switch
        {
            Chips => "ChipsPickup",
            Bandages => "BandagesPickup",
            Ammo => "AmmoPickup",
            _ => "Pickup"
        };        

        // Determine VFX prefab – here we use a generic one.
        GameObject vfxPrefab = genericPickupVfx;

        if (vfxPrefab == null) return;
        
        GameObject vfx = Instantiate(vfxPrefab, item.transform.position, Quaternion.identity);
        
        Destroy(vfx, 1f);
    }
}
