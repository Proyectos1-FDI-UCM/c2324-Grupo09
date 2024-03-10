using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EnemySpawner : MonoBehaviour
{
    private bool enemyMoves;
    private GameObject spawnedEnemy;

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

    [SerializeField]
    Direction _enemyLooking = Direction.right;

    private void Start()
    {
        FindNewEnemyPrefab();
    }

    private void FindNewEnemyPrefab()
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
        EnemyTypeChanged();
    }

    public void Spawn()
    {
        spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        if(enemyMoves)
        {
            EnemyMovement eM = spawnedEnemy.GetComponent<EnemyMovement>();
            eM.Direction(direction);
            eM.Speed(speed);
            spawnedEnemy.transform.localScale = new Vector3(spawnedEnemy.transform.localScale.x * (int)_enemyLooking, spawnedEnemy.transform.localScale.y, spawnedEnemy.transform.localScale.z);
        }
    }

    public void Spawn(EnemyType eT, Vector2 dir, float sp)
    {
        enemy = eT;
        FindNewEnemyPrefab();

        spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        if (enemyMoves)
        {
            EnemyMovement eM = spawnedEnemy.GetComponent<EnemyMovement>();
            eM.Direction(dir);
            eM.Speed(sp);
            spawnedEnemy.transform.localScale = new Vector3(spawnedEnemy.transform.localScale.x * (int)_enemyLooking, spawnedEnemy.transform.localScale.y, spawnedEnemy.transform.localScale.z);
        }
    }

    private void EnemyTypeChanged()
    {
        if(enemy == EnemyType.RegularImp || enemy == EnemyType.BlueImp || enemy == EnemyType.Naha)
            enemyMoves = true;
        else 
            enemyMoves = false;
    }

    public void DestroySpawnedEnemy()
    {
        Destroy(spawnedEnemy);
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
public enum Direction
{
    right = 1,
    left = -1
}

