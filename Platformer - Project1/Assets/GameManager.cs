using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int FPS;
    
    void Start()
    {
        Application.targetFrameRate = FPS;
    }

}
