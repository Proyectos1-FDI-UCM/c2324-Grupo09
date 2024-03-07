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
    static private GameManager _instance; 
    static public GameManager Instance    
    {     
        get { return _instance; }    
    }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
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
        cameraController = FindObjectOfType<CameraController>();

    }
    public void OnDie(Vector3 playerPosition)
    {
        FadeCanvas.SetActive(true);
        SetCirclePosition(playerPosition);
        CameraAnimator.SetTrigger("FadeOut");
        cameraController.DespawnEnemiesOnRoomExit();
    }
    public void SetCirclePosition(Vector3 position)
    {
        Circle.transform.position = position;
    }
    public void PlayerTeleport()
    {
        charController.TeleportPlayer();
        cameraController.SpawnEnemiesOnRoomEnter();
        //CharController.Dead = false;
    }

}
