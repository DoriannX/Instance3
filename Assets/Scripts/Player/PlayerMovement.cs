using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashDistance = 3.0f;      // Distance to dash
    [SerializeField] private float dashDuration = 0.2f;      // Total time for the dash burst
    [SerializeField] private float dashCooldown = 0.25f;     // Cooldown between dashes
    [SerializeField] private float invulnerabilityDuration = 0.1f; // Duration of invulnerability (a fraction of dashDuration)

    private float lastDashTime;
    private bool isDashing = false;
    private bool isInvulnerable = false;

    private Rigidbody rb;
    private Vector3 movementInput;
    private Vector3 lastMoveDirection = Vector3.right; // Default dash direction

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    /// <summary>
    /// Handles player movement.
    /// Receives the current speed from the Player (Entity.Speed) to apply movement.
    /// </summary>
    /// <param name="currentSpeed">Movement speed passed by the Player.</param>
    public void HandleMovement(float currentSpeed)
    {
        // Only process normal movement if not currently dashing.
        if (!isDashing)
        {
            // --- Read input ---
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            // Create a movement vector on the XZ plane.
            movementInput = new Vector3(horizontal, 0f, vertical).normalized;

            // --- Apply normal movement using the provided speed ---
            rb.linearVelocity = movementInput * currentSpeed;

            // --- Store last movement direction for dash ---
            if (movementInput != Vector3.zero)
            {
                lastMoveDirection = movementInput;
            }
        }

        // --- Dash Input ---
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        // Begin dash
        isDashing = true;
        isInvulnerable = true;
        
        // Calculate dash velocity to cover dashDistance in dashDuration.
        // (dashDistance/dashDuration) gives the required speed.
        Vector3 dashVelocity = lastMoveDirection.normalized * (dashDistance / dashDuration);
        
        // Override current velocity.
        rb.linearVelocity = dashVelocity;
        
        // Wait for invulnerability period.
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
        
        // Wait for remaining dash duration.
        yield return new WaitForSeconds(dashDuration - invulnerabilityDuration);
        
        // End dash: reset velocity so that normal movement can resume.
        rb.linearVelocity = Vector3.zero;
        isDashing = false;
        lastDashTime = Time.time;
    }

    // Optionally, you could expose a public property for invulnerability
    public bool IsInvulnerable => isInvulnerable;
}
