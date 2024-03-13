using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    static public AudioManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
         RuntimeManager.PlayOneShot(sound, worldPos);
    }
}
