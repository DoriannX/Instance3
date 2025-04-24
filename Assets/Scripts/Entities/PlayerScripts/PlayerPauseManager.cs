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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseInput();
            }
        }

        public void TogglePauseInput()
        {
            pauseMenu.TogglePause();
        }
    }
}