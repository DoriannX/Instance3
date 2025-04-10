using UnityEngine;

[System.Serializable]
public abstract class Upgrade
{
    [SerializeField] private string upgradeName;  // Name for identification
    [SerializeField] private int level = 0;         // Current upgrade level
    [SerializeField] private int maxLevel = 5;        // Maximum level for this upgrade
    [SerializeField] private int baseCost = 10;       // Base cost for the first upgrade

    // Public properties for designer and code access
    public string UpgradeName => upgradeName;
    public int Level => level;
    public int MaxLevel => maxLevel;

    // Cost increases exponentially based on the current level.
    public int Cost => baseCost * (int)Mathf.Pow(2, level);

    public bool CanUpgrade()
    {
        return level < maxLevel;
    }

    public void IncreaseLevel()
    {
        if (CanUpgrade())
        {
            level++;
        }
    }

    // Reset the upgrade level (for testing purposes)
    public void ResetUpgrade()
    {
        level = 0;
    }

    /// <summary>
    /// Applies the effect of the upgrade to the player.
    /// Derived classes must implement this method.
    /// </summary>
    /// <param name="player">Reference to the Player</param>
    public abstract void Apply(Player player);
}