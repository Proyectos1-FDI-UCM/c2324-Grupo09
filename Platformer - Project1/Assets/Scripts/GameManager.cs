using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int FPS;
    
    [SerializeField]
    private ParticleManager particleManager;
    private GameObject FadeCanvas;
    private Transform Circle;
    private Transform DeathImage;
    private Animator CameraAnimator;
    private TextMeshProUGUI _deathCountText;
    private float masterVolume = 1f;
    private float musicVolume = 1f;
    private float sfxVolume = 1f;
    private int _deathCount = 0;
    
    private RefactoredCharacterController charController;
    private CameraController cameraController;
    private PauseScript _pause;
    static public GameManager Instance;
    private bool isEnabled=true;
    private bool _blockDeadCount = false;

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
        Application.targetFrameRate = FPS;
        //FadeCanvas = FindObjectOfType<Canvas>().gameObject;
        AfterLoad();

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
        if (!_blockDeadCount)
        {
            _deathCount++;
            _blockDeadCount = true;
            StartCoroutine(blockDeadCount());
        }

        SetDeathCountPosition();
        CameraAnimator.SetTrigger("FadeOut");
        cameraController.DespawnEnemiesOnRoomExit();
        boss?.PlayerDied();

    }

    IEnumerator blockDeadCount()
    {
        yield return new WaitForSeconds(1.5f);
        _blockDeadCount = false;
    }

    public void SetCirclePosition(Vector3 position)
    {
        Circle.transform.position = position;
    }

    public void SetDeathCountPosition()
    {
        DeathImage.transform.position = new Vector3(charController.transform.position.x, cameraController.transform.position.y + 60,0);
        _deathCountText.text = _deathCount.ToString();
        _deathCountText.transform.position = new Vector3(DeathImage.transform.position.x + 20, DeathImage.transform.position.y, 0);
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
    public void EnableParticle(bool toggle)
    {
        isEnabled = toggle;
    }                                         
    public void Check() 
    {
        particleManager= FindObjectOfType<ParticleManager>();
        if(particleManager != null)
            particleManager.enabled = (isEnabled);

    }
    public void AfterLoad() 
    {
        Application.targetFrameRate = FPS;
        FadeCanvas = FindObjectOfType<TeleportPlayer>()?.gameObject; 
        Circle = FadeCanvas?.transform.GetChild(0);
        DeathImage = FadeCanvas?.transform.GetChild(1);
        _deathCountText = FadeCanvas?.GetComponentInChildren<TextMeshProUGUI>();
        CameraAnimator = FadeCanvas?.GetComponent<Animator>();
        charController = FindObjectOfType<RefactoredCharacterController>();
        cameraController = null;
        particleManager = FindObjectOfType<ParticleManager>();
        boss = FindObjectOfType<BossIA>();
        NumerateAllRooms();
        AudioManager.Instance.mastervolume = masterVolume;
        AudioManager.Instance.Musicvolume = musicVolume;
        AudioManager.Instance.SFXvolume = sfxVolume;
        Return();
        _pause= FindObjectOfType<PauseScript>();        
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
    public void SaveValue() 
    {
        int i;
        if (isEnabled) 
        {
            i = 1;
        }
        else 
        {
            i = 0;
        }
        SaveValues.instance.SavePrefs(masterVolume,musicVolume,sfxVolume, i);
    }
    public void Level1Completed(out bool complete) 
    {
        complete= SaveValues.instance.CheckLevel1();
    }
    public void Return() 
    {

        try
        {
            int i;
            SaveValues.instance.ReturnPrefs(out masterVolume, out musicVolume, out sfxVolume, out i);
            if (i == 0) { isEnabled = false; }
            else { isEnabled = true; }
        }
        catch
        {
            Debug.Log("SaveValues peta");
        }
    }
    public void ChangeValues(out float master, out float music,out float sfx,out  bool toggle) 
    {
        master = masterVolume;
        music = musicVolume;
        sfx = sfxVolume;
        toggle = isEnabled;
    }


}
