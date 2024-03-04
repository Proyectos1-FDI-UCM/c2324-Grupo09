using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileInstantiate : MonoBehaviour
{
    #region references
    private Transform _myTransform;
    [SerializeField]
    private GameObject _proyectilePrefab;
    private RefactoredCharacterController _refactoredChController;
    #endregion
    #region methods
    public void Launch(Vector3 _proyectileDirection)
    {
        GameObject newGameObject = Instantiate(_proyectilePrefab, _myTransform.position, Quaternion.identity);
        newGameObject.GetComponent<ProyectileMovement>().SetDirection(_proyectileDirection);
    }
    #endregion
    void Start()
    {
        _myTransform = transform;
        _refactoredChController = FindObjectOfType<RefactoredCharacterController>();
    }
}
