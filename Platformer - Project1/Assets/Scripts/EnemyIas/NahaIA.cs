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
    private GameObject[] _culosNaha;
    private GameObject _prefabCuloNaha;
    [SerializeField]
    private float xOffsetCuloNaha = 40f;
    private int _lookingTo;
    #endregion
    #region properties
    private bool _isUsingPogo;
    private bool _alreadyHitWithPogo = false;
    #endregion
    #region parameters
    [SerializeField]
    private Vector2 _offsetWR;
    [SerializeField]
    private Vector2 _sizeWR;

    [SerializeField]
    private Vector3 _initialOffsetCulosNaha;
    #endregion
    public override void OnHit()
    {
        if(_isUsingPogo && !_alreadyHitWithPogo)
        {
            _anim.NahaShift();
            for(int i= 0; i < _culosNaha.Length; i++)
            {
                try
                {
                    _culosNaha[i].GetComponent<EnemyAnimController>().NahaShift();
                }
                catch
                {
                    Debug.Log("peta con el n:" + i);
                }

            }
            GameObject newGameObject = Instantiate(_wallrunPrefab,_myTransform.position, Quaternion.identity);
            newGameObject.transform.position += (0.5f * (1 + _culosNaha.Length) * _sizeWR.x -  0.75f * xOffsetCuloNaha) * Vector3.right * _lookingTo + Vector3.up * _offsetWR.y;//((((_culosNaha.Length-1) * xOffsetCuloNaha * 15/20) + _offsetWR.x)) * Vector3.right  * _lookingTo + Vector3.up * _offsetWR.y;
            newGameObject.transform.localScale = (1+_culosNaha.Length) * _sizeWR.x * Vector3.right + _sizeWR.y * Vector3.up;//new Vector3(_myTransform.localScale.x - _xRunWallScale, _myTransform.localScale.y - _yRunWallScale, 0);
            newGameObject.transform.parent = _myTransform;
            newGameObject.GetComponent<SpriteRenderer>().color = Color.clear;
            _alreadyHitWithPogo = true;
        }
    }
    public override void Death()
    {
        Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Awake()
    {

        _characterController = FindObjectOfType<RefactoredCharacterController>();
        _anim = GetComponent<EnemyAnimController>();
        _culosNaha = new GameObject[0]; 
    }

    // Update is called once per frame
    void Update()
    {
        _isUsingPogo = _characterController.IsUsingPogo;
    }


    public void SetSize(int n, int Dir)
    {
        _lookingTo = Dir;
        _myTransform = transform;
        _prefabCuloNaha = Resources.Load<GameObject>("NahaBody");
        _culosNaha = new GameObject[n];
        for (int i = 0; i < n-1; i++)
        {
            _culosNaha[i] = Instantiate(_prefabCuloNaha, _initialOffsetCulosNaha.y * Vector3.up +_myTransform.position + (Mathf.Sign(Dir) * Vector3.right * (xOffsetCuloNaha * (i+1) + _initialOffsetCulosNaha.x)), Quaternion.identity, _myTransform);
        }
        _culosNaha[n-1] = Instantiate(Resources.Load<GameObject>("NahaTail"), _initialOffsetCulosNaha.y * Vector3.up + _myTransform.position + (Mathf.Sign(Dir) * Vector3.right * (xOffsetCuloNaha * n + _initialOffsetCulosNaha.x)), Quaternion.identity, _myTransform);
        //_culosNaha[n].transform.localScale = _culosNaha[n].transform.localScale * Mathf.Sign(_myTransform.localScale.x);
    }
}
