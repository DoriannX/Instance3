using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerProfileUI : MonoBehaviour
{
    [Header("Player Profile UI Elements")]
    [SerializeField] private TMP_Text chipsText;
    [SerializeField] private Image meleeWeaponImage;
    [SerializeField] private Image rangedWeaponImage;
    [SerializeField] private TMP_Text playerNameText;

    [Header("Upgrade UI - Radial Sliders")]
    [Tooltip("Order: 0-Speed, 1-Health, 2-Ammo, 3-Cooldown")]
    [SerializeField] private Slider[] upgradeSliders;

    private Player player;
    private UpgradeManager upgradeManager;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        upgradeManager = player.GetComponent<UpgradeManager>();

        if (player == null || upgradeManager == null)
        {
            Debug.LogError("Player or UpgradeManager not found in scene.");
            return;
        }

        playerNameText.text = player.name;

        // Subscribe to events.
        player.onChipsChanged += UpdateChipsUI;
        player.OnWeaponChanged += UpdateWeaponUI;
        upgradeManager.OnUpgradeChanged += UpdateUpgradeUI;

        // Initial UI update.
        UpdateChipsUI(player.Chips);
        UpdateWeaponUI(player.CurrentWeapon);

        for (int i = 0; i < upgradeSliders.Length; i++)
        {
            upgradeSliders[i].maxValue = 5;
            upgradeSliders[i].value = 0;
        }
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.onChipsChanged -= UpdateChipsUI;
            player.OnWeaponChanged -= UpdateWeaponUI;
        }
        if (upgradeManager != null)
        {
            upgradeManager.OnUpgradeChanged -= UpdateUpgradeUI;
        }
    }

    private void UpdateChipsUI(int chips)
    {
        chipsText.text = chips.ToString();
    }

    private void UpdateWeaponUI(Weapon newWeapon)
    {
        // Use the public property Data to access the icon.
        if (newWeapon is MeleeWeapon && newWeapon.Data != null)
        {
            meleeWeaponImage.sprite = newWeapon.Data.icon;
        }
        else if (newWeapon is RangeWeapon && newWeapon.Data != null)
        {
            rangedWeaponImage.sprite = newWeapon.Data.icon;
        }
        // Alternatively, update both always:
        if (player.MeleeWeapon != null && player.MeleeWeapon.Data != null)
            meleeWeaponImage.sprite = player.MeleeWeapon.Data.icon;
        if (player.RangeWeapon != null && player.RangeWeapon.Data != null)
            rangedWeaponImage.sprite = player.RangeWeapon.Data.icon;
    }

    private void UpdateUpgradeUI(string upgradeType, int level)
    {
        int index = -1;
        switch (upgradeType)
        {
            case "Speed": index = 0; break;
            case "Health": index = 1; break;
            case "Ammo":   index = 2; break;
            case "Cooldown": index = 3; break;
        }

        if (index >= 0 && index < upgradeSliders.Length)
        {
            upgradeSliders[index].value = level;
        }
    }
}
