using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private void Start()
    {
        MusicManager.instance.PlayMusic("MainMenu");
    }

    public void StartGame()
    {
        // Load the scene using the specified index.
        SceneManager.LoadScene("Merge");
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
