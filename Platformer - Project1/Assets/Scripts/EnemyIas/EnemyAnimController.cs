using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void BlueImpTransition()
    {
        _animator.SetBool("Spin", true);
    }
    public void BeneActivate()
    {
        _animator.SetBool("Fly", true);
    }
}
