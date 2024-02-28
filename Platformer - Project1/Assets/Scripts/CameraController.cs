using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region references
    public GameObject VirtualCamera;
    [SerializeField]
    public CinemachineBrain _cinemachineBrain;
    private RefactoredCharacterController _characterController;
    #endregion
    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RefactoredCharacterController>() != null)
        {
            _characterController = collision.GetComponent<RefactoredCharacterController>();
            VirtualCamera.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<RefactoredCharacterController>() != null)
        {
            VirtualCamera.SetActive(false);
        }
    }
    #endregion
    private void Start()
    {
        //_cinemachineBrain = GetComponent<CinemachineBrain>();
    }
    private void FixedUpdate()
    {
        if (_characterController != null)
            _cinemachineBrain.m_DefaultBlend.m_Time = (Mathf.Abs(40 / _characterController.CharacterVelocity.x));
        //Debug.Log(_characterController.CharacterVelocity.x);

    }
}
