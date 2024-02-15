using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterMovement _chMovement;
    //reference to animation component
    private AnimationComponent _animComp;
    //x value of the movement joystick
    int xInput;
    void Start()
    {
        _chMovement = GetComponent<CharacterMovement>();
        _animComp = GetComponentInChildren<AnimationComponent>();
    }

    //Triggers on pressing the jump key
    public void JumpDown() 
    {
        _chMovement.JumpPressed();
    }

    //Triggers on pressing the slide/pogo key
    public void SlideDown()
    {
        _chMovement.SlidePressed();
    }

    //Triggers on changing the value of the x movement joystick
    public  void RunDown(float move)
    {
        xInput = (int) move;

    }

    //Triggers on releasing jump button
    public void JumpUp()
    {
        _chMovement.JumpReleased();
    }

    //Triggers on releasing slide button
    public void SlideUp()
    {
        _chMovement.SlideReleased();
    }

    //triggers when the value of the x movement is equal to 0
    public void RunUp(float move)
    {
        xInput = (int)move;

    }


    //Runs 50 times per second
    void FixedUpdate()
    {
        //changes the direction the player is facing
        if(xInput != 0)
        {
            transform.localScale = new Vector3(xInput * 1, 1, 1); 
        }
        //activates the run action of character movement
        _chMovement.Run(xInput);

        //Updating animation parameters
        _animComp.UpdateXInput(xInput);
        _animComp.SetVelocityY(_chMovement.RByVel);
        _animComp.SetGrounded(_chMovement.Grounded);
        _animComp.SetSlide(_chMovement.Sliding);
    }
}
