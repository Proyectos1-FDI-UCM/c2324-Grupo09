using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    #endregion
    #region parameters

    public float ReadOnlyDirection
    {
        get => Mathf.Sign(_movementDirection.x);
    }
    [SerializeField]
    private float _movementSpeed = 3.0f;
    [SerializeField]
    private Vector3 _movementDirection = Vector3.right;
    #endregion
    #region methods
    public void Direction(Vector3 direction)
    {
        _movementDirection = direction.normalized;
    }
    public void Speed(float speed)
    {
        _movementSpeed = speed;
    }
    #endregion
    void Start()
    {
        _myTransform = transform;
    }

    void Update()
    {
        _myTransform.position += _movementSpeed * _movementDirection * Time.deltaTime;
    }
}
