using System;
using UnityEngine;

namespace PlayerTest
{
    public class PlayerSave : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("ammoMultiplier", player.GetAmmoMultiplier());
            PlayerPrefs.SetFloat("cooldownMultiplier", player.GetCooldownMultiplier());
        }

        public void Load()
        {
            player.SetAmmoMultiplier(PlayerPrefs.GetFloat("ammoMultiplier", player.GetAmmoMultiplier()));
            player.SetCooldownMultiplier(PlayerPrefs.GetFloat("cooldownMultiplier", player.GetCooldownMultiplier()));
        }
    }
}