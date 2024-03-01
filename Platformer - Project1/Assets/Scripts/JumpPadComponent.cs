using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadComponent : MonoBehaviour
{
    #region parameters
    [SerializeField]
    [Range(0, 7)]
    [Tooltip("Defines launch angle in pi/4 radians.")]
    int _direction;

    float _angle;
    Vector2 _JumpDirection;
    #endregion
    private void Start()
    {
        _angle = _direction*Mathf.PI/4;
        _JumpDirection = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RefactoredCharacterMovement player = collision.collider.GetComponent<RefactoredCharacterMovement>();
        if (player != null)
        {

        }
    }
}
