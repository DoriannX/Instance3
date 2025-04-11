using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour
{
    public static SFXManager instance { get; private set; }

    private AudioSource audioSource;
    [SerializeField] private List<SFXClip> audioClips = new();

    private void Awake()
    {
        instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySFX(string name)
    {
        SFXClip clip = audioClips.Find(x => x.name == name);
        if (clip.audioClip == null)
        {
            Debug.LogError($"SFX clip with name {name} not found.");
            return;
        }
        audioSource.PlayOneShot(clip.audioClip);
    }
}

[System.Serializable]
public struct SFXClip
{
    public string name;
    public AudioClip audioClip;
}
