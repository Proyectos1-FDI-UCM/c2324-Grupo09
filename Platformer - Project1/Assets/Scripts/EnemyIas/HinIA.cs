using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HinIA : EnemyIA
{
    #region references
    private Transform _myTransform;
    private EnemyMovement _enemyMovement;
    private RefactoredCharacterController _player;
    private BoxCollider2D _myCollider;
    private EnemyHit _hitControl;
    #endregion
    #region parameters
    [SerializeField]
    private float _hinVision = 1.0f;
    [SerializeField]
    private float _yMaxPosInFirstStage = 1.0f;
    [SerializeField]
    private float _firstStageCoolDown = 1.0f;
    [SerializeField]
    private float _alturaMax = 1.0f;
    #endregion
    #region properties
    private bool _HinHasTeleported = false;
    private bool _isFalling = false;
    private float _yPreviousFrameValue;
    private float _timeSinceSecondStage;
    private bool _FinalStageControl = true;
    private bool _playerIsInRange;
    #endregion
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

            }

        }
    }
    #endregion
    public override void OnHit()
    {
        Death();
    }
    public override void Death()
    {
        Destroy(this.gameObject);
    }
    void Start()
    {
        _myTransform = transform;
        _enemyMovement = GetComponent<EnemyMovement>();
        _player = FindObjectOfType<RefactoredCharacterController>();
        _yPreviousFrameValue = _myTransform.position.y;
        _myCollider = GetComponent<BoxCollider2D>();
        _hitControl = GetComponent<EnemyHit>();
    }


    void Update()
    {
        _playerIsInRange = _player.transform.position.x < (_myTransform.position.x + _hinVision) && _player.transform.position.x > (_myTransform.position.x - _hinVision);
        _timeSinceSecondStage = Mathf.FloorToInt(Time.time) + _firstStageCoolDown - 1;

        if (_playerIsInRange && !_HinHasTeleported)
        {
            HinFirstStage();
        }
        if ((_timeSinceSecondStage % _firstStageCoolDown == 0) && _HinHasTeleported && _FinalStageControl)
        {
            _FinalStageControl = false;
        }
        if(!_FinalStageControl)
        {
            _myCollider.enabled = true;
            _hitControl.enabled = true;

            HinFinalStage();
        }

    }

}
