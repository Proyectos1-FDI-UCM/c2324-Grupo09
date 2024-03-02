using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region references
    public GameObject VirtualCamera;
    [SerializeField]
    private CinemachineBrain _cinemachineBrain;
    private RefactoredCharacterController _characterController;
    [SerializeField]
    private EnemySpawner[] eSpawner;
    private Transform _spawnPoint;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<RefactoredCharacterController>() != null)
        {
            _characterController = collision.GetComponent<RefactoredCharacterController>();
            VirtualCamera.SetActive(true);
            SpawnEnemiesOnRoomEnter();
            _characterController.AssignSpawnPoint(_spawnPoint);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<RefactoredCharacterController>() != null)
        {
            VirtualCamera.SetActive(false);
            DespawnEnemiesOnRoomExit();
        }
    }
    #endregion
    private void Start()
    {
        _cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        _spawnPoint = GetComponentInChildren<RespawnComponent>().transform;
    }
    private void FixedUpdate()
    {
        if (_characterController != null)
            _cinemachineBrain.m_DefaultBlend.m_Time = Mathf.Min(Mathf.Abs(40 / _characterController.CharacterVelocity.magnitude), 1.5f);
        //Debug.Log(_characterController.CharacterVelocity.x);
    }

    private void SpawnEnemiesOnRoomEnter()
    {
        for(int i = 0; i<eSpawner.Length; i++)
        {
            eSpawner[i].Spawn();
        }
    }

    private void DespawnEnemiesOnRoomExit()
    {
        for (int i = 0; i < eSpawner.Length; i++)
        {
            eSpawner[i].DestroySpawnedEnemy();
        }
    }
}
