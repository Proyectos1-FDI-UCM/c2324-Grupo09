using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeneIA : EnemyIA
{
    #region references
    private Transform _myTransform;
    private Transform _playerTransform;
    private EnemyMovement _enemyMovement;
    #endregion
    #region parameters
    [SerializeField]
    private float _beneVison = 1.0f;
    [SerializeField]
    private float _rightOffset = 15.0f;
    [SerializeField]
    private float _leftOffset = 15.0f;
    #endregion
    void Start()
    {
        _myTransform = transform;
        _playerTransform = FindObjectOfType<RefactoredCharacterMovement>().gameObject.transform;
        _enemyMovement = GetComponent<EnemyMovement>();
    }
    void Update()
    {
        if(((Mathf.Abs(_myTransform.position.y) - Mathf.Abs(_playerTransform.position.y))<_beneVison )&& (_myTransform.position.x >= _playerTransform.position.x-_leftOffset && _myTransform.position.x <= _playerTransform.position.x+_rightOffset))
        {
            _enemyMovement.Direction(Vector3.down);
        }
    }

    public override void OnHit()
    {
        Death();
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }


}
