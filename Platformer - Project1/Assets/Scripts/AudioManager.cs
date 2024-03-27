using FMODUnity;
using UnityEngine;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    static public AudioManager Instance;
    [Header ("Volume")]
    [Range(0f, 1f)]
    private float mastervolume = 1f;
    [Range(0f, 1f)]
    private float SFXvolume = 1f;
    [Range(0f, 1f)]
    private float Musicvolume = 1f;
    private Bus masterBus;
    private Bus sfxBus;
    private Bus musicBus;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
        masterBus = RuntimeManager.GetBus("bus:/");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        musicBus = RuntimeManager.GetBus("bus:/Music");
    }
    private void Start()
    {
        InitializeMusic(FMODEvents.Instance.Music);
        
    }
    private void Update()
    {
        masterBus.setVolume(mastervolume);
        sfxBus.setVolume(SFXvolume);
        musicBus.setVolume(Musicvolume);
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
    public void ChangeVolume(float v)
    {
        mastervolume = v;

    } public void ChangeMusic(float v)
    {
        Musicvolume = v;

    } public void ChangeSFX(float v)
    {
        SFXvolume = v;

    }


}
