using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemies : MonoBehaviour
{
    EnemyHit _enemyHit;
   
    LayerMask _layerchecked;
    int _layer = 7;
    void Start()
    {
        
    }
  

    private void OnTriggerExit2D(Collider2D collision)
    {
        _layerchecked=collision.gameObject.layer;
        if (_layerchecked==_layer) 
        {
            Destroy(collision.gameObject);
        }
    }
   
}
