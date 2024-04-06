using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundComponent : MonoBehaviour
{
    #region references
    private Transform _targetCameraTransform;
    private Transform _myTransform;
    #endregion

    #region parameters
    [SerializeField]
    private Vector2 _followValues;
    #endregion

    #region properties
    private Vector3 _startpos;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _myTransform = transform;
        _startpos = transform.position;
        _targetCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_targetCameraTransform != null)
        {
            _myTransform.position = new Vector3(_startpos.x + _targetCameraTransform.position.x * _followValues.x,
                _startpos.y + _targetCameraTransform.position.y * _followValues.y);
        }
    }
}
