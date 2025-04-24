using UnityEngine;

public class EnemyFeedbackManager : MonoBehaviour
{
    private EntityHealth     entityHealth;
    private SFXManager       sfxManager;

    private void Awake()
    {
        // Cache components
        entityHealth = GetComponent<EntityHealth>();
        sfxManager   = SFXManager.instance;

        // Subscribe health‐damage feedback
        if (entityHealth != null)
            entityHealth.onHealthChanged += OnEnemyDamaged;
    }

    private void OnDestroy()
    {
        // Unsubscribe everything
        if (entityHealth != null)
            entityHealth.onHealthChanged -= OnEnemyDamaged;
    }

    /// <summary>
    /// Called when the player takes damage.
    /// </summary>
    private void OnEnemyDamaged(int lastHp, int newHp)
    {
        sfxManager?.PlaySFX("Hit");
    }
    /// <summary>
    /// Called when the player dashes.
    /// </summary>
  
}