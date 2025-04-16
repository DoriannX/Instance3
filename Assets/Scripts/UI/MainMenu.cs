using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Scene index to load when the game starts.")]
    [SerializeField] private int startSceneIndex = 1;

    
    public void StartGame()
    {
        // Load the scene using the specified index.
        SceneManager.LoadScene(startSceneIndex);
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
