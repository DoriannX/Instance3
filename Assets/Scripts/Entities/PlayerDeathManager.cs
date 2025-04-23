using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Entities
{
    [RequireComponent(typeof(EntityHealth))]
    public class PlayerDeathManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string deathTrigger = "Die";
        [SerializeField] private float maxRestartTime = 1f;
        [SerializeField] private float colorLerpSpeed = 1f;
        [SerializeField] private Image deathPanel;
        private EntityHealth health;
        private bool isDead = false;
        private float restartTime = 0f;
        private TextMeshProUGUI deathText;

        private void Awake()
        {
            health = GetComponent<EntityHealth>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
            playerHealth = GetComponent<EntityHealth>();
            deathText = deathPanel.GetComponentInChildren<TextMeshProUGUI>();
            Assert.IsNotNull(deathPanel, "Death panel is not assigned in the inspector.");
        }

        private void OnEnable()  => health.onDeath += HandleDeath;
        private void OnDisable() => health.onDeath -= HandleDeath;

        private void HandleDeath()
        {
            // 1) kill velocity
            if (TryGetComponent<Rigidbody>(out var rb))
                rb.linearVelocity = Vector3.zero;

            // 2) disable all player controls
            foreach (var mb in new MonoBehaviour[] {
                         GetComponent<PlayerMovement>(),
                         GetComponent<PlayerDash>(),
                         GetComponent<InputManager>(),
                         GetComponent<PlayerAttack>()
                     })
                if (mb != null) mb.enabled = false;

            // 3) trigger the death animation
            animator.SetTrigger(deathTrigger);

            // 4) start polling for when it’s actually done
            StartCoroutine(WaitForDeathAnimation());
            playerHealth.onDeath += OnPlayerDie;
            restartTime = maxRestartTime;
        }

        private IEnumerator WaitForDeathAnimation()
        {
            // wait until we've actually entered the "Death" state
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            );
            if (isDead)
            {
                Debug.Log("Player is dead. Restarting scene...");
                deathPanel.gameObject.SetActive(true);

                SFXManager.instance.PlaySFX("PlayerDeath");

                // Corrected Lerp usage for smooth transition  
                deathPanel.color = Color.Lerp(deathPanel.color, new Color(0, 0, 0, 1f),
                    colorLerpSpeed * Time.unscaledDeltaTime);
                deathText.color = Color.Lerp(deathText.color, new Color(1f, 0, 0, 1f),
                    colorLerpSpeed * Time.unscaledDeltaTime);

                Time.timeScale =
                    Mathf.Lerp(Time.timeScale, 0f, colorLerpSpeed * Time.unscaledDeltaTime); // Gradually slow down time

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

            // then wait until that state's normalizedTime >= 1 (i.e. fully played)
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
            );

            // now we know the animation is 100% done:
            LevelManager.ReloadLevel();
        }
    }
}