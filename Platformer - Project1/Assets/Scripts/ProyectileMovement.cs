using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileMovement : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    #endregion
    #region parameters
    [SerializeField]
    private float _proyectileSpeed = 1.0f;
    [SerializeField]
    private Vector3 _direction;
    #endregion
    #region methods
    public void SetDirection(Vector3 newDirection)
    {
        _direction = newDirection;
    }
    #endregion

    void Start()
    {
        _myTransform = transform;
    }


    void Update()
    {
        _myTransform.position += _direction * _proyectileSpeed * Time.deltaTime;
    }
}
