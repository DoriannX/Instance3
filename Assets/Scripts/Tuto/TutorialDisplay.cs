using InputDetection;
using UnityEngine;

namespace Tuto
{
    public class TutorialDisplay : MonoBehaviour
    {
        [Header("Movement Icon")]
        [SerializeField] private GameObject keyboardMovement;
        [SerializeField] private GameObject gamepadMovement;
        
        
        private void Awake()
        {
            InputDetector.OnInputModeChanged += UpdateDisplay;
            
            UpdateDisplay(InputMode.KeyboardMouse);
        }
        
        private void UpdateDisplay(InputMode inputMode)
        {
            keyboardMovement.SetActive(inputMode == InputMode.KeyboardMouse);
            gamepadMovement.SetActive(inputMode == InputMode.Gamepad);
        }

        private void OnDestroy()
        {
            InputDetector.OnInputModeChanged -= UpdateDisplay;
        }
    }
}