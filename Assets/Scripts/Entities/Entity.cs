using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // --- Core Attributes ---
    [SerializeField] protected int hp;         // Current health points
    [SerializeField] protected int maxHp;      // Maximum health points
    [SerializeField] protected float speed;    // Movement speed

    // --- Weapon References ---
    [SerializeField] protected Weapon currentWeapon;  // The currently equipped weapon
    [SerializeField] protected RangeWeapon rangeWeapon; // Reference to a ranged weapon
    [SerializeField] protected MeleeWeapon meleeWeapon; // Reference to a melee weapon

    // --- Optional Health Component (delegation example) ---
    protected EntityHealth healthComponent;

    // --- Initialization ---
    protected virtual void Awake()
    {
        // Optionally initialize the EntityHealth component if available.
        healthComponent = GetComponent<EntityHealth>();

        // Ensure the entity starts with full health.
        hp = maxHp;
    }

    // --- Damage and Health Management ---

    /// <summary>
    /// Apply damage to the entity. If health falls to zero or below, triggers Die().
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    public virtual void TakeDamage(int damage)
    {
        hp -= damage;

        // Optionally: trigger hurt animations or update UI here.

        if (hp <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Heals the entity up to its maximum health.
    /// </summary>
    /// <param name="amount">Amount of health to restore</param>
    public virtual void Heal(int amount)
    {
        hp = Mathf.Min(hp + amount, maxHp);
        // Optionally update health-related UI here.
    }

    // --- Weapon Handling ---

    /// <summary>
    /// Sets the current equipped weapon and updates the references for ranged or melee weapons accordingly.
    /// </summary>
    /// <param name="newWeapon">The new weapon to equip</param>
    public virtual void SetCurrentWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        if(newWeapon is RangeWeapon)
        {
            rangeWeapon = (RangeWeapon)newWeapon;
        }
        else if(newWeapon is MeleeWeapon)
        {
            meleeWeapon = (MeleeWeapon)newWeapon;
        }
        
        // Optionally: trigger an update in UI to reflect weapon change.
    }

    // --- Death Handling ---

    /// <summary>
    /// Abstract method for handling an entity’s death.
    /// Each subclass should provide its implementation (e.g., play a death animation, notify managers, etc.).
    /// </summary>
    public abstract void Die();
}
