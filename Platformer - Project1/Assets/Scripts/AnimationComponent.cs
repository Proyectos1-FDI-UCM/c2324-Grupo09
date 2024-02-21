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
        _originalSize = new Vector3(Mathf.Abs(_myTransform.localScale.x), _myTransform.localScale.y, _myTransform.localScale.z);
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
    public void SetPogoTr()
    {
        _animator.SetTrigger("PogoTr");
    }
    public void SetPogo(bool val)
    {
        _animator.SetBool("Pogo", val);
    }

    public void SetSlide(bool val)
    {
        _animator.SetBool("Slide", val);
    }

    public void SetWJ(bool val)
    {
        _animator.SetBool("WJ", val);
    }

    public void LookTo1D(int direction)
    {
        transform.localScale = new Vector3(Mathf.Sign(direction) * _originalSize.x, _originalSize.y, _originalSize.z);
    }
}
