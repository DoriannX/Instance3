using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    [field: SerializeField] public int Hp { get; private set; }
    
    public UnityEvent<int> OnDamageTaken;

    private void Awake()
    {
        Hp = maxHp;
        OnDamageTaken ??= new UnityEvent<int>();
    }

    public void TakeDamage(int damage)
    {
        Hp -= damage;
        
        OnDamageTaken.Invoke(damage);

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        Hp = Mathf.Min(Hp + amount, maxHp);
    }

    public void IncreaseMaxHp(int amount)
    {
        maxHp += amount;
        Hp += amount;
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject);
    }

    public void SetMaxHp(int amount)
    {
        maxHp = amount;
        Hp = maxHp;
    }
}