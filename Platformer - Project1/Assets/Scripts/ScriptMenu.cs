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
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    ParticleManager _particleManager;
     float soundLevel;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _optionsMenu.SetActive(false);
        try
        {
            AudioManager.Instance.InitializeMusic(FMODEvents.Instance.MenuMusic);
        }
        catch
        {
            Debug.Log("Mete AudioManager prefab UwU");
        }
        _eventSystem = FindObjectOfType<EventSystem>();
        _vfxtoggle.isOn= _gameManager.IsToggleEnabled();
    }

    public void PressedLevel1()
    {
        SceneManager.LoadScene(1);
        try
        {
            AudioManager.Instance.StopMusic(FMODEvents.Instance.MenuMusic);
        }
        catch
        {
            Debug.Log("Mete AudioManager prefab UwU");
        }

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
    public void ChangeSliderValue(float master, float music, float sfx)
    {
        masterSlider.value = master;
        musicSlider.value = music;
        sfxSlider.value = sfx;
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
