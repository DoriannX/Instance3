using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(Player))]
    public class InputManager : MonoBehaviour
    {
        private Player player;
        private void Awake()
        {
            // If not assigned in inspector
            if (player == null)
                player = GetComponent<Player>();
        }

        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Vector3 moveInput = Vector3.zero;
            moveInput.Set(input.x, 0, input.y);
            player.SetMovementInput(moveInput);
        }

        public void OnDashPerformed(InputAction.CallbackContext context)
        {
            if (!context.started)
            {
                return;
            }
            player.StartDash();
        }
    }
}