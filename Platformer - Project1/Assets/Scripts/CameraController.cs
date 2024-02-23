using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region references
    public GameObject VirtualCamera;
    #endregion
    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RefactoredCharacterController>() != null)
        {
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
}
