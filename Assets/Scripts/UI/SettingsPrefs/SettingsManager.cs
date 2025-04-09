using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey("fullScreen"))
        {
            LoadToggleScreen();
        }
        else
        {
            SetToggleScreen();
        }

        SetFullScreen();

        MusicManager.instance.PlayMusic("MainMenu");
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetToggleScreen()
    {
        int toggle = fullscreenToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("fullScreen", toggle);
    }

    private void LoadToggleScreen()
    {
        fullscreenToggle.isOn = (PlayerPrefs.GetInt("fullScreen") != 0);

        SetToggleScreen();
    }

    public void SetFullScreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }

    private void Update()
    {        
        if(Input.GetKeyDown(KeyCode.Space)) SFXManager.instance.PlaySFX("Test");
        if(Input.GetKeyDown(KeyCode.Q)) MusicManager.instance.PlayMusic("InGame");
    }
}
