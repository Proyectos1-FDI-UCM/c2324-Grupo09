using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region references
    private ParticleSystem _runParticle;
    private Animator _characterAnim;
    private Transform _myTransform;
    [SerializeField]
    private GameObject _particlePrefab;
    #endregion
    #region properties
    private float _xdistance;
    private float _ydistance;
    private float _yrotation;
    private float _xrotation;
    private bool _isRunning = false;
    #endregion
    #region parameters
    [SerializeField]
    private float _xdistanceParam = 1.0f;
    [SerializeField]
    private float _ydistanceParam = 1.0f;
    #endregion
    #region methods
    public void InstantiateParticle(int x)
    {
        if (x == 0)
        {
            _xrotation = 0.0f;
            _xdistance = _xdistanceParam;
            _yrotation = 90.0f;
            _ydistance = 0.0f;

        }
        else if (x == 1)
        {
            _xrotation = 0.0f;
            _xdistance = -_xdistanceParam;
            _yrotation = -90.0f;
            _ydistance = 0.0f;

        }
        else if(x == 2)
        {
            _xrotation = 90.0f;
            _xdistance = 0.0f;
            _yrotation = 90.0f;
            _ydistance = _ydistanceParam;
        }

        GameObject newParticles = Instantiate(_particlePrefab, new Vector3(_myTransform.position.x + _xdistance, _myTransform.position.y + _ydistance, _myTransform.position.z), Quaternion.identity);
        newParticles.transform.Rotate(new Vector3(_xrotation, _yrotation, 90));
        newParticles.GetComponent<ParticleSystem>().Play();
    }
    #endregion
    void Start()
    {
        _myTransform = GetComponentInParent<Transform>();
        _runParticle = GetComponentInChildren<ParticleSystem>();
        _characterAnim = GetComponent<Animator>();
    }
    void Update()
    {
        if(_characterAnim.GetInteger("xMovement") != 0 && !_isRunning && _characterAnim.GetBool("Grounded"))
        {
            _runParticle.Play();
            _isRunning = true;
        }

        if(_characterAnim.GetInteger("xMovement") == 0 && _isRunning || !_characterAnim.GetBool("Grounded"))
        {
            _runParticle.Stop();
            _isRunning = false;
        }
    }

}
