using UnityEngine;

[System.Serializable]
public class AmmoUpgrade : Upgrade
{
    [Header("Ammo Upgrade Settings")]
    // Fixed increase in ammo capacity per upgrade level.
    [SerializeField] private int ammoIncrement = 10;

    public override void Apply(Player player)
    {
        // Since we haven't implemented weapons in detail,
        // we simply log the effect. Later this would increase ammo capacity.
        Debug.Log($"Applied AmmoUpgrade: increased ammo capacity by {ammoIncrement} (Level {Level}).");
        IncreaseLevel();
    }
}