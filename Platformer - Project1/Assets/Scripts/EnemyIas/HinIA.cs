using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinIA : EnemyIA
{
    #region references
    private Transform _myTransform;
    private EnemyMovement _enemyMovement;
    private RefactoredCharacterController _player;
    EnemyState _enemyState;
    #endregion
    #region parameters
    [SerializeField]
    private float _hinVision = 1.0f;
    [SerializeField]
    private float _yMaxPosInFirstStage = 1.0f;
    [SerializeField]
    private float _firstStageCoolDown = 1.0f;
    private float _time;
    [SerializeField]
    private float _alturaMax = 1.0f;
    #endregion
    #region properties
    private bool _HinHasTeleported = false;
    private bool _isFalling = false;
    private float _yPreviousFrameValue;
    private Vector3 _direction;
    [SerializeField]
    private float _speed;
    #endregion
    #region methods
    public override void OnHit()
    {

    }
    public override void Death()
    {

    }
    private void HinFirstStage()
    { 
        _enemyMovement.Direction(Vector3.up);

    }
    private void HinSecondStage()
    {
        _time = Time.time;
    }
    private void HinFinalStage()
    {
        _enemyMovement.Direction(Vector3.up);
        _enemyMovement.Speed(_speed);
        if (_myTransform.position.y > _yMaxPosInFirstStage + _yPreviousFrameValue + _alturaMax)
        {
            _enemyMovement.Speed(0);
            _isFalling=true;
        }
    }
    #endregion

    void Start()
    {
        _myTransform = transform;
        _enemyMovement = GetComponent<EnemyMovement>();
        _player = FindObjectOfType<RefactoredCharacterController>();
        _yPreviousFrameValue = _myTransform.position.y;
    }

    
    void Update()
    {
        _direction = _enemyMovement._direction;

        if ((_player.transform.position.x < (_myTransform.position.x + _hinVision) && (_player.transform.position.x > _myTransform.position.x - _hinVision))&&!_HinHasTeleported)
        {
            _myTransform.transform.position = new Vector3(_player.transform.position.x, _myTransform.position.y, 0);
            _HinHasTeleported=true;
        }

        _enemyMovement.Direction(Vector3.zero);

        if (_HinHasTeleported && _myTransform.position.y <= _yPreviousFrameValue + _yMaxPosInFirstStage)
        {
            HinFirstStage();
        }

        if(_HinHasTeleported && _direction == Vector3.zero)
        {
            HinSecondStage();
        }
        if(_time > _firstStageCoolDown && !_isFalling)
        {
            HinFinalStage();
        }
        if (_isFalling)
        {
            
            _enemyMovement.Speed(_speed);
            _enemyMovement.Direction(Vector3.down);
            
        }
        if(_myTransform.position.y < _yPreviousFrameValue)
        {
            _enemyMovement.Direction(Vector3.zero);
            _HinHasTeleported = false;
            _isFalling = false;
            _time = 0;
        }

        Debug.Log(_time);
    }
}
