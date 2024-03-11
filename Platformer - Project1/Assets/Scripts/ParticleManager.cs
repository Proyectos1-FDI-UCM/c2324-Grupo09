using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region references
    private ParticleSystem _myParticleSystem;
    private Transform _myTransform;
    [SerializeField]
    private GameObject _particlePrefab;
    #endregion
    #region properties
    private float _distance;
    private float _yrotation;
    #endregion
    #region methods
    public void InstantiateParticle(int x)
    {
        if (x == 0)
        {
            _distance = 25.0f;
            _yrotation = 90.0f;
        }
        else if (x == 1)
        { 
            _distance = -25.0f;
            _yrotation = -90.0f;
        }

        GameObject newParticles = Instantiate(_particlePrefab, new Vector3(_myTransform.position.x + _distance, _myTransform.position.y, _myTransform.position.z), Quaternion.identity);
        newParticles.transform.Rotate(new Vector3(0, _yrotation, 90));
        newParticles.GetComponent<ParticleSystem>().Play();
    }
    #endregion
    void Start()
    {
        _myTransform = transform;
    }

}
