using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public static MusicManager instance { get; private set; }

    private AudioSource audioSource;
    [SerializeField] private List<MusicClip> audioClips = new();

    private void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic(string name)
    {
        MusicClip clip = audioClips.Find(x => x.name == name);

        if (clip.audioClip == null)
        {
            Debug.LogError($"Music clip with name {name} not found.");
            return;
        }

        audioSource.Stop();
        audioSource.clip = clip.audioClip;
        audioSource.Play();
    }
}

[System.Serializable]
public struct MusicClip
{
    public string name;
    public AudioClip audioClip;
}
