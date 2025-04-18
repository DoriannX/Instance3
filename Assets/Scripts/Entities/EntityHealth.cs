using System;
using Entities;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [field : SerializeField] public int maxHp { get; private set; } = 30 ;
    [field: SerializeField] public int Hp { get; private set; }
    [field: SerializeField] public float hpMultiplier = 1.0f;
    public event Action onDeath;
    public event Action<Transform> onHit;
    public event Action<int, int> onHealthChanged;
    private InvincibilityManager invincibilityManager;

    private void Awake()
    {
        // Set starting health and max health.
        Hp = Mathf.RoundToInt(maxHp * hpMultiplier);
        invincibilityManager = GetComponent<InvincibilityManager>();
    }

    public void TakeDamage(int damage, Transform origin = null)
    {
        if (invincibilityManager != null && invincibilityManager.isInvulnerable)
        {
            Debug.Log($"{gameObject.name} is invulnerable and took no damage.");
            return; 
        }
        Hp = Mathf.Max(Hp - damage, 0);
        onHealthChanged?.Invoke(Hp, maxHp);
        onHit?.Invoke(origin);
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
    }

    public void SetMaxHp(int amount)
    {
        maxHp = amount;
        Hp = maxHp;
        onHealthChanged?.Invoke(Hp, maxHp);
    }

    public void UpgradeHealthMultiplier(float amount)
    {
        hpMultiplier = amount;
        maxHp = Mathf.RoundToInt(maxHp * hpMultiplier);
        Hp = Mathf.RoundToInt(Hp * hpMultiplier);
    }
}