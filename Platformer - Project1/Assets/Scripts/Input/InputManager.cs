using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static Unity.Collections.Unicode;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    GameObject _pauseMenu;
    RefactoredCharacterController _characterController;
    [SerializeField]
    InputActionReference _jumpAction;
    [SerializeField]
    InputActionReference _slideAction;
    [SerializeField]
    InputActionReference _runAction;
    [SerializeField]
    InputActionReference _wallRunAction;

    [SerializeField]
    InputActionReference _closeMenuAction;
    [SerializeField]
    InputActionReference _openCloseMenuAion;

    GameObject _spawnMenuPrefab;
    GameObject _spawnMenu;

    bool _blockInput = false;

    bool[] abilities;
    //Usamos esto para el desbloqueo de habilidades. El primero es slide
    //El segundo es pogo, tercero walljump y cuarto wallrun
    
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(FindObjectOfType<PauseScript>()?.gameObject);
        //_pauseMenu = FindObjectOfType<PauseScript>()?.gameObject;
        _characterController = FindObjectOfType<RefactoredCharacterController>();
        abilities = new bool[5];

        _spawnMenuPrefab = Resources.Load<GameObject>("_AbilityUnlockExplanationScreen");
    }
    void Awake()
    {
        _jumpAction.action.performed += JumpDown;
        _wallRunAction.action.performed += WallRunDown;
        _slideAction.action.performed += SlideDown;
        _runAction.action.performed += RunDown;
        _jumpAction.action.canceled += JumpUp;
        _wallRunAction.action.canceled += WallrunUp;
        _slideAction.action.canceled += SlideUp;
        _runAction.action.canceled += RunUp;
        _closeMenuAction.action.performed += CloseMenu;
        _openCloseMenuAion.action.performed += OpenCloseMenu;
    }
    private void OnEnable()
    {
        _jumpAction.action.Enable();
        _slideAction.action.Enable();
        _runAction.action.Enable();
        _wallRunAction.action.Enable();
        _closeMenuAction.action.Enable();
        _openCloseMenuAion.action.Enable();
    }
    private void OnDisable()
    {
        _jumpAction.action.Disable();
        _slideAction.action.Disable();
        _runAction.action.Disable();
        _wallRunAction.action.Disable();
        _closeMenuAction.action.Disable();
        _openCloseMenuAion.action.Disable();
    }
    private void OnDestroy()
    {
        _jumpAction.action.performed -= JumpDown;
        _wallRunAction.action.performed -= WallRunDown;
        _slideAction.action.performed -= SlideDown;
        _runAction.action.performed -= RunDown;
        _jumpAction.action.canceled -= JumpUp;
        _slideAction.action.canceled -= SlideUp;
        _wallRunAction.action.canceled -= WallrunUp;
        _runAction.action.canceled -= RunUp;
        _closeMenuAction.action.canceled -= CloseMenu;
        _openCloseMenuAion.action.canceled -=OpenCloseMenu;
    }

    private void CloseMenu(InputAction.CallbackContext obj)
    {
        if (_blockInput)
        {
            _blockInput = false;
            Destroy(_spawnMenu);
        }
    }
    private void OpenCloseMenu(InputAction.CallbackContext obj)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        if (_pauseMenu.activeSelf) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
    private void JumpDown(InputAction.CallbackContext obj) 
    {
        if (!_blockInput)
            _characterController.JumpDown();
    }
    private void SlideDown(InputAction.CallbackContext obj)
    {
        if (!_blockInput)
            _characterController.SlideDown();
        //obj.InputControl
       
    }
    private void RunDown(InputAction.CallbackContext obj)
    {
        if (!_blockInput)
            _characterController.RunDown(obj.ReadValue<float>());
    }
    private void WallRunDown(InputAction.CallbackContext obj)
    {
        if (!_blockInput)
            _characterController.WallRunDown();
        
    }
    private void RunUp(InputAction.CallbackContext obj)
    {
        if (!_blockInput)
            _characterController.RunUp(0);
    }
    private void SlideUp(InputAction.CallbackContext obj)
    {
        if (!_blockInput)
            _characterController.SlideUp();
    }
    private void JumpUp(InputAction.CallbackContext obj)
    {
        if(!_blockInput)
            _characterController.JumpUp();
    }
    private void WallrunUp(InputAction.CallbackContext obj)
    {
        if (!_blockInput)
            _characterController.WallRunUp();
    }

    

    public void SpawnUnlockMenu(int i)
    {
        _characterController.JumpUp();
        _characterController.SlideUp();
        _characterController.WallRunUp();
        _characterController.RunUp(0);

        _blockInput = true;
        _spawnMenu = Instantiate(_spawnMenuPrefab);
        _spawnMenu.GetComponent<SpawnMenuComponent>().SetMenu(i);
    }
}
