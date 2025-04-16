using System;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [SerializeField] private int maxHp = 30;
    [field: SerializeField] public int Hp { get; private set; }
    
    public int GetMaxHealth() => maxHp;

    private Entity entity;
    public event Action onDeath;
    public event Action<int, int> OnHealthChanged;

    private void Awake()
    {
        // Set starting health to maximum.
        Hp = maxHp;
        // Reference the Entity component on the same GameObject.
        entity = GetComponent<Entity>();
    }

    public void TakeDamage(int damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);
        OnHealthChanged?.Invoke(Hp, maxHp);
        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        Hp = Mathf.Min(Hp + amount, maxHp);
        OnHealthChanged?.Invoke(Hp, maxHp);
    }

    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
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
        Hp = maxHp;  // Optionally, also heal the entity to full health.
    }
}