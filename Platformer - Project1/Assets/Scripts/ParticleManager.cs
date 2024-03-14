using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    #region references
    private RefactoredCharacterMovement _checkLastDir;
    private RefactoredCharacterController _checkStates;
    private ParticleSystem _runParticle;
    private TrailRenderer _myTrail;
    private Animator _characterAnim;
    private Transform _myTransform;
    [SerializeField]
    private GameObject _collisionParticlePrefab;
    [SerializeField]
    private GameObject _initialJumpParticlePrefab;
    [SerializeField]
    private GameObject _endJumpParticlePrefab;
    private GameObject _particleController;
    #endregion
    #region properties
    private int _lastDir;
    private float _xdistance;
    private float _ydistance;
    private float _xrotation;
    private float _yrotation;
    private float _zrotation;
    private float _yOffsetOnJump;
    private bool _isRunning = false;
    private bool _hasBeenUsedWallJump = false;
    private bool _isSliding = false;
    private bool _saveDir = true;
    private bool _alreadyJump = false;
    private bool _isOnAir = false;
    private bool _hasLanded = false;
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
            _zrotation = 90.0f;

        }
        else if (x == 1)
        {
            _xrotation = 0.0f;
            _xdistance = -_xdistanceParam;
            _yrotation = -90.0f;
            _ydistance = 0.0f;
            _zrotation = 90.0f;

        }
        else if(x == 2)
        {
            _xrotation = 90.0f;
            _xdistance = 0.0f;
            _yrotation = 90.0f;
            _zrotation = 90.0f;
            _ydistance = _ydistanceParam;
        }

        GameObject newParticles = Instantiate(_collisionParticlePrefab, new Vector3(_myTransform.position.x + _xdistance, _myTransform.position.y + _ydistance, _myTransform.position.z), Quaternion.identity);
        newParticles.transform.Rotate(new Vector3(_xrotation, _yrotation, _zrotation));
        newParticles.GetComponent<ParticleSystem>().Play();
    }
    private void InstantiateJumpParticles(int a)
    {
        GameObject JumpParticles;
        if(a == 0)
        {
            JumpParticles = _initialJumpParticlePrefab;
            _yOffsetOnJump = 8.0f;
        }
        else
        {
            JumpParticles = _endJumpParticlePrefab;
            _yOffsetOnJump = 2.5f;
        }
        GameObject newJumpParticles = Instantiate(JumpParticles, new Vector3(_myTransform.position.x, _myTransform.position.y-_yOffsetOnJump, _myTransform.position.z),Quaternion.identity);
        newJumpParticles.transform.Rotate(new Vector3(-90,0,0));
        newJumpParticles.GetComponent<ParticleSystem>().Play();
    }
    private void RunCheck()
    {
        if (_characterAnim.GetInteger("xMovement") != 0 && !_isSliding && !_isRunning && _characterAnim.GetBool("Grounded"))
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
    private void CheckInitialJump()
    {

        if (!_characterAnim.GetBool("Grounded") && !_characterAnim.GetBool("FallingDown") && !_alreadyJump && !_isOnAir)
        {
            _alreadyJump = true;
            _isOnAir = true;
        }
        else if (_alreadyJump && !_characterAnim.GetBool("Grounded") && !_characterAnim.GetBool("FallingDown") && _isOnAir)
        {
            InstantiateJumpParticles(0);
            _alreadyJump = false;
        }
        else if (_characterAnim.GetBool("Grounded") && _isOnAir)
        {
            _isOnAir = false;
            _hasLanded = true;
        }
    }
    private void CheckEndOfJump()
    {
        if(_hasLanded)
        {
            InstantiateJumpParticles(1);
            _hasLanded = false;
        }
    }
    private void CheckSlide()
    {
        if(_isSliding)
        {
            _myTrail.emitting = true;
        }
        else
        {
            _myTrail.emitting = false;        
        }
    }
    #endregion
    void Start()
    {
        _myTransform = transform;
        _runParticle = GetComponentInChildren<ParticleSystem>();
        _characterAnim = GetComponent<Animator>();
        _checkLastDir = FindObjectOfType<RefactoredCharacterMovement>();
        _checkStates = GetComponentInParent<RefactoredCharacterController>();
        _myTrail = GetComponentInChildren<TrailRenderer>();
        
    }
    void Update()
    {
        _isSliding = _checkStates.IsSliding;

        RunCheck();
        SaveLastDirectionOfWallJump();
        WallJumpCheck();

        CheckInitialJump();
        CheckEndOfJump();

        CheckSlide();

    }

}
