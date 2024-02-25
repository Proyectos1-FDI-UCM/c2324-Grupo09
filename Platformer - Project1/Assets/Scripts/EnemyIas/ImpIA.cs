using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpIA : MonoBehaviour
{
    CharacterController _character;
    EnemyMovement _enemyMove;
    [SerializeField]
    float _maxSpeed;
    float _timeHit,speed;
    [SerializeField]
    float _timeForCicle;
    EnemyState enemyState=EnemyState.alive;
    private void Start()
    {
        _enemyMove = GetComponent<EnemyMovement>();
    }
    private void Update()
    {
        if (enemyState == EnemyState.hitted1) 
        {
          speed=  Mathf.Lerp(_maxSpeed, -_maxSpeed, (Mathf.Sin(((Time.time-_timeHit)/_timeForCicle)*Mathf.PI*2-Mathf.PI/2)+1)/2);
            _enemyMove.Speed(speed);
        }
        else if (enemyState == EnemyState.dead) 
        {
        Destroy(gameObject);
        }
    }
    private void ChangeState()
    {
        if (enemyState == EnemyState.hitted1)
        {
            _timeHit = Time.time;
            enemyState = EnemyState.dead;
        }
        else if (enemyState == EnemyState.alive)
        {
            enemyState=EnemyState.hitted1;
            _enemyMove.Direction(Vector3.up);
           
        }
    }
    private void OnTriggerEnter2D(Collider2D colision)
    {
        _character = colision.GetComponent<CharacterController>();
        if (_character != null) 
        {
            ChangeState();
        }
    }
}
