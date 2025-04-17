using UnityEngine;

namespace UpgradeMachine
{
    public class UpgradeMachine : MonoBehaviour
    {
        [SerializeField] private Player player;

        private PlayerUpgrade playerUpgrade;

        private int currentSpeedLevel;
        private int desiredSpeedLevel;
        private int maxLevel = 5;

        private void Awake()
        {
            playerUpgrade = player.GetComponent<PlayerUpgrade>() ?? throw new MissingComponentException("PlayerUpgrade component not found on player.");
        }

        public void UpgradeMovementSpeed(int number)
        {
            for (int i = currentSpeedLevel; i < maxLevel; i++)
            {
                currentSpeedLevel++;
                playerUpgrade.ApplySpeedUpgrade();
            }
        }

        public void SetDesiredSpeedLevel(int level)
        {
            if (level < 0 || level > maxLevel)
            {
                Debug.LogError("Desired speed level must be between 0 and " + maxLevel);
                return;
            }

            desiredSpeedLevel = level;
        }

        /*private int healthLevel;
        private int ammoLevel;
        private int fireRateLevel;*/
        /*public void UpgradeFireRate(int number)
        {
            for (int i = fireRateLevel; i < maxLevel; i++)
            {
                fireRateLevel++;
                _playerUpgrade.ApplyCooldownUpgrade();
            }
        }

        public void UpgradeMaxHealth(int number)
        {
            for (int i = healthLevel; i < maxLevel; i++)
            {
                healthLevel++;
                _playerUpgrade.ApplyHealthUpgrade();
            }
        }

        public void UpgradeMawAmmo(int number)
        {
            for (int i = ammoLevel; i < maxLevel; i++)
            {
                ammoLevel++;
                _playerUpgrade.ApplyAmmoUpgrade();
            }
        }*/
    }
}