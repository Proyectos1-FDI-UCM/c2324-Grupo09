using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationComponent : MonoBehaviour
{
    Animator _animator;
    Transform _myTransform;
    Vector3 _originalSize;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _myTransform = transform;
        _originalSize = _myTransform.localScale;
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

    public void LookTo1D(int direction)
    {
        transform.localScale = new Vector3(Mathf.Sign(direction) * _originalSize.x, _originalSize.y, _originalSize.z);
    }
}
