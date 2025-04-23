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

    public void TogglePause()
    {   
        Debug.Log("tried to pause");
        if (pauseMenu.activeSelf)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        } 
    }

    private void ToggleTimePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        ToggleTimePause(false);
    }

    private void PauseGame()
    {
        pauseMenu.SetActive(true);
        ToggleTimePause(true);
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
