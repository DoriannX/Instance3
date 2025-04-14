using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Entities
{
    public class PlayerDeathManager : MonoBehaviour
    {
        [SerializeField] private float restartSpeed = 2f;
        private EntityHealth playerHealth;
        private bool isDead = false;

        private void Awake()
        {
            playerHealth = GetComponent<EntityHealth>();
        }

        private void Start()
        {
            playerHealth.onDeath += OnPlayerDie;
        }

        private void Update()
        {
            if (isDead)
            {
                Debug.Log("Player is dead. Restarting scene...");
                Time.timeScale = Mathf.Max(0, Time.timeScale - Time.unscaledDeltaTime * restartSpeed);
                if (Time.timeScale <= 0)
                {
                    Time.timeScale = 1;
                    LevelManager.ReloadLevel();
                }
            }
        }

        private void OnPlayerDie()
        {
            isDead = true;
        }
    }
}