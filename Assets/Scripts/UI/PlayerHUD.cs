using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("HUD Elements")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text chipsText;
    [SerializeField] private Image weaponSlot1;
    [SerializeField] private Image weaponSlot2;
    [SerializeField] private TMP_Text ammoText;
    
    [Header("Player Reference")]
    [SerializeField] private Player player; // Assign the Player in the Inspector.

    private EntityHealth healthComp;
    private PlayerAttack playerAttack;

    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in PlayerHUD. Please assign a Player in the Inspector.");
            return;
        }
        
        healthComp = player.GetComponent<EntityHealth>();
        playerAttack = player.GetComponent<PlayerAttack>();

        // Initialize HUD elements.
        if (healthBar != null && healthComp != null)
        {
            healthBar.maxValue = healthComp.GetMaxHealth();
            healthBar.value = healthComp.Hp;
        }
        if (chipsText != null)
        {
            chipsText.text = player.Chips.ToString();
        }
        // Initially hide weapon icons; they will update once weapons are assigned.
        if (weaponSlot1 != null) weaponSlot1.enabled = false;
        if (weaponSlot2 != null) weaponSlot2.enabled = false;
        if (ammoText != null)
        {
            ammoText.text = "0";
        }
    }

    private void OnEnable()
    {
        if (healthComp != null)
            healthComp.OnHealthChanged += UpdateHealthBar;
        if (player != null)
            player.onChipsChanged += UpdateChips;
        if (playerAttack != null)
            playerAttack.OnAmmoChanged += UpdateAmmo;
        
        // Subscribe to the weapon change event provided by the base Entity class.
        player.OnWeaponChanged += UpdateWeapons;
    }

    private void OnDisable()
    {
        if (healthComp != null)
            healthComp.OnHealthChanged -= UpdateHealthBar;
        if (player != null)
            player.onChipsChanged -= UpdateChips;
        if (player != null)
            player.OnWeaponChanged -= UpdateWeapons;
        if (playerAttack != null)
            playerAttack.OnAmmoChanged -= UpdateAmmo;
    }
    
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }
    
    private void UpdateChips(int chips)
    {
        if (chipsText != null)
        {
            chipsText.text = chips.ToString();
        }
    }

    /// <summary>
    /// Updates the weapon icons. The current weapon's icon goes to slot1,
    /// and the unequipped (or secondary) weapon goes to slot2.
    /// </summary>
    private void UpdateWeapons(Weapon newWeapon)
    {
        // Update current weapon icon (slot1)
        if (newWeapon != null && newWeapon.Data != null)
        {
            if (weaponSlot1 != null)
            {
                weaponSlot1.sprite = newWeapon.Data.icon;
                weaponSlot1.enabled = true;
            }
        }
        else if (weaponSlot1 != null)
        {
            weaponSlot1.enabled = false;
        }

        // Determine the secondary weapon – if current is melee, show ranged; if current is ranged, show melee.
        Weapon secondary = newWeapon switch
        {
            MeleeWeapon when player.RangeWeapon != null => player.RangeWeapon,
            RangeWeapon when player.MeleeWeapon != null => player.MeleeWeapon,
            _ => null
        };

        if (secondary != null && secondary.Data != null)
        {
            if (weaponSlot2 != null)
            {
                weaponSlot2.sprite = secondary.Data.icon;
                weaponSlot2.enabled = true;
            }
        }
        else if (weaponSlot2 != null)
        {
            weaponSlot2.enabled = false;
        }
    }
    
    private void UpdateAmmo(int currentAmmo)
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }
}
