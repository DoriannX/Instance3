using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Dash Settings")] [SerializeField]
        private float dashDistance = 3.0f;

        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float dashCooldown = 0.25f;

        [SerializeField]
        private float invulnerabilityDuration = 0.1f;

        private float lastDashTime;
        private bool isDashing = false;
        private bool isInvulnerable = false;

        private Rigidbody rb;
        private Vector3 movementInput;
        private Vector3 lastMoveDirection = Vector3.right;
        private Vector3 currentVel;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
        }

        private void Start()
        {
            AllowImmediateDash();
        }

        private void AllowImmediateDash()
        {
            lastDashTime = Time.time - dashCooldown - dashDuration;
        }
    
        public void HandleMovement(float currentSpeed)
        {
            if (!isDashing)
            {
                currentVel =  AddGravityToVelocity(movementInput * currentSpeed);

                if (movementInput != Vector3.zero)
                {
                    lastMoveDirection = movementInput;
                }
            }
        }

        private Vector3 AddGravityToVelocity(Vector3 velocity)
        {
            velocity.y = rb.linearVelocity.y;
            return velocity;
        }

        public void StartDash()
        {
            bool isDashOnCooldown = Time.time < lastDashTime + dashDuration + dashCooldown;
            if (isDashing || isDashOnCooldown)
            {
                return;
            }

            isInvulnerable = true;
            isDashing = true;
            lastDashTime = Time.time;
        }

        public void HandleDash()
        {
            if (isDashing)
            {
                currentVel = AddGravityToVelocity(lastMoveDirection.normalized * (dashDistance / dashDuration));
                CheckDashFinish();
            }
        }

        private void CheckDashFinish()
        {
            bool isDashFinished = Time.time > lastDashTime + dashDuration;
            if(isDashFinished)
            {
                isDashing = false;
            }
        }

        public void CheckVulnerability()
        {
            bool hasInvulnerabilityExpired = Time.time > lastDashTime + dashDuration + invulnerabilityDuration;
            if (isInvulnerable && hasInvulnerabilityExpired)
            {
                isInvulnerable = false;
            }
        }

        public void ApplyVelocity()
        {
            Vector3 currentVelWithGravity = currentVel;
            currentVelWithGravity.y = rb.linearVelocity.y;
            rb.linearVelocity = currentVelWithGravity;
        }

        public void SetMovementInput(Vector2 moveInput)
        {
            movementInput = moveInput;
        }
    }
}

/*

Here's a structured review of the `PlayerMovement.cs` file:
   
   ### General Issues
   1. Missing `FixedUpdate` for physics-based movement
   2. No input abstraction - direct use of `Input.GetAxisRaw` and `Input.GetKeyDown` makes testing difficult
   3. No serialized fields for input keys/axes names
   4. No XML documentation for public methods
   5. Mixing of movement and dash mechanics in one class (potential violation of Single Responsibility Principle)
   
   ### Fields
   - Consider making `dashDistance`, `dashDuration`, etc. readonly since they're configuration values
   - `currentVel` name is unclear - should be `currentVelocity`
   - Missing validation for serialized fields (should be positive values)
   
   ### Methods Review
   
   #### `HandleMovement`
   ```csharp
   // Issues:
   // 1. Should be in FixedUpdate since it deals with physics
   // 2. Missing input validation for currentSpeed
   // 3. Unnecessary allocation of Vector3 every frame
   public void HandleMovement(float currentSpeed)
   ```
   
   #### `GetMovementInput`
   ```csharp
   // Issues:
   // 1. Hardcoded input axes names
   // 2. No input normalization for diagonal movement
   // Suggested fix:
   private Vector3 GetMovementInput()
   {
       Vector3 movementValue = new Vector3(
           Input.GetAxisRaw("Horizontal"),
           0f,
           Input.GetAxisRaw("Vertical")
       );
       return movementValue.normalized;
   }
   ```
   
   #### Dash-related Methods
   ```csharp
   // Issues:
   // 1. State management is scattered across multiple methods
   // 2. Magic number in invulnerability calculation
   // 3. No events/callbacks for dash state changes
   ```
   
   ### Suggested Improvements
   
   1. Move to physics-based updates:
   ```csharp
   private void FixedUpdate()
   {
       HandleMovement(currentSpeed);
       HandleDash();
       ApplyVelocity();
       CheckVulnerability();
   }
   ```
   
   2. Add input configuration:
   ```csharp
   [Header("Input Settings")]
   [SerializeField] private string horizontalAxisName = "Horizontal";
   [SerializeField] private string verticalAxisName = "Vertical";
   [SerializeField] private KeyCode dashKey = KeyCode.Space;
   ```
   
   3. Add validation:
   ```csharp
   private void OnValidate()
   {
       dashDistance = Mathf.Max(0f, dashDistance);
       dashDuration = Mathf.Max(0.01f, dashDuration);
       dashCooldown = Mathf.Max(0f, dashCooldown);
       invulnerabilityDuration = Mathf.Max(0f, invulnerabilityDuration);
   }
   ```
   
   4. Consider splitting into separate components:
   - `PlayerInput`
   - `PlayerMovement`
   - `PlayerDash`
   
   ### Missing Features
   1. No events for state changes (dash start/end, invulnerability)
   2. No interpolation for smooth movement
   3. No collision handling during dash
   4. No dash direction visualization/feedback
   5. No movement speed configuration
   
   ### Performance Considerations
   1. Cache Input axis names as string IDs
   2. Avoid Vector3 allocations in tight loops
   3. Consider using `[SerializeField] private bool debug;` for debug logs
   
   ### Unity-specific Issues
   1. `GetComponent<Rigidbody>()` in Awake is correct, but consider caching component references in fields
   2. Missing proper cleanup in `OnDisable`/`OnDestroy`
   3. Consider using `[RequireComponent(typeof(Collider))]` if collision detection is needed
   
   Consider implementing these improvements based on your specific requirements and performance needs.

*/

/*

Please generate unit tests for this class.

   Use a clean and readable structure.

   Cover all public methods, including edge cases and invalid inputs.

   Use the appropriate testing framework (e.g., NUnit for Unity or xUnit/NUnit for standard C#).

   If mocking is needed, suggest or use a mocking framework like Moq.

   Ensure that each test is named clearly and describes what it's testing.

*/