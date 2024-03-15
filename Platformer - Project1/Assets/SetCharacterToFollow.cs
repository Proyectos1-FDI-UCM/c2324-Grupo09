using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCharacterToFollow : MonoBehaviour
{
    CinemachineVirtualCamera _cinemachine;
    private void Start()
    {
        _cinemachine = GetComponent<CinemachineVirtualCamera>();
       _cinemachine.Follow= FindObjectOfType<RefactoredCharacterController>().gameObject.transform;
    }

}
