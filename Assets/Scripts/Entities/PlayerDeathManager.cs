// Path: Entities/PlayerDeathManager.cs

using System.Collections;
using UnityEngine;

namespace Entities
{
    [RequireComponent(typeof(EntityHealth))]
    public class PlayerDeathManager : MonoBehaviour
    {
        [Tooltip("Animator driving the Death animation")]
        [SerializeField] private Animator animator;

        [Tooltip("Trigger parameter name for Death")]
        [SerializeField] private string deathTrigger = "Die";

        [Tooltip("Exact name of the Death animation clip")]
        [SerializeField] private string deathClipName = "Death";

        private EntityHealth health;

        private void Awake()
        {
            health = GetComponent<EntityHealth>();
            if (animator == null)
                animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            health.onDeath += HandleDeath;
        }

        private void OnDisable()
        {
            health.onDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            // 1) Stop any leftover velocity
            var rb = GetComponent<Rigidbody>();
            if (rb != null) rb.linearVelocity = Vector3.zero;

            // 2) Kill player control so nothing can interrupt the Death clip
            foreach (var mb in new MonoBehaviour[] {
                GetComponent<PlayerMovement>(),
                GetComponent<PlayerDash>(),
                GetComponent<InputManager>(),
                GetComponent<PlayerAttack>()
            })
            {
                if (mb != null)
                    mb.enabled = false;
            }

            // 3) Fire the trigger into the Death state
            animator.SetTrigger(deathTrigger);

            // 4) Kick off our “wait out the clip then reload” coroutine
            StartCoroutine(ReloadAfterDeath());
        }

        private IEnumerator ReloadAfterDeath()
        {
            // default fallback
            float waitTime = 1f;

            // look up your animation clips for the exact length
            var clips = animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i].name == deathClipName)
                {
                    waitTime = clips[i].length;
                    break;
                }
            }

            // unscaled so it doesn't get cut off by any Time.timeScale fiddling
            yield return new WaitForSecondsRealtime(waitTime);

            LevelManager.ReloadLevel();
        }
    }
}
