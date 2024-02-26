using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpIA : EnemyIA
{

    CharacterController _character;
    EnemyMovement _enemyMove;
    [SerializeField]
    float _maxSpeed;
    float _timeHit,speed;
    [SerializeField]
    float _timeForCicle;
    [SerializeField]
    float _gravity;
    EnemyState enemyState=EnemyState.baseState;
    public override void OnHit()
    {
        
        if (enemyState == EnemyState.state1)
        {
            
            enemyState = EnemyState.dead;
        }
        else if (enemyState == EnemyState.baseState)
        {
            _timeHit = Time.time;
            enemyState = EnemyState.state1;
            _enemyMove.Direction(Vector3.up);

        }
    }
    public override void Death()
    {
        Destroy(this.gameObject);
    }
    private void Start()
    {
        _enemyMove = GetComponent<EnemyMovement>();
    }
    private void Update()
    {
       if (enemyState == EnemyState.state1) 
        {
            if (!((Time.time - _timeHit)  >= _timeForCicle))
            {
                speed = Mathf.Lerp(_maxSpeed,-_gravity, (Mathf.Sin(((Time.time - _timeHit) / _timeForCicle) * Mathf.PI     - Mathf.PI / 2) + 1) / 2);
                _enemyMove.Speed(speed);
            
            }
        }
        else if (enemyState == EnemyState.dead) 
        {
            Death();
        }
    }
    
   
}
