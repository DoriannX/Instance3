using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    // --- Core Attributes ---
    [SerializeField] protected float speed; // Movement speed

    // Public accessor so other components (like PlayerMovement) can read and modify the speed.
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    // --- Weapon References ---
    /*
    [SerializeField] protected Weapon currentWeapon;  // Currently equipped weapon
    [SerializeField] protected RangeWeapon rangeWeapon; // Reference to a ranged weapon
    [SerializeField] protected MeleeWeapon meleeWeapon; // Reference to a melee weapon
    */

    // --- Health Component ---
    protected EntityHealth healthComponent;

    protected virtual void Awake()
    {
        // Get the EntityHealth component attached to this GameObject.
        healthComponent = GetComponent<EntityHealth>();
    }

    // --- Weapon Handling ---

    /// <summary>
    /// Sets the current equipped weapon and updates the appropriate reference.
    /// </summary>
    /// <param name="newWeapon">The new weapon to equip.</param>
    /*
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

        // Optionally trigger a UI update here.
    }
    */
}