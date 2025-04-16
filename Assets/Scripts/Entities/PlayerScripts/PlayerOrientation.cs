using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities.PlayerScripts
{
    public class PlayerOrientation : MonoBehaviour
    {
        private Camera playerCamera;
        [SerializeField] private LayerMask groundLayer;

        private Vector3 rightStickInput;
        private Transform playerTransform;
        
        private void Awake()
        {
            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            playerTransform = transform;
        }
        
        private void Update()
        {
            Vector3 targetPosition = GetLookTargetPosition();
            if (targetPosition != Vector3.zero)
            {
                LookAtTarget(targetPosition);
            }
        }
        
        private Vector3 GetLookTargetPosition()
        {
            if (Gamepad.current != null)
            {
                if (rightStickInput.sqrMagnitude > 0.1f)
                {
                    return playerTransform.position + rightStickInput * 100f;
                }
            }
            else
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
                {
                    return hit.point;
                }
            }
            
            
            return Vector3.zero;
        }
        
        private void LookAtTarget(Vector3 targetPosition)
        {
            Vector3 lookDirection = targetPosition - transform.position;
            lookDirection.y = 0; // Keep the player oriented horizontally
            
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }
        }
        
        public void SetRightStickInput(Vector3 input)
        {
            rightStickInput = input;
        }
    }
}