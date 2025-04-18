using UnityEngine;

[System.Serializable]
public class SpeedUpgrade : Upgrade
{
    // Fixed increase in speed per upgrade level.
    [SerializeField] private float speedIncrement = 10f;
    public override void Apply(Player player)
    {
        // Increase the player's speed (found in Entity) by a fixed increment.
        //player.Speed += speedIncrement;
        player.SetSpeedMultiplier(1+speedIncrement/100);
        IncreaseLevel();
        Debug.Log($"Applied SpeedUpgrade: new speed is {player.Speed} (Level {Level}).");
    }
}