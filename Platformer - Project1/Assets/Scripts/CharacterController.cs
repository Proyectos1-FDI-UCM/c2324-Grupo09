using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private CharacterMovement _chMovement;
    private AnimationComponent _animComp;

    void Start()
    {
        _chMovement = GetComponent<CharacterMovement>();
        _animComp = GetComponentInChildren<AnimationComponent>();
    }
    
    void Update()
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


    void FixedUpdate()
    {
        int input = (int)Input.GetAxisRaw("Horizontal");
        if(input != 0)
        {
            transform.localScale = new Vector3(input * 1, 1, 1); 
        }
        _chMovement.Run(input);
        _animComp.UpdateXInput(input);
        _animComp.SetVelocityY(_chMovement.RByVel);
        _animComp.SetGrounded(_chMovement.Grounded);
        _animComp.SetSlide(_chMovement.Sliding);
    }
}
