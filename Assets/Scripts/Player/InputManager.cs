using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputManager : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private void Awake()
        {
            // If not assigned in inspector
            if (playerMovement == null)
                playerMovement = GetComponent<PlayerMovement>();
        }

        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 moveInput = Vector3.zero;
            moveInput.Set(input.x, 0, input.y);
            playerMovement.SetMovementInput(moveInput);
        }

        public void OnDashPerformed(InputAction.CallbackContext context)
        {
            playerMovement.StartDash();
        }
    }
}