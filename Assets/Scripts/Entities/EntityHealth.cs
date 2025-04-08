using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [SerializeField] private int maxHp;
    [SerializeField] private int hp;

    private Entity entity;

    private void Awake()
    {
        // Set starting health to maximum.
        hp = maxHp;
        // Reference the Entity component on the same GameObject.
        entity = GetComponent<Entity>();
    }

    /// <summary>
    /// Applies damage to this entity. If health falls to zero or below, calls Die().
    /// </summary>
    /// <param name="damage">Amount of damage to apply.</param>
    public void TakeDamage(int damage)
    {
        hp -= damage;
        
        // TODO: Optionally notify UI or play hurt animations.
        
        if (hp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heals the entity up to the maximum health.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        // TODO: Optionally update health-related UI.
    }

    /// <summary>
    /// Handles the death of the entity.
    /// </summary>
    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        // For now, simply destroy the entity.
        Destroy(gameObject);
        
        // TODO: In a more complete implementation you might notify managers,
        // TODO: play death animations, or trigger game over events.
    }
}