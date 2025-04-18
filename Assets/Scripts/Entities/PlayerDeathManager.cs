using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Entities
{
    public class PlayerDeathManager : MonoBehaviour
    {
        private EntityHealth playerHealth;
        private bool isDead = false;
        private float restartTime = 0f;
        [SerializeField] private float maxRestartTime = 1f;
        [SerializeField] private float colorLerpSpeed = 1f;
        [SerializeField] private Image deathPanel;
        private TextMeshProUGUI deathText;

        private void Awake()
        {
            playerHealth = GetComponent<EntityHealth>();
            deathText = deathPanel.GetComponentInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(deathPanel, "Death panel is not assigned in the inspector.");
        }

        private void Start()
        {
            playerHealth.onDeath += OnPlayerDie;
            restartTime = maxRestartTime;
        }

        private void Update()
        {
            if (isDead)
            {
                Debug.Log("Player is dead. Restarting scene...");
                deathPanel.gameObject.SetActive(true);

                // Corrected Lerp usage for smooth transition  
                deathPanel.color = Color.Lerp(deathPanel.color, new Color(0, 0, 0, 1f), colorLerpSpeed * Time.unscaledDeltaTime);
                deathText.color = Color.Lerp(deathText.color, new Color(1f, 0, 0, 1f), colorLerpSpeed * Time.unscaledDeltaTime);

                Time.timeScale = 0; // Slow down time for a better visual effect

                if (restartTime <= 0)
                {
                    Time.timeScale = 1;
                    LevelManager.ReloadLevel();
                }
                else
                {
                    restartTime -= Time.unscaledDeltaTime;
                }
            }
        }

        private void OnPlayerDie()
        {
            isDead = true;
        }
    }
}