using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Scene index to load when the game starts.")]
    [SerializeField] private int startSceneIndex = 1;

    [Header("UI Panels")]
    [Tooltip("Panel to display the Options (Settings) menu.")]
    [SerializeField] private GameObject optionsPanel;

    [Tooltip("Panel to display the Credits.")]
    [SerializeField] private GameObject creditsPanel;
    
    public void StartGame()
    {
        // Load the scene using the specified index.
        SceneManager.LoadScene(startSceneIndex);
    }
    
    public void OpenOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Options Panel is not assigned in the inspector!");
        }
    }
    
    public void CloseOptions()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
        }
    }
    
    public void OpenCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Credits Panel is not assigned in the inspector!");
        }
    }
    
    public void CloseCredits()
    {
        if (creditsPanel != null)
        {
            creditsPanel.SetActive(false);
        }
    }
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        // If running in the Unity Editor, stop playing.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running in a build, quit the application.
        Application.Quit();
#endif
    }
}
