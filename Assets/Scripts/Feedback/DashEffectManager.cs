using UnityEngine;

public class DashEffectManager : MonoBehaviour
{
    [SerializeField] private AudioClip dashSFX;
    private AudioSource audioSource;
    
    private DashGhostEffect ghostEffect;
    private PlayerDash playerDash;

    private void Awake()
    {
        // Cache required components.
        playerDash = GetComponent<PlayerDash>();
        ghostEffect = GetComponent<DashGhostEffect>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        
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
        if (dashSFX != null)
        {
            audioSource.PlayOneShot(dashSFX);
        }
        
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