using System.Collections;
using System.Collections.Generic;
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
    private void Start() 
    {
        _gameManager= FindObjectOfType<GameManager>();
    _eventSystem=FindObjectOfType<EventSystem>();
      
        particle.isOn=_gameManager.IsToggleEnabled();
    }

    public void OnPressedBackToMenu() 
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }
    public void OnPressedOptions()
    {
        _optionsMenu.SetActive(true);
        _mainMenu.SetActive(false);

       _eventSystem.SetSelectedGameObject(_backButton);
    }
    public void OnPressedResume()
    {
       Time.timeScale = 1.0f;
        _gameManager.Check();
       this.gameObject.SetActive(false);
    }
    public void OnPressedExitGame() 
    {
    Application.Quit();
    }
    public void OnPressedBack()
    {
        _mainMenu.SetActive(true);
        _optionsMenu.SetActive(false);
        _eventSystem.SetSelectedGameObject(_eventSystem.firstSelectedGameObject);
    }
    public void ChangeSliderValue(float master, float music ,float sfx) 
    {
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;    
    }
    public void ChangeToggleValue(bool booleano)
    {
        Debug.Log(booleano);
        particle.isOn=booleano;
      
    }
}
