using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
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
    
    public void OnLookControllerPerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        Vector3 lookInput = Vector3.zero;
        lookInput.Set(input.x, 0, input.y);
        Debug.Log(input);
        player.SetRightStickInput(lookInput);
    }

    public void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        player.StartDash();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }
               
        player.Interact();
    }

    public void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        player.Attack();
    }

    public void Previous(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        player.SwitchWeapon();
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        player.SwitchWeapon();
    }
}