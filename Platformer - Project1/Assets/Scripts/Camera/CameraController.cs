using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region references
    [HideInInspector]
    public int Id;
    public GameObject VirtualCamera;
    [SerializeField]
    private CinemachineBrain _cinemachineBrain;
    private RefactoredCharacterController _characterController;
    [SerializeField]
    private EnemySpawner[] eSpawner;
    [SerializeField]
    private GameObject[] setActivePlatforms;
    private Transform _spawnPoint;
    #endregion

    #region methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _characterController = collision.GetComponent<RefactoredCharacterController>();
        if (_characterController != null)
        {
            GameManager.Instance.UpdateCameraControllerReference(this);
        }
    }

    public void DrawRoom()
    {
        foreach (GameObject platform in setActivePlatforms)
        {
            platform.SetActive(true);
        }
        VirtualCamera.SetActive(true);
        SpawnEnemiesOnRoomEnter();
        _characterController.AssignSpawnPoint(_spawnPoint);
    }

    public void EraseRoom()
    {
        VirtualCamera.SetActive(false);
        DespawnEnemiesOnRoomExit();
    }

    #endregion
    private void Start()
    {
        _cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        _spawnPoint = GetComponentInChildren<RespawnComponent>()?.transform;
    }
    private void FixedUpdate()
    {
        if (_characterController != null)
        {
            _cinemachineBrain.m_DefaultBlend.m_Time = Mathf.Min(Mathf.Abs(40 / _characterController.CharacterVelocity.magnitude), 1.5f);
        }
        //Debug.Log(_characterController.CharacterVelocity.x);
    }

    public void SpawnEnemiesOnRoomEnter()
    {
        DespawnEnemiesOnRoomExit();
        for(int i = 0; i<eSpawner.Length; i++)
        {
            //Debug.Log(eSpawner[i]);

            eSpawner[i]?.Spawn();
        }
    }

    public void DespawnEnemiesOnRoomExit()
    {
        for (int i = 0; i < eSpawner.Length; i++)
        {
            eSpawner[i]?.DestroySpawnedEnemy();
        }
    }
}
