using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnIdleBoss : MonoBehaviour
{
    BossIA _bossIA;
    public void Start()
    {
        _bossIA = FindObjectOfType<BossIA>();
    }

    public void Spawn()
    {
        GameObject aux = Instantiate(Resources.Load<GameObject>("IdleBoss"), this.transform.position, Quaternion.identity);
        if(_bossIA.CheckSpawnBossIdle(ref aux))
        {
            Destroy(aux);
        }
    }


    public void Finished()
    {
        Destroy(gameObject);
    }
}
