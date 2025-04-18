using UnityEngine;

[System.Serializable]
public class HealthUpgrade : Upgrade
{
    [Header("Health Upgrade Settings")]
    // Fixed increase in max health per upgrade level.
    [SerializeField] private int healthIncrement = 20;

    public override void Apply(Player player)
    {
        // Increase the player's maximum health via the EntityHealth component.
        EntityHealth health = player.GetComponent<EntityHealth>();
        if (health != null)
        {
            health.IncreaseMaxHp(healthIncrement);
            Debug.Log($"Applied HealthUpgrade: increased max HP by {healthIncrement} (Level {Level}).");
        }
        IncreaseLevel();
    }
}