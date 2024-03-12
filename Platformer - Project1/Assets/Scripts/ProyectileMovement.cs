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
        newDirection.Normalize();
        _direction = newDirection;
        transform.localRotation = Quaternion.Euler(Vector3.forward * (newDirection.x * -90 + newDirection.y * -180));
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
