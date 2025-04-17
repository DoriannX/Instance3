using UnityEngine;
using UnityEngine.Events;

public class PlayerFeedbackManager : MonoBehaviour
{
    private EntityHealth entityHealth;
    private PlayerDash playerDash;

    private DamageFlash damageFlash;
    private DashGhostEffect dashGhost;
    
    private SFXManager sfxManager;

    private void Awake()
    {
        entityHealth = GetComponent<EntityHealth>();
        playerDash = GetComponent<PlayerDash>();
        damageFlash = GetComponent<DamageFlash>();
        dashGhost = GetComponent<DashGhostEffect>();

        sfxManager = SFXManager.instance;

        // Subscribe to health and dash events.
        if(entityHealth != null)
            entityHealth.onDamageTaken.AddListener(OnPlayerDamaged);
        if(playerDash != null)
            playerDash.OnDash += OnPlayerDash;
    }

    private void OnDestroy()
    {
        if(entityHealth != null)
            entityHealth.onDamageTaken.RemoveListener(OnPlayerDamaged);
        if(playerDash != null)
            playerDash.OnDash -= OnPlayerDash;
    }

    /// <summary>
    /// Called when the player takes damage.
    /// </summary>
    /// <param name="damage">Damage amount.</param>
    private void OnPlayerDamaged(int damage)
    {
        sfxManager?.PlaySFX("Hit");
        
        if(damageFlash != null)
        {
            damageFlash.Flash();
        }
        
        if (ScreenBorderFlash.Instance != null)
        {
            ScreenBorderFlash.Instance.FlashBorder();
        }
    }
    
    private void OnPlayerDash()
    {
        sfxManager?.PlaySFX("Dash");
        
        if(dashGhost != null)
        {
            dashGhost.TriggerGhost();
        }
        
        if (ScreenBlinkEffect.Instance != null)
        {
            ScreenBlinkEffect.Instance.Blink();
        }
    }
}
