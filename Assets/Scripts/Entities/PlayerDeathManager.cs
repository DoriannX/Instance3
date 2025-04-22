using UnityEngine;
using System.Collections;

namespace Entities
{
    [RequireComponent(typeof(EntityHealth))]
    public class PlayerDeathManager : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string deathTrigger = "Die";
        private EntityHealth health;

        private void Awake()
        {
            health = GetComponent<EntityHealth>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
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
        }

        private IEnumerator WaitForDeathAnimation()
        {
            // wait until we've actually entered the "Death" state
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).IsName("Death")
            );

            // then wait until that state's normalizedTime >= 1 (i.e. fully played)
            yield return new WaitUntil(() =>
                animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f
            );

            // now we know the animation is 100% done:
            LevelManager.ReloadLevel();
        }
    }
}