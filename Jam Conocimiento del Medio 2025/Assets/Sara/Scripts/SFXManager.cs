using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager _instance;

    [SerializeField] private List<AudioSource> soundsList;

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        soundsList = new List<AudioSource>();
    }

    public void PlaySFX()
    {
        for(int i = 0; i < soundsList.Count; i++)
            soundsList[i].Play();
    }
}
