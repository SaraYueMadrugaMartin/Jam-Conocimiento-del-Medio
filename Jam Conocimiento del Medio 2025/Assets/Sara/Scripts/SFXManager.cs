using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager _instance;

    private void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }

    public void PlaySFX()
    {

    }
}
