using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class JumpPadComponent : MonoBehaviour
{
    #region references
    Transform _arrowTransform;
    #endregion

    #region parameters
    [SerializeField]
    [Range(0, 7)]
    [Tooltip("Defines launch angle in pi/4 radians.")]
    [OnValueChanged("AimArrow")]
    int _direction;

    float _angle;
    Vector2 _jumpDirection;
    [SerializeField]
    float _padForce = 100f;
    #endregion

    private void Start()
    {
        try
        { 

            AudioManager.Instance.InitializeMusic(FMODEvents.Instance.Music);
        }
        catch
        {
            Debug.Log("Mete audiomanager porfaaa");
        }
        
        AimArrow();
    }

    private void AimArrow()
    {
        _arrowTransform = transform.GetChild(0);
        _angle = _direction * Mathf.PI / 4;
        _jumpDirection = new Vector2(Mathf.Cos(_angle), Mathf.Sin(_angle));
        _arrowTransform.localRotation = Quaternion.Euler(0, 0, _angle * 180 / MathF.PI - 90);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        RefactoredCharacterController player = collider.GetComponent<RefactoredCharacterController>();
        if (player != null)
        {
            AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Trampoline, this.transform.position);
            player.JumpPadContact(_jumpDirection, _padForce);
        }
    }
}
