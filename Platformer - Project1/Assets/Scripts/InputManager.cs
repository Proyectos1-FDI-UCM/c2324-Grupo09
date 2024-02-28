using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;

public class InputManager : MonoBehaviour
{
    RefactoredCharacterController _characterController;
    [SerializeField]
    InputActionReference _jumpAction;
    [SerializeField]
    InputActionReference _slideAction;
    [SerializeField]
    InputActionReference _runAction;
    [SerializeField]
    InputActionReference _wallRunAction;
    bool[] abilities;
    //Usamos esto para el desbloqueo de habilidades. El primero es slide
    //El segundo es pogo, tercero walljump y cuarto wallrun
    
    // Start is called before the first frame update
    private void Start()
    {
        _characterController = FindObjectOfType<RefactoredCharacterController>();
        abilities = new bool[] { true, true, true, false };
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
    }
    private void OnEnable()
    {
        _jumpAction.action.Enable();
        _slideAction.action.Enable();
        _runAction.action.Enable();
        _wallRunAction.action.Enable();
    }
    private void OnDisable()
    {
        _jumpAction.action.Disable();
        _slideAction.action.Disable();
        _runAction.action.Disable();
        _wallRunAction.action.Disable();
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
    }
    private void JumpDown(InputAction.CallbackContext obj) 
    {
        _characterController.JumpDown();
    }
    private void SlideDown(InputAction.CallbackContext obj)
    {
       
            _characterController.SlideDown();
       
    }
    private void RunDown(InputAction.CallbackContext obj)
    {
        _characterController.RunDown(obj.ReadValue<float>());
    }
    private void WallRunDown(InputAction.CallbackContext obj)
    {
       
            _characterController.WallRunDown();
        
    }
    private void RunUp(InputAction.CallbackContext obj)
    {
        _characterController.RunUp(0);
    }
    private void SlideUp(InputAction.CallbackContext obj)
    {
        _characterController.SlideUp();
    }
    private void JumpUp(InputAction.CallbackContext obj)
    {
        _characterController.JumpUp();
    }
    private void WallrunUp(InputAction.CallbackContext obj)
    {
       _characterController.WallRunUp();
    }
   /* public void Unlock(int i)
    {
        abilities[3] = true;
        Debug.Log("true");
    }*/

    /* Update is called once per frame
    void Update()
    {
    }
    */
}
