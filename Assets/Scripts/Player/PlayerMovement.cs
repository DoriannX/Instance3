using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Dash")]
    [SerializeField] private float dashDistance = 3.0f;
    [SerializeField] private float dashCooldown = 0.25f;
    private float lastDashTime;

    private Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 lastMoveDirection = Vector3.right; // Default dash direction

    // A reference to the Entity (or Player) component, to use the unified speed.
    private Entity entity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        entity = GetComponent<Entity>();
    }

    public void HandleMovement()
    {
        // --- Read input ---
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        // Create a movement vector on the XZ plane.
        movementInput = new Vector3(horizontal, 0f, vertical).normalized;

        // --- Apply movement ---
        // Use the speed value defined in the Entity component.
        rb.linearVelocity = movementInput * entity.Speed;

        // --- Store last movement direction for dash ---
        if (movementInput != Vector3.zero)
        {
            lastMoveDirection = movementInput;
        }

        // --- Dash Input ---
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            Dash();
        }
    }

    private void Dash()
    {
        // Dash in the last direction the player moved.
        rb.MovePosition(rb.position + lastMoveDirection * dashDistance);
        lastDashTime = Time.time;
        
        // TODO: Add dash SFX, visual effects, etc.
    }
}