using System.Collections;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [SerializeField] private int maxHp = 100;
    [field: SerializeField] public int Hp { get; private set; }

    // --- Feedback Attributes ---
    [SerializeField] private AudioClip hitSound; // Designers can assign a hit sound here.
    [SerializeField] private float flashDuration = 0.1f;  // (Not used directly here, managed by DamageFlash)

    private AudioSource audioSource;
    private Entity entity;
    
    private void Awake()
    {
        // Set starting health to maximum.
        Hp = maxHp;
        entity = GetComponent<Entity>();

        // Setup AudioSource, add one if missing.
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    
    public void TakeDamage(int damage)
    {
        Hp -= damage;
        
        // Play hit sound.
        PlayHitSound();
        
        // Delegate the flash effect to the DamageFlash component if attached.
        DamageFlash flashComponent = GetComponent<DamageFlash>();
        if (flashComponent != null)
        {
            flashComponent.Flash();
        }
        
        // If this entity is the player, trigger the screen border flash.
        if (gameObject.CompareTag("Player") && ScreenBorderFlash.Instance != null)
        {
            ScreenBorderFlash.Instance.FlashBorder();
        }
        
        if (Hp <= 0)
        {
            Die();
        }
    }
    
    public void Heal(int amount)
    {
        Hp = Mathf.Min(Hp + amount, maxHp);
        // TODO: Optionally update health-related UI.
    }
    
    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        Hp += amount;  // Optionally, also heal the entity for the increased amount.
    }
    
    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
        // TODO: Further implementation for death (animations, notifications, etc.)
    }
    
    public void SetMaxHp(int amount)
    {
        maxHp = amount;
        Hp = maxHp;  // Optionally, also heal the entity to full health.
    }
    
    // Plays the hit sound if assigned.
    private void PlayHitSound()
    {
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }
    }
}
