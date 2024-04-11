using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinIA : EnemyIA
{
    #region references
    private Transform _myTransform;
    private EnemyMovement _enemyMovement;
    private Transform _player;
    private BoxCollider2D _myCollider;
    private EnemyHit _hitControl;
    private EnemyAnimController _enemyAnimController;
    #endregion
    #region parameters
    [SerializeField]
    private float _hinVision = 1.0f;
    #endregion
    #region properties
    [SerializeField]
    private float _timeMoving;
    [SerializeField]
    private float _totalTimeMoving;
    [SerializeField]
    private float _HinMaxSpeed;

    State _currentState = State.waiting;
    #endregion
    /*
    #region methods
    private void HinFirstStage()
    {
        Debug.Log("Estoy en la primera fase");
        _myTransform.transform.position = new Vector3(_player.transform.position.x, _myTransform.position.y, 0);

        if (_myTransform.position.y < _yPreviousFrameValue + _yMaxPosInFirstStage)
        {
            _enemyMovement.Direction(Vector3.up);
        }

        else if (_myTransform.transform.position.y > _yPreviousFrameValue + _yMaxPosInFirstStage)
        {
            _HinHasTeleported = true;
            _enemyMovement.Direction(Vector3.zero);
        }

    }
    private void HinFinalStage()
    {
        if(_myTransform.position.y < (_yPreviousFrameValue + _alturaMax) && !_isFalling)
        {
            _enemyMovement.Direction(Vector3.up);
        }
        else
        {
            _isFalling=true;
            _enemyAnimController.HinFall();
        }
        if (_isFalling)
        {
            _enemyMovement.Direction(Vector3.down);

            if (_myTransform.position.y < _yPreviousFrameValue)
            {
                _enemyMovement.Direction(Vector3.zero);
                _HinHasTeleported = false;
                _isFalling = false;
                _FinalStageControl = true;
                _myCollider.enabled = false;
                _hitControl.enabled = false;
                _enemyAnimController.HinWait();

            }

        }
    }
    #endregion

    */
    public override void OnHit()
    {
        _timeMoving += _totalTimeMoving / 2;
        //Death();
    }
    public override void Death()
    {
        Destroy(this.gameObject);
    }
    void Start()
    {
        _myTransform = transform;
        _enemyMovement = GetComponent<EnemyMovement>();
        _player = FindObjectOfType<RefactoredCharacterController>().transform;
        _myCollider = GetComponent<BoxCollider2D>();
        _hitControl = GetComponent<EnemyHit>();
        _enemyAnimController = GetComponent<EnemyAnimController>();
    }


    void Update()
    {
        if(_currentState == State.waiting)
        {
            if (PlayerIsInRange())
            {
                _currentState = State.moving;
                _timeMoving = Time.time;
            }
        }
        else
        {
            _enemyMovement.Speed(Mathf.Lerp(_HinMaxSpeed,-_HinMaxSpeed,Mathf.Pow((Time.time - _timeMoving)/_totalTimeMoving, 1f)));
            if (Time.time - _timeMoving > _totalTimeMoving)
            {
                _enemyMovement.Speed(0);
                _currentState = State.waiting;
            }
        }
    }

    bool PlayerIsInRange()
    {
        return _player.position.x < (_myTransform.position.x + _hinVision) && _player.position.x > (_myTransform.position.x - _hinVision);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(_myTransform.position, _hinVision * Vector3.one);
    }

    enum State
    {
        waiting,
        moving
    }

}
