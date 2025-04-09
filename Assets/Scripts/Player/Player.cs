using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement))]
// [RequireComponent(typeof(PlayerAttack))]
    [RequireComponent(typeof(EntityHealth))]
    public class Player : Entity
    {
        // --- Player-specific attributes ---
        [SerializeField] private int chips = 0;
        [SerializeField] private float ammoMultiplier = 1.0f;
        [SerializeField] private float cooldownMultiplier = 1.0f;

        // --- References ---
        private PlayerMovement movement;
        // private PlayerAttack attack;
        // private UI ui;

        private void Awake()
        {
            // Base Entity setup
            healthComponent = GetComponent<EntityHealth>();

            // Player-specific component references
            movement = GetComponent<PlayerMovement>();
            // attack = GetComponent<PlayerAttack>();
            // ui = FindObjectOfType<UI>(); // Assuming UI is scene-based and singleton-style
        }

        private void Start()
        {
            // UpdateUI();
        }

        private void Update()
        {
            // Pass the speed from Entity to the movement component.
            movement.CheckVulnerability();

            // Future attack handling would go here:
            // attack.HandleAttack();
        }

        private void FixedUpdate()
        {
            movement.HandleDash();
            movement.HandleMovement(Speed);
            movement.ApplyVelocity();
        }

        public void AddChips(int amount)
        {
            chips += amount;
            // UpdateUI();
        }

        /*
    public void Interact(InteractableObject obj)
    {
        obj.Interact(this);
    }
    */

        public float GetAmmoMultiplier() => ammoMultiplier;
        public float GetCooldownMultiplier() => cooldownMultiplier;

        /*
    private void UpdateUI()
    {
        if (ui != null)
            ui.Update(this);
    }
    */
    }
}