using FMODUnity;
using UnityEngine;
using FMOD.Studio;

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
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}
