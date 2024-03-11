using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NahaIA : EnemyIA
{
    #region references
    [SerializeField]
    private GameObject _wallrunPrefab;
    private Transform _myTransform;
    private RefactoredCharacterController _characterController;
    private EnemyAnimController _anim;
    #endregion
    #region properties
    private bool _isUsingPogo;
    private bool _alreadyHitWithPogo = false;
    #endregion
    #region parameters
    [SerializeField]
    private float _xOffset = 1.0f;
    [SerializeField]
    private float _xRunWallScale = 1.0f;
    [SerializeField]
    private float _yRunWallScale = 1.0f;
    #endregion
    public override void OnHit()
    {
        if(_isUsingPogo && !_alreadyHitWithPogo)
        {
            _anim.NahaShift();
            GameObject newGameObject = Instantiate(_wallrunPrefab,_myTransform.position, Quaternion.identity);
            newGameObject.transform.position += _xOffset * GetComponent<EnemyMovement>().ReadOnlyDirection * Vector3.right;
            newGameObject.transform.localScale = new Vector3(_myTransform.localScale.x - _xRunWallScale, _myTransform.localScale.y - _yRunWallScale, 0);
            newGameObject.transform.parent = _myTransform;
            _alreadyHitWithPogo = true;
        }
    }
    public override void Death()
    {
        Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _characterController = FindObjectOfType<RefactoredCharacterController>();
        _anim = GetComponent<EnemyAnimController>();
    }

    // Update is called once per frame
    void Update()
    {
        _isUsingPogo = _characterController.IsUsingPogo;
    }
}
