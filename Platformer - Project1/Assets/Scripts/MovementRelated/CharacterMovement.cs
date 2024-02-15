using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    #region references
    [SerializeField]
    private MovementData _md;
    private Rigidbody2D _rb;
    #endregion

    #region hiddenVariables
    bool _isGrounded = false;
    bool _isJumping = false;
    bool _isSliding = false;
    bool _isJumpFalling = false;
    bool _isWallJumping = false;
    bool _isUsingPogo = false;
    bool _pogoAnimationCompleted = false;
    Vector3 _pogoStartVelocity = Vector3.zero;


    #region Timers
    float _lastGroundedTime = 0f;
    float _lastJumpTimeInput = 0f;
    [SerializeField]
    float _lastSlideTime = -10f;
    float _lastTimeSlideInput = 0f;
    float _pogoStartTime = 0f;
    float _pogoTouchedGround = 0f;
    #endregion

    #endregion

    #region Parameters
    private int _currentWallJumpNumber;
    private float _lastDirection = 1;
    #endregion

    #region accessors
    public bool Grounded { get { return _isGrounded; } }
    public float RByVel { get { return _rb.velocity.y; } }
    public bool Sliding { get { return _isSliding; } }
    #endregion

    #region methods

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _currentWallJumpNumber = _md.maxNumberOfWallJumps;
    }

    void Update()
    {
        _isGrounded = CheckGrounded();

        #region slide
        if(Time.time -_lastSlideTime > _md.slideDuration)
        {
            _isSliding = false;
        }
        if (_lastTimeSlideInput > 0) {
            if (Time.time - _lastSlideTime > _md.timeBetweenSlides && _isGrounded) //&& Mathf.Abs(_rb.velocity.y) < _md.slideYVelMarginError)
            {
                Slide();
            } 
            else if (!_isUsingPogo)
            {
                PogoStart();
            }
        }
        #endregion

        #region jump

        if (_lastJumpTimeInput > 0)
        {
            //Do Grounded Jump-----------------------------------------------------------------------
            if (_lastGroundedTime > 0 && !_isJumping)
            {
                Jump();
            }
            //DO WallJump----------------------------------------------------------------------------
            else if (_currentWallJumpNumber > 0 && !_isWallJumping && !_isUsingPogo)
            {
                WallJump();
            }
        }


        #region jumpChecks
        if(_rb.velocity.y < 0 && _lastGroundedTime <=0)
        {
            _isJumping = false;

            //no wall jumping
            _isJumpFalling = true;
        }
        else
        {
            _isJumpFalling = false;
        }
        #endregion

        #endregion

        #region SorroundingChecksUpdate
        if (!_isGrounded)
        {
            _lastGroundedTime -= Time.deltaTime;
            //if (_isUsingPogo && _rb.velocity.y < 0)
            //    _lastTimeSlideInput = -1;
        }
        else
        {
            if (_isUsingPogo)
            {
                _isUsingPogo = false;
                _pogoTouchedGround = Time.time;
            }
            _isWallJumping = false;
            _lastGroundedTime = _md.jumpCoyoteTime;
        }
        #endregion

        #region buffers
        if (!_isUsingPogo)
        {
            _lastTimeSlideInput -= Time.deltaTime;
            _lastJumpTimeInput -= Time.deltaTime;
        }
        #endregion

        #region changeGravity
        UpdateGravity();
        #endregion

        #region Pogo
        if (_isUsingPogo)
        {
            if (Time.time - _pogoStartTime < _md.pogoXDuration)
            {
                _rb.velocity = new Vector2 (Mathf.Lerp(_pogoStartVelocity.x, 0 , (Time.time - _pogoStartTime) / _md.pogoXDuration), _rb.velocity.y);
            }
            if(Time.time - _pogoStartTime < _md.pogoYDuration)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Lerp(_pogoStartVelocity.y, _md.pogoInitialUpVel, (Time.time - _pogoStartTime) / _md.pogoYDuration));
            }
            else 
            {
                _rb.AddForce(Vector2.down * _md.pogoFallForce, ForceMode2D.Impulse);
                if (!_pogoAnimationCompleted)
                    _pogoAnimationCompleted = true;
            }
        }
        #endregion
    }

    public void Run(float direction)
    {
        if (_isWallJumping || _isSliding || _isUsingPogo) return;

        //calculate the direction we want to move in and our desired velocity
        float targetSpeed = direction * _md.maxMoveSpeed;
        float accelRate;
        //-----------------------------------------------targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);
        //change acceleration rate depending on situation
        if (_isJumping || _isJumpFalling)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? (_md.acceleration * _md.accelInAir) : (_md.decceleration * _md.deccelInAir);
        else 
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _md.acceleration : _md.decceleration;

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (_md.doConserveMomentum && Mathf.Abs(_rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _lastGroundedTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //add bonus spead at the apex of a jump
        if ((_isJumping || _isJumpFalling) && Mathf.Abs(_rb.velocity.y) < _md.jumpHangTimeThreshold)
        {
            accelRate *= _md.jumpHangAccelerationMultiplier;
            targetSpeed *= _md.jumpHangMaxSpeedMultiplier;
        }

        //calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - _rb.velocity.x;
        //applies acceleration to speed difference, then raises to a set power so acceleration increases with higher speeds
        //finally multiplies by sign to reapply direction
        float movement = speedDif * accelRate;

        //applies force to rigidody, multiplying by Vector2.right so that it only affects X axis
        _rb.AddForce(movement * Vector2.right);
        if(direction != 0) _lastDirection = direction;
    }

    private void PogoStart()
    {
        _isUsingPogo = true;
        _pogoStartTime = Time.time;
        _pogoStartVelocity = _rb.velocity;
        _pogoAnimationCompleted = false;
    }

    private void Jump()
    {
        float jumpForceMultiplier = 1f;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        if (Time.time - _pogoTouchedGround <= _md.pogoEmpoweredJumpDuration && _pogoAnimationCompleted)
        {
            jumpForceMultiplier = _md.empoweredJumpForceMultiplier;
            _lastTimeSlideInput = -1;
        }
        _rb.AddForce(Vector2.up * _md.jumpForce * jumpForceMultiplier, ForceMode2D.Impulse);
        _pogoTouchedGround = -1;
        _isSliding = false;
        _isJumping = true;
        _lastJumpTimeInput = 0;
        _lastGroundedTime = 0;
    }

    private void WallJump()
    {
        float sameDirectionFactor = _md.wallJumpSameDirectionForceMultiplier;
        if (Mathf.Sign(_lastDirection) != Mathf.Sign(_rb.velocity.x) || Mathf.Abs(_rb.velocity.x) < _md.wallJumpForceApplyThreshold)
        {
            sameDirectionFactor = 1;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        //_rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _md.wallJumpForce.y + sameDirectionFactor * _md.wallJumpForce.x * _lastDirection * Vector2.right, ForceMode2D.Impulse);
        _isWallJumping = true;
        //_isJumping = true;
    }

    private void Slide()
    {
        _lastSlideTime = Time.time;
        _isSliding = true;
        _isUsingPogo = false;
        _lastTimeSlideInput = -1;
        _pogoTouchedGround = -1;

        float sameDirectionFactor = _md.slideSameDirectionForceMultiplier;
        if (Mathf.Sign(_lastDirection) != Mathf.Sign(_rb.velocity.x) || Mathf.Abs(_rb.velocity.x) < _md.slideForceApplyThreshold)
        {
            _rb.velocity = new Vector2(0,_rb.velocity.y);
            sameDirectionFactor = 1;
        }
        _rb.AddForce(sameDirectionFactor * Vector2.right * _lastDirection * _md.slideHorizontalForce, ForceMode2D.Impulse);
    }

    #region gravityRelated
    public void UpdateGravity()
    {
        if((_isJumping || _isJumpFalling) && Mathf.Abs(_rb.velocity.y) < _md.jumpHangTimeThreshold)
        {
            _rb.gravityScale = _md.gravityScale * _md.jumpHangGravityMultiplier;
        }
        else if (_rb.velocity.y < 0)
        {
            _rb.gravityScale = _md.gravityScale * _md.fallGravityMultiplier;
            //limitar la velocidad máxima de caida
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -_md.maxFallSpeed));
        }
        else
        {
            _rb.gravityScale = _md.gravityScale;
        }
    }
    #endregion

    #region SorroundingChecks
    private bool CheckGrounded()
    { 
        return Physics2D.OverlapBox((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, _md.groundCheckSize, 0, _md.groundLayer);
    }
    #endregion

    #region InputCallbacks
    public void JumpPressed()
    {
        _lastJumpTimeInput = _md.jumpBufferTime;
    }

    public void JumpReleased()
    {
        if(_isJumping)
        {
            _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _md.jumpCutMultiplier), ForceMode2D.Impulse);
        }

        _lastJumpTimeInput = 0;
    }

    public void SlidePressed()
    {
        _lastTimeSlideInput = _md.slideBufferTime;
    }
    public void SlideReleased()
    {
        _lastTimeSlideInput = 0;
    }
    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, _md.groundCheckSize);
    }
    #endregion


    #endregion
}
