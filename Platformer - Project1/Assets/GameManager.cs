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

    }
    public void OnDie(Vector3 playerPosition)
    {
        FadeCanvas.SetActive(true);
        Circle.transform.position = playerPosition;
        CameraAnimator.SetTrigger("FadeOut");
    }
    public void PlayerTeleport()
    {
        charController.TeleportPlayer();
        //CharController.Dead = false;
    }

}
