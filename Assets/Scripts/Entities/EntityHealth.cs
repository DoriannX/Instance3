using System;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    // --- Health Attributes ---
    [field : SerializeField] public int maxHp { get; private set; } = 30 ;
    [field: SerializeField] public int Hp { get; private set; }

    private Entity entity;
    private EnemyHealthBar enemyHealthBar;
    public event Action onDeath;

    private void Awake()
    {
        // Set starting health to maximum.
        Hp = maxHp;
        // Reference the Entity component on the same GameObject.
        entity = GetComponent<Entity>();
        enemyHealthBar = GetComponent<EnemyHealthBar>();
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        if (enemyHealthBar != null)
        {
            enemyHealthBar.UpdateHealthBar();
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