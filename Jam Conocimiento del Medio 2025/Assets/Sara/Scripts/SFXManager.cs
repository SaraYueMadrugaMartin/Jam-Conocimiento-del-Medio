using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlaySFX(string clipName, bool loop = false)
    {
        AudioClip clip = audioClips.Find(c => c != null && c.name == clipName);
        if (clip != null)
        {
            if (loop)
            {
                audioSource.clip = clip;
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(clip);
            }
        }
        else
        {
            Debug.LogWarning("SFX no encontrado: " + clipName);
        }
    }

    public void StopSFX(string clipName)
    {
        if (audioSource.clip != null && audioSource.clip.name == clipName && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}