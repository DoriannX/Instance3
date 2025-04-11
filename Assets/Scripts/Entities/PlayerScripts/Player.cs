using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerVulnerabilityManager))]
// [RequireComponent(typeof(PlayerAttack))]
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

    private PlayerVulnerabilityManager vulnerabilityManager;
    // private PlayerAttack attack;
    // private UI ui;

    protected override void Awake()
    {
        base.Awake();
        // Base Entity setup.
        healthComponent = GetComponent<EntityHealth>();

        // Player-specific component references
        movement = GetComponent<PlayerMovement>();
        vulnerabilityManager = GetComponent<PlayerVulnerabilityManager>();
        dash = GetComponent<PlayerDash>();
        // attack = GetComponent<PlayerAttack>();
        // ui = FindObjectOfType<UI>(); // Assuming UI is scene-based and singleton-style
    }

    private void Start()
    {
        // UpdateUI();
    }

    private void Update()
    {
        vulnerabilityManager.CheckVulnerability();

        // Future attack handling would go here:
        // attack.HandleAttack();
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
        // UpdateUI();
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

    /*
public void Interact(InteractableObject obj)
{
    obj.Interact(this);
}
*/

    public virtual float GetAmmoMultiplier() => ammoMultiplier;
    public virtual float GetCooldownMultiplier() => cooldownMultiplier;

    public virtual void SetAmmoMultiplier(float multiplier)
    {
        ammoMultiplier = multiplier;
        // UpdateUI();
    }

    public virtual void SetCooldownMultiplier(float multiplier)
    {
        cooldownMultiplier = multiplier;
        // UpdateUI();
    }

    /*
private void UpdateUI()
{
    if (ui != null)
        ui.Update(this);
}
*/
}