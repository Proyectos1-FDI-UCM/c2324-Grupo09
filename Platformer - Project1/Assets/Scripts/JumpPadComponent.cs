using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadComponent : MonoBehaviour
{
    #region references
    Transform _arrowTransform;
    #endregion

    #region parameters
    [SerializeField]
    [Range(0, 7)]
    [Tooltip("Defines launch angle in pi/4 radians.")]
    int _direction;

    float _angle;
    Vector2 _jumpDirection;
    #endregion
    private void Start()
    {
        _arrowTransform = transform.GetChild(0);
        _angle = _direction*Mathf.PI/4;
        _jumpDirection = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
        _arrowTransform.localRotation = Quaternion.Euler(0,0,_angle*180/MathF.PI-90);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RefactoredCharacterMovement player = collision.collider.GetComponent<RefactoredCharacterMovement>();
        if (player != null)
        {

        }
    }
}
