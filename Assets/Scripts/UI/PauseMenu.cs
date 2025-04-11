using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private void Start()
    {
        Time.timeScale = 1f;
        Assert.IsNotNull(pauseMenu, "Pause menu is not assigned in the inspector.");
    }

    public void PauseUI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (pauseMenu.activeSelf)
            {
                ResumeGame();
                ToggleTimePause(false);
            }
            else
            {
                PauseGame();
                ToggleTimePause(true);
            } 
        }        
    }

    private void ToggleTimePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
