using UnityEngine;

public class PlayerFeedbackManager : MonoBehaviour
{
    private EntityHealth     entityHealth;
    private DamageFlash      damageFlash;
    private PlayerDash       playerDash;
    private DashGhostEffect  dashGhost;
    private SFXManager       sfxManager;

    private void Awake()
    {
        // Cache components
        entityHealth = GetComponent<EntityHealth>();
        damageFlash  = GetComponent<DamageFlash>();
        playerDash   = GetComponent<PlayerDash>();
        dashGhost    = GetComponent<DashGhostEffect>();
        sfxManager   = SFXManager.instance;

        // Subscribe health‐damage feedback
        if (entityHealth != null)
            entityHealth.onHealthChanged += OnPlayerDamaged;

        // Subscribe dash feedback
        if (playerDash != null)
            playerDash.OnDash += OnPlayerDash;
    }

    private void OnDestroy()
    {
        // Unsubscribe everything
        if (entityHealth != null)
            entityHealth.onHealthChanged -= OnPlayerDamaged;

        if (playerDash != null)
            playerDash.OnDash -= OnPlayerDash;
    }

    /// <summary>
    /// Called when the player takes damage.
    /// </summary>
    private void OnPlayerDamaged(int currentHp, int newHp)
    {
        if (currentHp <= 0)
            return; 
        
        sfxManager?.PlaySFX("PlayerTakeDamage");
        damageFlash?.Flash();
        ScreenBorderFlash.Instance?.FlashBorder();
    }

    /// <summary>
    /// Called when the player dashes.
    /// </summary>
    private void OnPlayerDash()
    {
        sfxManager?.PlaySFX("Dash");
        dashGhost?.TriggerGhost();
        ScreenBlinkEffect.Instance?.Blink();
    }
}