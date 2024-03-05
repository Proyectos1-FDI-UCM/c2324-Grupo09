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
    private Transform Circle;
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
        //CameraAnimator = FindObjectOfType<Animator>(CameraAnimator);
        //Circle = FindObjectOfType<Transform>(Circle);

    }
    public void OnDie(Vector3 playerPosition)
    {
        Circle.transform.position = playerPosition;
        CameraAnimator.SetBool("FadeOut", true);
        CameraAnimator.SetBool("FadeOut", false);
    }

}
