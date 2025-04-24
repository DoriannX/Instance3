using UnityEngine;
    /// <summary>
    /// Handles saving and loading of player statistics
    /// </summary>
    public class PlayerSave : MonoBehaviour
    {
        [SerializeField] private Player player;

        private const string KEY_AMMO = "player.ammoMultiplier";
        private const string KEY_COOLDOWN = "player.cooldownMultiplier";
        private const string KEY_MAXHEALTH = "player.maxHealth";
        private const string KEY_SPEED = "player.speedMultiplier";

        private void Start()
        {
            if (!TryGetComponent(out player))
            {
                Debug.LogWarning($"[PlayerSave] Missing Player component on {gameObject.name}");
                enabled = false;
            }
        }

        /// <summary>
        /// Saves player statistics to persistent storage
        /// </summary>
        /// <returns>True if save was successful</returns>
        public bool Save()
        {
            try
            {
                PlayerPrefs.SetFloat(KEY_AMMO, player.GetAmmoMultiplier());
                PlayerPrefs.SetFloat(KEY_COOLDOWN, player.GetCooldownMultiplier());
                PlayerPrefs.SetInt(KEY_MAXHEALTH, player.healthComponent.maxHp);
                PlayerPrefs.SetFloat(KEY_SPEED, player.GetSpeedMultiplier());
                PlayerPrefs.Save();
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[PlayerSave] Failed to save player data: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Loads player statistics from persistent storage
        /// </summary>
        /// <returns>True if load was successful</returns>
        public bool Load()
        {
            try
            {
                float ammo = PlayerPrefs.GetFloat(KEY_AMMO, player.GetAmmoMultiplier());
                float cooldown = PlayerPrefs.GetFloat(KEY_COOLDOWN, player.GetCooldownMultiplier());
                int maxHealth = PlayerPrefs.GetInt(KEY_MAXHEALTH, player.healthComponent.maxHp);
                float speed = PlayerPrefs.GetFloat(KEY_SPEED, player.GetSpeedMultiplier());

                if (ValidateValues(ammo, cooldown, speed, maxHealth))
                {
                    player.SetAmmoMultiplier(ammo);
                    player.SetCooldownMultiplier(cooldown);
                    player.SetMaxHealth(maxHealth);
                    player.SetSpeedMultiplier(speed);
                    return true;
                }

                return false;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[PlayerSave] Failed to load player data: {e.Message}");
                return false;
            }
        }

        private bool ValidateValues(float ammo, float cooldown, float speed, int maxHealth)
        {
            return ammo > 0 && cooldown > 0 && speed > 0 && maxHealth > 0;
        }

        public void SetPlayer(Player mockPlayer)
        {
            player = mockPlayer;
        }
    }