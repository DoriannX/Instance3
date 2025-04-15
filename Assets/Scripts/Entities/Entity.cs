using UnityEngine;
using System;

[RequireComponent(typeof(EntityHealth))]
public abstract class Entity : MonoBehaviour
{
    // --- Core Attributes ---
    [SerializeField] protected float speed; // Movement speed

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // --- Weapon References ---
    [Header("Weapon References")]
    [SerializeField] protected Weapon currentWeapon;  // Currently active weapon
    [SerializeField] protected MeleeWeapon meleeWeapon; // Melee weapon slot
    [SerializeField] protected RangeWeapon rangeWeapon; // Ranged weapon slot

    // Public getters for UI access.
    public Weapon CurrentWeapon => currentWeapon;
    public MeleeWeapon MeleeWeapon => meleeWeapon;
    public RangeWeapon RangeWeapon => rangeWeapon;
    
    // Event to signal that the equipped weapon has changed.
    public event Action<Weapon> OnWeaponChanged;

    // --- Health Component ---
    protected EntityHealth healthComponent;

    protected virtual void Awake()
    {
        healthComponent = GetComponent<EntityHealth>();
    }

    /// <summary>
    /// Sets the equipped weapon and updates the appropriate reference.
    /// Fires OnWeaponChanged for UI updates.
    /// </summary>
    /// <param name="newWeapon">The new weapon to equip.</param>
    public virtual void SetCurrentWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
        if (newWeapon is RangeWeapon)
        {
            rangeWeapon = (RangeWeapon)newWeapon;
        }
        else if (newWeapon is MeleeWeapon)
        {
            meleeWeapon = (MeleeWeapon)newWeapon;
        }
        // Notify listeners (like the UI) that the current weapon has changed.
        OnWeaponChanged?.Invoke(newWeapon);
    }
}