using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeneDetection : MonoBehaviour
{
    #region references
    private EnemyMovement _myEnemyMovement;
    #endregion 


    void Start()
    {
        _myEnemyMovement = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        RefactoredCharacterController aux = collision.GetComponent<RefactoredCharacterController>();
        if (aux != null)
        {
            _myEnemyMovement.Direction(Vector3.down);
        }
    }

}
