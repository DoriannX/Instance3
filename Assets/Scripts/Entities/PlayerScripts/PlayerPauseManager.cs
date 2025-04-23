using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

namespace Entities.PlayerScripts
{
    public class PlayerPauseManager : MonoBehaviour
    {
        [SerializeField] private PauseMenu pauseMenu;

        private void Awake()
        {
            Assert.IsNotNull(pauseMenu);
        }
        
        public void TogglePauseInput(InputAction.CallbackContext ctx)
        {
            if (!ctx.started)
            {
                return;
            }
            Debug.Log("Pause Game");
            pauseMenu.TogglePause();
        }
    }
}