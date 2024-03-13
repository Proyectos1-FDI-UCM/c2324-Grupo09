using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region references
    private RefactoredCharacterMovement _checkLastDir;
    private ParticleSystem _runParticle;
    private Animator _characterAnim;
    private Transform _myTransform;
    [SerializeField]
    private GameObject _particlePrefab;
    #endregion
    #region properties
    private int _lastDir;
    private float _xdistance;
    private float _ydistance;
    private float _yrotation;
    private float _xrotation;
    private bool _isRunning = false;
    private bool _hasBeenUsedWallJump = false;
    private bool _saveDir = true;
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
    private void RunCheck()
    {
        if (_characterAnim.GetInteger("xMovement") != 0 && !_isRunning && _characterAnim.GetBool("Grounded"))
        {
            _runParticle.Play();
            _isRunning = true;
        }
        if (_characterAnim.GetInteger("xMovement") == 0 && _isRunning || !_characterAnim.GetBool("Grounded"))
        {
            _runParticle.Stop();
            _isRunning = false;
        }
    }
    private void WallJumpCheck()
    {
        if (_characterAnim.GetBool("WJ"))
        {
            _hasBeenUsedWallJump = true;
        }
        if (_characterAnim.GetBool("Grounded"))
        {
            _hasBeenUsedWallJump = false;
            _saveDir = true;
        }
        else if (_hasBeenUsedWallJump && !_characterAnim.GetBool("WJ") && _lastDir == -1)
        {
            InstantiateParticle(1);
            _hasBeenUsedWallJump = false;
        }
        else if (_hasBeenUsedWallJump && !_characterAnim.GetBool("WJ") && _lastDir == 1)
        {
            InstantiateParticle(0);
            _hasBeenUsedWallJump = false;
        }

    }
    private void SaveLastDirectionOfWallJump()
    {
        if (_characterAnim.GetBool("WJ") && _saveDir)
        {
            _lastDir = _checkLastDir.LastDirection;
            _saveDir = false;
        }
    }
    #endregion
    void Start()
    {
        _myTransform = transform;
        _runParticle = GetComponentInChildren<ParticleSystem>();
        _characterAnim = GetComponent<Animator>();
        _checkLastDir = FindObjectOfType<RefactoredCharacterMovement>();
        
    }
    void Update()
    {
        RunCheck();
        SaveLastDirectionOfWallJump();
        WallJumpCheck();
    }

}
