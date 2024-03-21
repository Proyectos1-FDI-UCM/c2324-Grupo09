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
    private void Start()
    {
        InitializeMusic(FMODEvents.Instance.Music);
    }
    private EventInstance musicEventInstance;
    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
         RuntimeManager.PlayOneShot(sound, worldPos);
    }
    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
    public EventInstance PlayMusic(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

}
