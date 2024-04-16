using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
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
    Button _bossButton;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    ParticleManager _particleManager;
     float soundLevel;
    private float master, music, sfx;
    private bool toggle;
    private bool completed;
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
      
        _gameManager.Return();
        _gameManager.ChangeValues(out master, out music, out sfx, out toggle);
        _gameManager.Level1Completed(out completed);
        masterSlider.value = master;
        sfxSlider.value = sfx;
        musicSlider.value = music;
        _vfxtoggle.isOn = toggle;
        _optionsMenu.SetActive(false);
        _bossButton.interactable = completed;

    }

    public void PressedLevel1()
    {
        _gameManager.SaveValue();
        try
        {
            AudioManager.Instance.StopMusic(FMODEvents.Instance.MenuMusic);
        }
        catch
        {
            Debug.Log("Mete AudioManager prefab UwU");
        }
        SceneManager.LoadScene(4);
        
        
    }
    public void PressedLevel2()
    {
        _gameManager.SaveValue();
        SceneManager.LoadScene(2);
       
    }
    public void PressedExit()
    {
        _gameManager.SaveValue();
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
    public void PressedVFX(bool enable) 
    {
        _gameManager.EnableParticle(enable);
        
    }
  
}
