using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScriptMenu : MonoBehaviour
{
    [SerializeField]
    GameObject _optionsMenu;
    [SerializeField]
    GameObject _mainMenu;
    public bool _VFXEnabled = true;
    [SerializeField]
    Toggle _vfxtoggle;
    GameManager _gameManager;
    EventSystem _eventSystem;
    [SerializeField]
    GameObject _backButton;
    ParticleManager _particleManager;
     float soundLevel;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _optionsMenu.SetActive(false);
        
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    public void PressedLevel1()
    {
        SceneManager.LoadScene(1);   
    }
    public void PressedLevel2()
    {
        SceneManager.LoadScene(2);
    }
    public void PressedExit()
    {
        Application.Quit();
    }
    public void PressedOptions() 
    {
        _optionsMenu.SetActive(true);
        _mainMenu.SetActive(false);
       
        _eventSystem.SetSelectedGameObject(_backButton);
    }
    public void PressedBack() 
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
    }
    public void PressedVFX() 
    {
        if (_VFXEnabled == true) 
        {
        _VFXEnabled=false;
        }
        else 
        {
        _VFXEnabled=true;
        }
        
        _gameManager.EnableParticle();
        
    }
  
}
