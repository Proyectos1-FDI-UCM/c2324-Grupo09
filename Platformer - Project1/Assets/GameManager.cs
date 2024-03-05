using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int FPS;
    [SerializeField]
    private Animator CameraAnimator;
    [SerializeField]
    private GameObject Circle;
    [SerializeField]
    private Transform Player;
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

    }
    public void OnDie()
    {
        Circle.transform .position = Player.transform.position;
        CameraAnimator.SetBool("FadeOut", true);
        CameraAnimator.SetBool("FadeOut", false);
    }

}
