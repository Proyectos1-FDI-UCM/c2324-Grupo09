using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void UpdateXInput(int x)
    {
        _animator.SetInteger("xMovement", x);
    }
    public void SetVelocityY(float y)
    {
        _animator.SetBool("FallingDown", (y <= 0));
    }
    public void SetGrounded(bool val)
    {
        _animator.SetBool("Grounded", val);
    }

    public void SetSlide(bool val)
    {
        _animator.SetBool("Slide", val);
    }
}
