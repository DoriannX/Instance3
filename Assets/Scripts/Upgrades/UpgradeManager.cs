using UnityEngine;
using System;

public class UpgradeManager : MonoBehaviour
{
    [Header("Upgrade References")]
    [SerializeField] private SpeedUpgrade speedUpgrade;
    [SerializeField] private HealthUpgrade healthUpgrade;
    [SerializeField] private AmmoUpgrade ammoUpgrade;
    [SerializeField] private CooldownUpgrade cooldownUpgrade;

    // Event to notify when an upgrade is changed.
    // The string parameter identifies the upgrade type (e.g., "Speed") and int parameter is the current level.
    public event Action<string, int> OnUpgradeChanged;

    private Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
        if (player == null)
            Debug.LogError("UpgradeManager must be attached to a Player.");
    }

    // Example methods for applying each upgrade:
    public void ApplySpeedUpgrade()
    {
        if (speedUpgrade.CanUpgrade())
        {
            speedUpgrade.Apply(player);
            OnUpgradeChanged?.Invoke("Speed", speedUpgrade.Level);
        }
        else
        {
            Debug.Log("Speed Upgrade is maxed out.");
        }
    }

    public void ApplyHealthUpgrade()
    {
        if (healthUpgrade.CanUpgrade())
        {
            healthUpgrade.Apply(player);
            OnUpgradeChanged?.Invoke("Health", healthUpgrade.Level);
        }
        else
        {
            Debug.Log("Health Upgrade is maxed out.");
        }
    }

    public void ApplyAmmoUpgrade()
    {
        if (ammoUpgrade.CanUpgrade())
        {
            ammoUpgrade.Apply(player);
            OnUpgradeChanged?.Invoke("Ammo", ammoUpgrade.Level);
        }
        else
        {
            Debug.Log("Ammo Upgrade is maxed out.");
        }
    }

    public void ApplyCooldownUpgrade()
    {
        if (cooldownUpgrade.CanUpgrade())
        {
            cooldownUpgrade.Apply(player);
            OnUpgradeChanged?.Invoke("Cooldown", cooldownUpgrade.Level);
        }
        else
        {
            Debug.Log("Cooldown Upgrade is maxed out.");
        }
    }

    // Optional: method to reset all upgrades (if needed)
    public void ResetUpgrades()
    {
        speedUpgrade.ResetUpgrade();
        healthUpgrade.ResetUpgrade();
        ammoUpgrade.ResetUpgrade();
        cooldownUpgrade.ResetUpgrade();

        OnUpgradeChanged?.Invoke("Speed", speedUpgrade.Level);
        OnUpgradeChanged?.Invoke("Health", healthUpgrade.Level);
        OnUpgradeChanged?.Invoke("Ammo", ammoUpgrade.Level);
        OnUpgradeChanged?.Invoke("Cooldown", cooldownUpgrade.Level);
        Debug.Log("All upgrades have been reset.");
    }
}
