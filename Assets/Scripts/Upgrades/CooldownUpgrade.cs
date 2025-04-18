using UnityEngine;

[System.Serializable]
public class CooldownUpgrade : Upgrade
{
    [Header("Cooldown Upgrade Settings")]
    // Fixed reduction in cooldown multiplier per upgrade level.
    [SerializeField] private float cooldownReduction = 0.1f;

    public override void Apply(Player player)
    {
        // Reduce the player's cooldown multiplier.
        float currentCooldown = player.GetCooldownMultiplier();
        float newCooldown = Mathf.Max(currentCooldown - cooldownReduction, 0.1f); // don't go below 0.1
        player.SetCooldownMultiplier(newCooldown);
        Debug.Log($"Applied CooldownUpgrade: new cooldown multiplier is {newCooldown} (Level {Level}).");
        IncreaseLevel();
    }
}