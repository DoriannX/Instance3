using System;
using Entities.PlayerScripts;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerVulnerabilityManager))]
[RequireComponent(typeof(PlayerOrientation))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(EntityHealth))]
[RequireComponent(typeof(PlayerInteract))]
[RequireComponent(typeof(InputManager))]
public class Player : Entity
{
    // --- Player-specific attributes ---
    [field: SerializeField] public int Chips { get; private set; } = 0;
    [SerializeField] private float speedMultiplier = 1.0f;
    public bool hasKey = false;

    // Events for updating UI
    public event Action<int> onChipsChanged;

    // --- References ---
    private PlayerMovement movement;
    private PlayerDash dash;
    private PlayerInteract playerInteract;
    private PlayerOrientation orientation;

    private PlayerVulnerabilityManager vulnerabilityManager;
    private PlayerAttack playerAttack;
    // private UI ui;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
        vulnerabilityManager = GetComponent<PlayerVulnerabilityManager>();
        dash = GetComponent<PlayerDash>();
        orientation = GetComponent<PlayerOrientation>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInteract = GetComponent<PlayerInteract>();
        // ui = FindObjectOfType<UI>(); // Assuming UI is scene-based and singleton-style
    }

    private void Update()
    {
        vulnerabilityManager.CheckVulnerability();
    }
    
    public void SetRightStickInput(Vector3 rightStickInput)
    {
        orientation.SetRightStickInput(rightStickInput);
    }

    public void StartDash()
    {
        dash.StartDash();
    }

    public void Interact()
    {
        playerInteract.Interact();
    }

    public void Attack()
    {
        playerAttack.Attack();
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
        onChipsChanged?.Invoke(Chips);
    }

    /// <summary>
    /// Switches the weapon between melee and ranged (if both are available).
    /// </summary>
    public void SwitchWeapon()
    {
        playerAttack.SwitchWeapon();
    }

    private void FixedUpdate()
    {
        dash.HandleDash();
        if (!dash.isDashing)
        {
            movement.HandleMovement(Speed * speedMultiplier);
        }
        movement.ApplyVelocity();
    }

    public virtual float GetAmmoMultiplier() => playerAttack.GetAmmoMultiplier();
    public virtual float GetCooldownMultiplier() => playerAttack.GetCooldownMultiplier();
    public virtual float GetDamageMultiplier() => playerAttack.GetDamageMultiplier();
    public virtual float GetSpeedMultiplier() => speedMultiplier;

    public virtual void SetAmmoMultiplier(float multiplier)
    {
        playerAttack.SetAmmoMultiplier(multiplier);
    }

    public virtual void SetCooldownMultiplier(float multiplier)
    {
        playerAttack.SetCooldownMultiplier(multiplier);
    }

    public virtual void SetDamageMultiplier(float multiplier)
    {
        playerAttack.SetDamageMultiplier(multiplier);
    }

    public virtual void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }
}