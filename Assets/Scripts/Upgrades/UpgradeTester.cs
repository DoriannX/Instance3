using UnityEngine;

public class UpgradeTester : MonoBehaviour
{
    [Header("Player Reference")]
    public Player player;  // Assign your Player instance via the Inspector

    [Header("Upgrade Instances")]
    public SpeedUpgrade speedUpgrade;       // Configure via Inspector
    public HealthUpgrade healthUpgrade;     // Configure via Inspector
    public AmmoUpgrade ammoUpgrade;         // Configure via Inspector
    public CooldownUpgrade cooldownUpgrade; // Configure via Inspector

    // These methods will be hooked up to UI buttons in the Inspector.
    
    public void OnApplySpeedUpgrade()
    {
        if (speedUpgrade.CanUpgrade())
        {
            speedUpgrade.Apply(player);
        }
        else
        {
            Debug.Log("Speed Upgrade is maxed out.");
        }
    }

    public void OnApplyHealthUpgrade()
    {
        if (healthUpgrade.CanUpgrade())
        {
            healthUpgrade.Apply(player);
        }
        else
        {
            Debug.Log("Health Upgrade is maxed out.");
        }
    }

    public void OnApplyAmmoUpgrade()
    {
        if (ammoUpgrade.CanUpgrade())
        {
            ammoUpgrade.Apply(player);
        }
        else
        {
            Debug.Log("Ammo Upgrade is maxed out.");
        }
    }

    public void OnApplyCooldownUpgrade()
    {
        if (cooldownUpgrade.CanUpgrade())
        {
            cooldownUpgrade.Apply(player);
        }
        else
        {
            Debug.Log("Cooldown Upgrade is maxed out.");
        }
    }

    // New method to reset all upgrades for testing.
    public void OnResetUpgrades()
    {
        speedUpgrade.ResetUpgrade();
        healthUpgrade.ResetUpgrade();
        ammoUpgrade.ResetUpgrade();
        cooldownUpgrade.ResetUpgrade();

        Debug.Log("All upgrades have been reset.");
    }
}