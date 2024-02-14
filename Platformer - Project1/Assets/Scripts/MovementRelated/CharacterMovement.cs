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
    bool _isJumping = false;
    bool _isSliding = false;
    bool _isJumpFalling = false;
    [SerializeField]
    bool _isWallJumping = false;

    #region Timers
    float _lastGroundedTime = 0f;
    float _lastJumpTime = 0f;
    float _lastSlideTime = -10f;
    float _lastTimeSlideInput = 0f;
    #endregion

    #endregion

    #region Parameters
    private int _currentWallJumpNumber;
    private float _lastDirection = 1;

    //private bool _movementIsBlocked = false;

    #region checkData
    [SerializeField]
    float _yGroundCheckOffSet = 0f;
    [SerializeField]
    Vector2 _groundCheckSize;
    [SerializeField]
    LayerMask _groundLayer;
    #endregion

    #endregion

    #region accessors
    public bool Grounded { get { return (!(_isJumping || _isJumpFalling)); } }
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
        #region slide
        if(Time.time -_lastSlideTime > _md.slideDuration)
        {
            _isSliding = false;
        }

        if (_lastTimeSlideInput > 0)
        {
            Slide();
        }
        #endregion

        #region jump

        if (_lastJumpTime > 0 )
        {
            //Do Grounded Jump-----------------------------------------------------------------------
            if (_lastGroundedTime > 0 && !_isJumping)
            {
                Jump();
            }
            //DO WallJump----------------------------------------------------------------------------
            else if (_currentWallJumpNumber > 0 && !_isWallJumping)
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
        if (!CheckGrounded())
        {
            _lastGroundedTime -= Time.deltaTime;
        }
        else
        {
            _isWallJumping = false;
            _lastGroundedTime = _md.jumpCoyoteTime;
        }
        #endregion

        #region buffers
        _lastTimeSlideInput -= Time.deltaTime;
        _lastJumpTime -= Time.deltaTime;
        #endregion

        #region changeGravity
        UpdateGravity();
        #endregion
    }

    public void Run(float direction)
    {
        if (_isWallJumping || _isSliding) return;

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

    private void Jump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _md.jumpForce, ForceMode2D.Impulse);
        _isSliding = false;
        _isJumping = true;
        _lastJumpTime = 0;
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
        if (!(Time.time - _lastSlideTime > _md.timeBetweenSlides) || !CheckGrounded()) return;
        _lastSlideTime = Time.time;
        _isSliding = true;

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
        return Physics2D.OverlapBox((Vector2)transform.position + _yGroundCheckOffSet * Vector2.up, _groundCheckSize, 0, _groundLayer);
    }
    #endregion

    #region InputCallbacks
    public void JumpPressed()
    {
        _lastJumpTime = _md.jumpBufferTime;
    }

    public void JumpReleased()
    {
        if(_isJumping)
        {
            _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _md.jumpCutMultiplier), ForceMode2D.Impulse);
        }

        _lastJumpTime = 0;
    }

    public void SlidePressed()
    {
        _lastTimeSlideInput = _md.slideBufferTime;
        Slide();
    }
    #endregion


    #region EDITOR METHODS
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + _yGroundCheckOffSet * Vector2.up, _groundCheckSize);
    }
    #endregion


    #endregion
}
