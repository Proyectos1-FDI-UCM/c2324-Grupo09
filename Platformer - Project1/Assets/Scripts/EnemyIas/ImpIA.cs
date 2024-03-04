using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ImpIA : EnemyIA
{

    RefactoredCharacterController _character;
    EnemyMovement _enemyMove;
    [SerializeField]
    float _maxSpeed;
    float _timeHit,speed;
    [SerializeField]
    float _timeForCicle;
    [SerializeField]
    float _gravity;
    EnemyState enemyState=EnemyState.baseState;
    private bool _isWallJumping;
    private bool _isUsingPogo;
    private ProyectileInstantiate _proyectileInstantiate;
    private float _x;


    public override void OnHit()
    {
        _x = _character.transform.position.x;

        if (enemyState == EnemyState.state1 && _isWallJumping)
        {
            enemyState = EnemyState.dead;
            if(_x > transform.position.x)
            {
                _proyectileInstantiate.Launch(Vector3.left);
            }
            if(_x < transform.position.x)
            {
                _proyectileInstantiate.Launch(Vector3.right);
            }

        }
        else if (enemyState == EnemyState.state1 && _isUsingPogo)
        {
            enemyState = EnemyState.dead;
            _proyectileInstantiate.Launch(Vector3.down);
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
        _proyectileInstantiate = GetComponent<ProyectileInstantiate>();
        _character = FindObjectOfType<RefactoredCharacterController>();
        
    }
    private void Update()
    {
        _isWallJumping = FindObjectOfType<RefactoredCharacterController>().IsWallJumping;
        _isUsingPogo = FindObjectOfType<RefactoredCharacterController>().IsUsingPogo;

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
