using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    [Header("Player Reference")]
    [SerializeField] private Player player;

    private PlayerUpgrade _playerUpgrade;

    private void Awake()
    {
        if (player == null)
        {
            Debug.LogError("Player reference is missing in PlayerProfileUI. Please assign a Player in the Inspector.");
            return;
        }

        _playerUpgrade = player.GetComponent<PlayerUpgrade>();
        if (_playerUpgrade == null)
        {
            Debug.LogError("PlayerUpgrade not found on the assigned Player.");
            return;
        }

        // Set initial UI values.
        playerNameText.text = player.name; // Ensure player's name is set properly (the GameObject name or an explicit property)
        UpdateChipsUI(player.Chips);
        UpdateWeaponUI(player.CurrentWeapon);
        
        // Set initial upgrade slider values.
        for (int i = 0; i < upgradeSliders.Length; i++)
        {
            upgradeSliders[i].maxValue = 5;
            upgradeSliders[i].value = 0;
        }
    }

    private void OnEnable()
    {
        if (player != null)
        {
            player.onChipsChanged += UpdateChipsUI;
            player.OnWeaponChanged += UpdateWeaponUI;
        }
        if (_playerUpgrade != null)
        {
            _playerUpgrade.OnUpgradeChanged += UpdatePlayerUpgradeUI;
        }
    }

    private void OnDisable()
    {
        if (player != null)
        {
            player.onChipsChanged -= UpdateChipsUI;
            player.OnWeaponChanged -= UpdateWeaponUI;
        }
        if (_playerUpgrade != null)
        {
            _playerUpgrade.OnUpgradeChanged -= UpdatePlayerUpgradeUI;
        }
    }

    private void UpdateChipsUI(int chips)
    {
        if (chipsText != null)
        {
            chipsText.text = chips.ToString();
            Debug.Log($"Chips updated: {chips}");
        }
    }

    private void UpdateWeaponUI(Weapon newWeapon)
    {
        // Log to see what weapon is passed in
        Debug.Log($"Weapon changed: {(newWeapon != null ? newWeapon.gameObject.name : "null")}");

        // Update current weapon icon (slot1)
        if (newWeapon != null && newWeapon.Data != null)
        {
            if (newWeapon is MeleeWeapon)
            {
                meleeWeaponImage.sprite = newWeapon.Data.icon;
                meleeWeaponImage.enabled = true;
            }
            else if (newWeapon is RangeWeapon)
            {
                rangedWeaponImage.sprite = newWeapon.Data.icon;
                rangedWeaponImage.enabled = true;
            }
        }
        else
        {
            if (meleeWeaponImage != null) meleeWeaponImage.enabled = false;
            if (rangedWeaponImage != null) rangedWeaponImage.enabled = false;
        }

        // Optionally, force update for both slots using player's equipped weapons
        if (player.MeleeWeapon != null && player.MeleeWeapon.Data != null)
        {
            meleeWeaponImage.sprite = player.MeleeWeapon.Data.icon;
            meleeWeaponImage.enabled = true;
        }
        if (player.RangeWeapon != null && player.RangeWeapon.Data != null)
        {
            rangedWeaponImage.sprite = player.RangeWeapon.Data.icon;
            rangedWeaponImage.enabled = true;
        }
    }

    private void UpdatePlayerUpgradeUI(string upgradeType, int level)
    {
        Debug.Log($"Upgrade changed: {upgradeType} new level: {level}");
        int index = -1;
        switch (upgradeType)
        {
            case "Speed": index = 0; break;
            case "Health": index = 1; break;
            case "Ammo": index = 2; break;
            case "Cooldown": index = 3; break;
            default:
                Debug.LogWarning($"Upgrade type {upgradeType} not recognized.");
                break;
        }

        if (index >= 0 && index < upgradeSliders.Length)
        {
            upgradeSliders[index].value = level;
        }
    }
}
