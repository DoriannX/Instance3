using UnityEngine;

[System.Serializable]
public class CooldownUpgrade : Upgrade
{
    [Header("Cooldown Upgrade Settings")]
    // Fixed reduction in cooldown multiplier per upgrade level.
    [SerializeField] private float cooldownReduction = 10f;

    public override void Apply(Player player)
    {
        // Reduce the player's cooldown multiplier.
        //float currentCooldown = player.GetCooldownMultiplier();
        //float newCooldown = Mathf.Max(currentCooldown - cooldownReduction, 0.1f); // don't go below 0.1
        player.SetCooldownMultiplier(1+cooldownReduction/100);
        Debug.Log($"Applied CooldownUpgrade: new cooldown multiplier is {1 + cooldownReduction / 100} (Level {Level}).");
        IncreaseLevel();
    }
}