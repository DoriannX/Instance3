using System;
using Entities.PlayerScripts;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerVulnerabilityManager))]
[RequireComponent(typeof(PlayerOrientation))]
[RequireComponent(typeof(EntityHealth))]
public class Player : Entity
{
    // --- Player-specific attributes ---
    [field: SerializeField] public int Chips { get; private set; } = 0;
    [SerializeField] private float ammoMultiplier = 1.0f;
    [SerializeField] private float cooldownMultiplier = 1.0f;

    // --- References ---
    private PlayerMovement movement;
    private PlayerDash dash;
    private PlayerOrientation orientation;

    private PlayerVulnerabilityManager vulnerabilityManager;

    protected override void Awake()
    {
        base.Awake();
        movement = GetComponent<PlayerMovement>();
        vulnerabilityManager = GetComponent<PlayerVulnerabilityManager>();
        dash = GetComponent<PlayerDash>();
        orientation = GetComponent<PlayerOrientation>();
    }

    private void Update()
    {
        vulnerabilityManager.CheckVulnerability();
    }

    public void SetMousePos(Vector3 mousePos)
    {
        orientation.SetMousePos(mousePos);
    }

    public void SetRightStickInput(Vector3 rightStickInput)
    {
        orientation.SetRightStickInput(rightStickInput);
    }

    public void StartDash()
    {
        dash.StartDash();
    }

    public void SetMovementInput(Vector3 moveInput)
    {
        movement.SetMovementInput(moveInput);
    }

    public virtual void AddChips(int amount)
    {
        Chips += amount;
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
    }

    public virtual void SetCooldownMultiplier(float multiplier)
    {
        cooldownMultiplier = multiplier;
    }
}