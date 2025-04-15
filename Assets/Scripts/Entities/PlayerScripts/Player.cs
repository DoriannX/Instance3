using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerVulnerabilityManager))]
[RequireComponent(typeof(EntityHealth))]
public class Player : Entity
{
    // --- Player-specific attributes ---
    [field: SerializeField] public int Chips { get; private set; } = 0;
    [SerializeField] private float ammoMultiplier = 1.0f;
    [SerializeField] private float cooldownMultiplier = 1.0f;

    // Events for updating UI
    public event System.Action<int> OnChipsChanged;

    // --- References ---
    private PlayerMovement movement;
    private PlayerDash dash;
    private PlayerVulnerabilityManager vulnerabilityManager;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
        vulnerabilityManager = GetComponent<PlayerVulnerabilityManager>();
        dash = GetComponent<PlayerDash>();
    }

    private void Update()
    {
        vulnerabilityManager.CheckVulnerability();
        // Future attack handling would go here.
    }

    public void StartDash()
    {
        dash.StartDash();
    }

    public void SetMovementInput(Vector3 moveInput)
    {
        movement.SetMovementInput(moveInput);
    }

    /// <summary>
    /// Adds chips to the player's total and notifies any subscribers.
    /// </summary>
    public virtual void AddChips(int amount)
    {
        Chips += amount;
        OnChipsChanged?.Invoke(Chips);
    }

    /// <summary>
    /// Switches the weapon between melee and ranged (if both are available).
    /// </summary>
    public void SwitchWeapon()
    {
        // If current weapon is melee and we have a ranged weapon, swap to ranged.
        if (currentWeapon is MeleeWeapon && rangeWeapon != null)
        {
            SetCurrentWeapon(rangeWeapon);
        }
        // Otherwise, if current weapon is ranged and we have a melee weapon, swap to melee.
        else if (currentWeapon is RangeWeapon && meleeWeapon != null)
        {
            SetCurrentWeapon(meleeWeapon);
        }
        else
        {
            Debug.LogWarning("Weapon switch not possible: one or both weapon slots are empty.");
        }
    }

    private void FixedUpdate()
    {
        dash.HandleDash();
        if (!dash.isDashing)
        {
            movement.HandleMovement(Speed);
        }
        movement.ApplyVelocity();
    }

    public virtual float GetAmmoMultiplier() => ammoMultiplier;
    public virtual float GetCooldownMultiplier() => cooldownMultiplier;

    public virtual void SetAmmoMultiplier(float multiplier)
    {
        ammoMultiplier = multiplier;
        // Optionally add UI update event
    }

    public virtual void SetCooldownMultiplier(float multiplier)
    {
        cooldownMultiplier = multiplier;
        // Optionally add UI update event
    }
}
