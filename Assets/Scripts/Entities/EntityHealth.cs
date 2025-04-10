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

    public void TakeDamage(int damage)
    {
        hp -= damage;
        // TODO: Optionally notify UI or play hurt animations.
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        // TODO: Optionally update health-related UI.
    }

    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        hp += amount;  // Optionally, also heal the entity for the increased amount.
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
        // TODO: Further implementation for death (animations, notifications, etc.)
    }
}