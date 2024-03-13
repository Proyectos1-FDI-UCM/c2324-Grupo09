using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScriptMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PressedLevel1()
    {
        SceneManager.LoadScene("Nivel Prueba Build");   
    }
    public void PressedLevel2()
    {
        SceneManager.LoadScene("Nivel Boss");
    }
    public void PressedExit()
    {
        Application.Quit();
    }
}
