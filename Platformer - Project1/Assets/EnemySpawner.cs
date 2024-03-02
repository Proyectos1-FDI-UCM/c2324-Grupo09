using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemySpawner : MonoBehaviour
{
    private bool enemyMoves;

    [SerializeField]
    [OnValueChanged("EnemyTypeChanged")]
    private EnemyType enemy;


    [SerializeField]
    [ShowIf("enemyMoves")]
    [AllowNesting]
    private float speed;

    [SerializeField]
    [ShowIf("enemyMoves")]
    [AllowNesting]
    private Vector2 direction;

    private GameObject enemyPrefab;

    private void Start()
    {
        string pathToEnemyPrefab = "";
        switch (enemy)
        {
            case EnemyType.RegularImp:
                pathToEnemyPrefab = "RegularImp";
                break;
            case EnemyType.BlueImp:
                pathToEnemyPrefab = "BlueImp";
                break;
            case EnemyType.Naha:
                pathToEnemyPrefab = "Naha";
                break;
            case EnemyType.Bene:
                pathToEnemyPrefab = "Bene";
                break;
            case EnemyType.Hin:
                pathToEnemyPrefab = "Hin";
                break;
        }
        enemyPrefab = Resources.Load<GameObject>(pathToEnemyPrefab);
        Debug.Log(enemyPrefab);
        Spawn();
    }

    public void Spawn()
    {
        GameObject spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    private void EnemyTypeChanged()
    {
        if(enemy == EnemyType.RegularImp || enemy == EnemyType.BlueImp || enemy == EnemyType.Naha)
            enemyMoves = true;
        else 
            enemyMoves = false;
    }
}

public enum EnemyType
{
    RegularImp,
    BlueImp,
    Bene,
    Hin,
    Naha,
}

