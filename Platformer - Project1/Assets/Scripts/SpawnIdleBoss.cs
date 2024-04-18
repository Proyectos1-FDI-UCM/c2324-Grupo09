using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnIdleBoss : MonoBehaviour
{
    BossIA _bossIA;
    GameObject aux = null;
    public void Start()
    {
        _bossIA = FindObjectOfType<BossIA>();
    }

    public void Spawn()
    {
        aux = Instantiate(Resources.Load<GameObject>("IdleBoss"), this.transform.position, Quaternion.identity);
        if(_bossIA.CheckSpawnBossIdle(ref aux))
        {
            _bossIA.GetSpawnedBoss(aux);

            Destroy(aux);
        }
        else
        {
            _bossIA.GetSpawnedBoss(aux);
        }
    }

    public void DestroyBefore()
    {
        Destroy(aux);
        Destroy(gameObject);
    }


    public void Finished()
    {
        Destroy(gameObject);
    }
}
