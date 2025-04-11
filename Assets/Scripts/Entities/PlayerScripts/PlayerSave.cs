using Tests;
using UnityEngine;
    /// <summary>
    /// Handles saving and loading of player statistics
    /// </summary>
    public class PlayerSave : MonoBehaviour
    {
        [SerializeField] private Player player;

        private const string KEY_AMMO = "player.ammoMultiplier";
        private const string KEY_COOLDOWN = "player.cooldownMultiplier";

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

                if (ValidateValues(ammo, cooldown))
                {
                    player.SetAmmoMultiplier(ammo);
                    player.SetCooldownMultiplier(cooldown);
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

        private bool ValidateValues(float ammo, float cooldown)
        {
            return ammo > 0 && cooldown > 0; // Add appropriate validation logic
        }

        public void SetPlayer(MockPlayer mockPlayer)
        {
            player = mockPlayer;
        }
    }