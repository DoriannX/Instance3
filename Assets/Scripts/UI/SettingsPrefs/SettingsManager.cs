using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private const string MusicVolumeKey = "musicVolume";
    private const string SFXVolumeKey = "SFXVolume";
    private const string FullScreenKey = "fullScreen";

    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        if (PlayerPrefs.HasKey(MusicVolumeKey) || PlayerPrefs.HasKey(SFXVolumeKey))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
            SetSFXVolume();
        }

        if (PlayerPrefs.HasKey(FullScreenKey))
        {
            LoadToggleScreen();
        }
        else
        {
            SetToggleScreen();
        }

        SetFullScreen();
    }

    public void SetMusicVolume()
    {
        float volume = Mathf.Clamp(musicSlider.value, 0.0001f, 1f);
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Clamp(sfxSlider.value, 0.0001f, 1f);
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat(MusicVolumeKey);
        sfxSlider.value = PlayerPrefs.GetFloat(SFXVolumeKey);

        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetToggleScreen()
    {
        int toggle = fullscreenToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt(FullScreenKey, toggle);
    }

    private void LoadToggleScreen()
    {
        fullscreenToggle.isOn = (PlayerPrefs.GetInt(FullScreenKey) != 0);

        SetToggleScreen();
    }

    private void SetFullScreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
}
