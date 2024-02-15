using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterMovement _chMovement;
    private AnimationComponent _animComp;
    //Valor que te dará el input para saber en qué dirección correr
    int xInput;
    void Start()
    {
        _chMovement = GetComponent<CharacterMovement>();
        _animComp = GetComponentInChildren<AnimationComponent>();
    }
    public void JumpDown() 
    {
        _chMovement.JumpPressed();
    }
    public void SlideDown()
    {
        _chMovement.SlidePressed();
    }
    public  void RunDown(float move)
    {
        xInput = (int) move;

    }
    public void JumpUp()
    {
        _chMovement.JumpReleased();
    }
    public void SlideUp()
    {
        _chMovement.SlideReleased();
    }
    public void RunUp(float move)
    {
        xInput = (int)move;

    }

    /*void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _chMovement.JumpPressed();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            _chMovement.JumpReleased();
        }

        if (Input.GetButtonDown("Slide"))
        {
            _chMovement.SlidePressed();
        }
        if (Input.GetButtonUp("Slide"))
        {
            _chMovement.SlideReleased();
        }
    }
    */


    void FixedUpdate()
    {
      //  int input = (int)Input.GetAxisRaw("Horizontal");
        if(xInput != 0)
        {
            transform.localScale = new Vector3(xInput * 1, 1, 1); 
        }
        _chMovement.Run(xInput);
        _animComp.UpdateXInput(xInput);
        _animComp.SetVelocityY(_chMovement.RByVel);
        _animComp.SetGrounded(_chMovement.Grounded);
        _animComp.SetSlide(_chMovement.Sliding);
    }
}
