using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int FPS;
    
    [SerializeField]
    private ParticleManager particleManager;
    private GameObject FadeCanvas;
    private Transform Circle;
    private Animator CameraAnimator;
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    
    private RefactoredCharacterController charController;
    private CameraController cameraController;
    private PauseScript _pause;
    static public GameManager Instance;
    public bool isEnabled=true;

    private BossIA boss;
    
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);   
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    } 
        void Start()
    {
        FadeCanvas = FindObjectOfType<TeleportPlayer>()?.gameObject;
        Application.targetFrameRate = FPS;
        //FadeCanvas = FindObjectOfType<Canvas>().gameObject;
        Circle = FadeCanvas.transform.GetChild(0);
        CameraAnimator = FadeCanvas.GetComponent<Animator>();
        charController = FindObjectOfType<RefactoredCharacterController>();
        cameraController = null;
        particleManager = FindObjectOfType<ParticleManager>();
        boss = FindObjectOfType<BossIA>();
        _pause=FindObjectOfType<PauseScript>();


        NumerateAllRooms();
        
    }

    void NumerateAllRooms()
    {
        CameraController[] obs = FindObjectsOfType<CameraController>();

        for(int i= 0; i < obs.Length; i++)
        {
            obs[i].Id = i;
        }
    }

    public void UpdateCameraControllerReference(CameraController meow)
    {
        if(cameraController == null || cameraController.Id != meow.Id)
        {
            cameraController?.EraseRoom();
            cameraController = meow;
            cameraController.DrawRoom();
        }
    }
    public void OnDie(Vector3 playerPosition)
    {
       
      
        FadeCanvas.SetActive(true);
        SetCirclePosition(playerPosition);
        CameraAnimator.SetTrigger("FadeOut");
        cameraController.DespawnEnemiesOnRoomExit();
        boss?.PlayerDied();
    }

    public void SetCirclePosition(Vector3 position)
    {
        Circle.transform.position = position;
    }
    public void PlayerTeleport()
    {
        charController.TeleportPlayer();
        cameraController.SpawnEnemiesOnRoomEnter();
        DestryAfterTime[] _obj = FindObjectsOfType<DestryAfterTime>();
        foreach (DestryAfterTime block in _obj)
        {
            block.Reset();
        }
        //CharController.Dead = false;
    }
    public void EnableParticle()
    {
        if (isEnabled == true) 
        {
        isEnabled= false;
        }
        else
        {
        isEnabled= true;
        }
       

    }                                         
    public void Check() 
    {
        particleManager= FindObjectOfType<ParticleManager>();
        if (isEnabled == true) 
        {
            particleManager.enabled = true;
        }
        else  
        {
            particleManager.enabled = false;
        }
    }
    public void AfterLoad() 
    {
        Application.targetFrameRate = FPS;
        FadeCanvas = FindObjectOfType<Canvas>().gameObject;
        Circle = FadeCanvas.transform.GetChild(0);
        CameraAnimator = FadeCanvas.GetComponent<Animator>();
        charController = FindObjectOfType<RefactoredCharacterController>();
        cameraController = null;
        particleManager = FindObjectOfType<ParticleManager>();
        boss = FindObjectOfType<BossIA>();
        NumerateAllRooms();
        AudioManager.Instance.mastervolume = masterVolume;
        AudioManager.Instance.Musicvolume = musicVolume;
        AudioManager.Instance.SFXvolume = sfxVolume;
        _pause= FindObjectOfType<PauseScript>();
        
        _pause.ChangeSliderValue(masterVolume,musicVolume,sfxVolume);
        _pause.ChangeToggleValue(isEnabled);
        
        Check();
        
    }
    public void SaveMasterVolume(float volume) 
    {
        masterVolume = volume;  
    }
    public void SaveMusicVolume(float volume)
    {
        musicVolume=volume;
    }
    public void SaveSFXVolume(float volume)
    {
        sfxVolume = volume;
    } 
    public bool IsToggleEnabled() 
    {
        return isEnabled;
    }


}
