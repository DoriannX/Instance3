using System;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [SerializeField] private int maxHp = 30;
    [field: SerializeField] public int Hp { get; private set; }
    
    // Expose max health as a property with a private set.
    public int MaxHealth { get; private set; }
    
    private Entity entity;
    public event Action onDeath;
    public event Action<int, int> onHealthChanged;

    private void Awake()
    {
        // Set starting health and max health.
        Hp = maxHp;
        MaxHealth = maxHp;
        // Reference the Entity component on the same GameObject.
        entity = GetComponent<Entity>();
    }

    public void TakeDamage(int damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);
        onHealthChanged?.Invoke(Hp, MaxHealth);
        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        Hp = Mathf.Min(Hp + amount, MaxHealth);
        onHealthChanged?.Invoke(Hp, MaxHealth);
    }

    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        MaxHealth = maxHp;
        Hp += amount;
    }

    public void Die()
    {
        onDeath?.Invoke();
        Debug.Log($"{gameObject.name} has died.");
        onDeath?.Invoke();
        // TODO: Further implementation for death (animations, notifications, etc.)
    }
    
    public void SetMaxHp(int amount)
    {
        maxHp = amount;
        MaxHealth = amount;
        Hp = MaxHealth;
    }
}