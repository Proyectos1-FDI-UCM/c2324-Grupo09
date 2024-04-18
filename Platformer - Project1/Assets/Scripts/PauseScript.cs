using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    EventSystem _eventSystem;
    [SerializeField]
    private GameObject _backButton;
    [SerializeField]
    private GameObject _mainMenu; 
    [SerializeField]
    private GameObject _optionsMenu;
    GameManager _gameManager;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Toggle particle;
    InputManager _inputManager;
    private float master, music, sfx;
    private bool enabled;
    private void Start() 
    {
        
        _gameManager= FindObjectOfType<GameManager>();
        _eventSystem=FindObjectOfType<EventSystem>();
        _gameManager.Return();
        _gameManager.ChangeValues(out master, out music, out sfx, out enabled);
       _inputManager = FindObjectOfType<InputManager>();
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;
        particle.isOn=enabled;
        _optionsMenu.SetActive(false);
       

    }

    public void OnPressedBackToMenu() 
    {
        Time.timeScale = 1.0f;
        _gameManager.SaveValue();
        SceneManager.LoadScene(0);
       
        
    }
    public void OnPressedOptions()
    {
        _optionsMenu.SetActive(true);
        _mainMenu.SetActive(false);

       _eventSystem.SetSelectedGameObject(_backButton);
    }
    public void OnPressedResume()
    {
        _inputManager.PauseEnable();
    }
    public void OnPressedExitGame() 
    {
    Application.Quit();
        _gameManager.SaveValue();
    }
    public void OnPressedBack()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        try
        {
            _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
        }
        catch{

        }
    }
    public void ChangeSliderValue(float master, float music ,float sfx) 
    {
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;    
    }
    public void ChangeToggleValue(bool booleano)
    {

        _gameManager.EnableParticle(booleano);
      
    }
    private void OnDisable()
    {
        OnPressedBack();
    }
}
