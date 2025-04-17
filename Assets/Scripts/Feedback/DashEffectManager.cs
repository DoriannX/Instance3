using UnityEngine;

public class DashEffectManager : MonoBehaviour
{       
    private DashGhostEffect ghostEffect;
    private PlayerDash playerDash;

    private void Awake()
    {
        // Cache required components.
        playerDash = GetComponent<PlayerDash>();
        ghostEffect = GetComponent<DashGhostEffect>();              
        
        // Subscribe to the dash event.
        if (playerDash != null)
        {
            playerDash.OnDash += OnDashTriggered;
        }
    }

    private void OnDestroy()
    {
        if (playerDash != null)
        {
            playerDash.OnDash -= OnDashTriggered;
        }
    }

    private void OnDashTriggered()
    {
        // Play dash SFX.
        SFXManager.instance?.PlaySFX("Dash");

        // Trigger the ghost effect.
        if (ghostEffect != null)
        {
            ghostEffect.TriggerGhost();
        }
        
        // Trigger the screen blink effect.
        if (ScreenBlinkEffect.Instance != null)
        {
            ScreenBlinkEffect.Instance.Blink();
        }
    }
}