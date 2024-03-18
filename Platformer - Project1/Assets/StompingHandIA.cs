using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingHandIA : MonoBehaviour
{
    EnemyMovement eM;
    [SerializeField]
    LayerMask floor;
    GameObject waveAttackPrefab;

    GameObject projectileSpawned;

    [SerializeField]
    float projectileSpawnVerticalPos = 5f;
    public bool spawnProjectile = true;

    stompDirection sD = stompDirection.left;

    // Start is called before the first frame update
    void Start()
    {
        eM = GetComponent<EnemyMovement>();
        waveAttackPrefab = Resources.Load<GameObject>("WaveAttack");
    }

    public void BeginToFall(int i)
    {
        eM.Speed(200);
        sD = (stompDirection)i;
    }

    public void PrevisualizeStomp()
    {
        eM.Speed(-30);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((floor == (floor | (1 << other.gameObject.layer))) && projectileSpawned == null)
        {
            eM.Speed(0);
            if(spawnProjectile)
                SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        if((int)sD != 0)
        {
            projectileSpawned = Instantiate(waveAttackPrefab, transform.position.x * Vector3.right + projectileSpawnVerticalPos * Vector3.up, Quaternion.identity);//, transform);
            projectileSpawned.GetComponent<EnemyMovement>().Direction(Vector3.right * (int)sD);
        }
    }

    public void ProjectileDestroy()
    {
        Destroy(projectileSpawned);
    }
    public void DestroySelf()
    {
        try
        {
            Destroy(this.gameObject);
        }
        catch{}
    }
}

enum stompDirection
{
    left = -1,
    right = 1,
    both = 0
}