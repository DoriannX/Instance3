using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace InputDetection
{
    public class InputDetector : MonoBehaviour
    {
        private string currentControlScheme;
        
        public static event Action<InputMode> OnInputModeChanged; 

        void OnEnable()
        {
            InputUser.onChange += OnInputDeviceChange;
        }

        void OnDisable()
        {
            InputUser.onChange -= OnInputDeviceChange;
        }

        private void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {
                currentControlScheme = user.controlScheme.Value.name;

                if (currentControlScheme == "Gamepad")
                    Debug.Log("🎮 Manette détectée");
                else
                    Debug.Log("⌨️ Clavier/Souris détectés");
            }
            
            if (device is Gamepad)
            {
                OnInputModeChanged?.Invoke(InputMode.Gamepad);
            }
            else if (device is Keyboard || device is Mouse)
            {
                OnInputModeChanged?.Invoke(InputMode.KeyboardMouse);
            }
        }
    }

}