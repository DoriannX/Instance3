using UnityEngine;

[System.Serializable]
public class HealthUpgrade : Upgrade
{
    // Fixed increase in max health per upgrade level.
    [SerializeField] private int healthIncrement = 20;
    public override void Apply(Player player)
    {
        // Increase the player's maximum health via the EntityHealth component.
        EntityHealth health = player.GetComponent<EntityHealth>();
        if (health != null)
        {
            //health.IncreaseMaxHp(healthIncrement);
            health.UpgradeHealthMultiplier(1 + healthIncrement/100);
            Debug.Log($"Applied HealthUpgrade: increased max HP by {healthIncrement} (Level {Level}).");
        }
        IncreaseLevel();
    }
}