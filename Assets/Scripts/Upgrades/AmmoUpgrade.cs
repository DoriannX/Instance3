using UnityEngine;

[System.Serializable]
public class AmmoUpgrade : Upgrade
{
    [Header("Ammo Upgrade Settings")]
    // Fixed increase in ammo capacity per upgrade level.
    [SerializeField] private int ammoIncrement = 10;

    public override void Apply(Player player)
    {
        player.SetAmmoMultiplier(1+ammoIncrement/100);
        Debug.Log($"Applied AmmoUpgrade: increased ammo capacity by {ammoIncrement} (Level {Level}).");
        IncreaseLevel();
    }
}