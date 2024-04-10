using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompingHandIA : MonoBehaviour
{
    [SerializeField]
    Animator _anim;

    EnemyMovement eM;
    [SerializeField]
    LayerMask floor;
    GameObject waveAttackPrefab;

    GameObject projectileSpawned;

    [SerializeField]
    float projectileSpawnVerticalPos = 5f;
    [SerializeField]
    float offsetX = 10f;
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
            _anim.SetTrigger("TouchingGround");
            eM.Speed(0);
            if(spawnProjectile)
                Invoke("SpawnProjectile", 0.5f);
        }
    }

    private void SpawnProjectile()
    {
        if((int)sD != 0)
        {
            if(_anim.GetComponent<SpriteRenderer>().flipX == false)
                projectileSpawned = Instantiate(waveAttackPrefab, (transform.position.x + offsetX) * Vector3.right + projectileSpawnVerticalPos * Vector3.up, Quaternion.identity);//, transform);
            else
            {
                projectileSpawned = Instantiate(waveAttackPrefab, (transform.position.x - (offsetX*(2f/3))) * Vector3.right + projectileSpawnVerticalPos * Vector3.up, Quaternion.identity);//, transform);
                projectileSpawned.transform.localScale = new Vector3(projectileSpawned.transform.localScale.x * -1, projectileSpawned.transform.localScale.y, projectileSpawned.transform.localScale.z);
            }
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