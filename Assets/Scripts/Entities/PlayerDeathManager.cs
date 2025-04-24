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
        [Header("Animator Settings")]
        [SerializeField] private Animator animator;
        [SerializeField] private string deathTrigger = "Die";

        [Header("Death Panel Settings")]
        [SerializeField] private Image deathPanel;
        [SerializeField] private float maxRestartTime = 1f;
        [SerializeField] private float colorLerpSpeed = 1f;

        private EntityHealth health;
        private TextMeshProUGUI deathText;

        private void Awake()
        {
            health = GetComponent<EntityHealth>();

            // Fallback if you forgot to assign Animator in the Inspector
            if (animator == null)
                animator = GetComponentInChildren<Animator>();

            // Make sure the panel reference is set, then grab its Text
            Assert.IsNotNull(deathPanel, "Death panel is not assigned in the inspector.");
            deathText = deathPanel.GetComponentInChildren<TextMeshProUGUI>();

            // Start with panel hidden
            deathPanel.gameObject.SetActive(false);
        }

        private void OnEnable()  => health.onDeath += HandleDeath;
        private void OnDisable() => health.onDeath -= HandleDeath;

        private void HandleDeath()
        {
            // 1) kill any velocity
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

            // 4) launch our fade-&-reload coroutine
            StartCoroutine(PlayDeathSequence());
        }

        private IEnumerator PlayDeathSequence()
        {
            // A) wait until the Animator actually enters the "Death" state
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            );

            // B) show panel and sfx
            deathPanel.gameObject.SetActive(true);
            SFXManager.instance.PlaySFX("PlayerDeath");

            // initialize transparent
            Color panelClr = deathPanel.color; panelClr.a = 0f;
            deathPanel.color = panelClr;
            Color textClr = deathText.color; textClr.a = 0f;
            deathText.color = textClr;

            // C) over maxRestartTime seconds, fade panel to black, text to red, and slow time to 0
            float elapsed = 0f;
            while (elapsed < maxRestartTime)
            {
                elapsed += Time.unscaledDeltaTime * colorLerpSpeed;
                float alpha = Mathf.Clamp01(elapsed / maxRestartTime);

                // fade panel from transparent to opaque black
                deathPanel.color = new Color(0f, 0f, 0f, alpha);

                // fade text from transparent to opaque red
                deathText.color = new Color(1f, 0f, 0f, alpha);

                // slow game time
                Time.timeScale = Mathf.Lerp(1f, 0f, alpha);

                yield return null;
            }

            // D) restore time & reload
            Time.timeScale = 1f;
            LevelManager.ReloadLevel();
        }
    }
}
