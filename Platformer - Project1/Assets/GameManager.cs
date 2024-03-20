using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int FPS;
    private GameObject FadeCanvas;
    private Transform Circle;
    private Animator CameraAnimator;
    private RefactoredCharacterController charController;
    private CameraController cameraController;
    static public GameManager Instance;

    private BossIA boss;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    } 
        void Start()
    {
        Application.targetFrameRate = FPS;
        FadeCanvas = FindObjectOfType<Canvas>().gameObject;
        Circle = FadeCanvas.transform.GetChild(0);
        CameraAnimator = FadeCanvas.GetComponent<Animator>();
        charController = FindObjectOfType<RefactoredCharacterController>();
        cameraController = null;

        boss = FindObjectOfType<BossIA>();


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

}
