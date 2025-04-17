using System;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [field : SerializeField] public int maxHp { get; private set; } = 30 ;
    [field: SerializeField] public int Hp { get; private set; }
    public event Action onDeath;
    public event Action<int, int> onHealthChanged;

    private void Awake()
    {
        // Set starting health and max health.
        Hp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        Hp = Mathf.Max(Hp - damage, 0);
        onHealthChanged?.Invoke(Hp, maxHp);
        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        Hp = Mathf.Min(Hp + amount, maxHp);
        onHealthChanged?.Invoke(Hp, maxHp);
    }

    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        Hp += amount;
        onHealthChanged?.Invoke(Hp, maxHp);
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
        Hp = maxHp;
        onHealthChanged?.Invoke(Hp, maxHp);
    }
}