using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class RefactoredCharacterController : MonoBehaviour
{
    #region references
    ChangeCollider _changeCollider;
    private HitboxComponent _hitbox;
    private RefactoredCharacterMovement _chMovement;
    private MovementData _md;
    //reference to animation component
    private AnimationComponent _animComp;
    #endregion

    #region inputVariables
    //x value of the movement joystick
    int xInput = 0;
    #endregion

    #region hiddenVariables
    //bool _flipComplete = true;
    bool _isGrounded = false;
    bool _isJumping = false;
    bool _isSliding = false;
    bool _isJumpFalling = false;
    bool _isWallJumping = false;
    bool _isUsingPogo = false;
    bool _pogoAnimationCompleted = false;
    bool _canPogoJump = false;
    bool _canWallRun = false;
    bool _isWallRunning = false;
    bool _wallRunHeld = false;
    //float _xVelocityPreviousToWallJump = 0;
    int _remainingWallJumpNumber;
    //stores the rigidbody velocity when the pogo button is pressed
    Vector3 _pogoStartVelocity = Vector3.zero;


    #region Timers
    float _lastGroundedTime = 0f;
    //jump buffer
    float _lastJumpTimeInput = 0f;
    //timer for slide cooldown
    float _lastSlideTime = -10f;
    //slide buffer
    float _lastTimeSlideInput = 0f;
    //stores time mark when the pogo button is pressed
    float _pogoStartTime = 0f;
    //stores time mark when you touch ground and _isUsingPogo == true
    float _pogoTouchedGround = 0f;
    float _lastWallJumpImpulse = -2f;
    //float _redirTiming = 0f;
    #endregion

    #endregion


    #region InputCallbacks

    //Triggers on pressing the jump key
    public void JumpDown()
    {
        _lastJumpTimeInput = _md.jumpBufferTime;
    }

    //Triggers on pressing the slide/pogo key
    public void SlideDown()
    {
        _lastTimeSlideInput = _md.slideBufferTime;
        
    }
  public void  WallRunDown() 
    {
        _wallRunHeld = true;
    }

    //Triggers on changing the value of the x movement joystick
    public void RunDown(float move)
    {
        xInput = (int)move;

    }

    //Triggers on releasing jump button
    public void JumpUp()
    {
        if (_isJumping)
        {
            _chMovement.JumpCut();
        }

        _lastJumpTimeInput = 0;
    }

    //Triggers on releasing slide button
    public void SlideUp()
    {
        _lastTimeSlideInput = 0;
        
    }
    public void WallRunUp()
    {
        _wallRunHeld = false;
    }
    public void WallRunToggle(bool toggle)
    {
        _canWallRun = toggle;
    }

    //triggers when the value of the x movement is equal to 0
    public void RunUp(float move)
    {
        xInput = (int)move;

    }
    

    #endregion




    void Start()
    {
        _chMovement = GetComponent<RefactoredCharacterMovement>();
        _md = _chMovement.md;
        _animComp = GetComponentInChildren<AnimationComponent>();
        _hitbox = GetComponent<HitboxComponent>();
        _changeCollider = GetComponent<ChangeCollider>();
        _remainingWallJumpNumber = _md.maxNumberOfWallJumps;
    }

    //Runs 50 times per second
    void FixedUpdate()
    {
        _isGrounded = CheckGrounded();

        #region WallRun
        if (_wallRunHeld && _canWallRun && !_isWallRunning && !_isUsingPogo)
        {
            _chMovement.WallRunStart();
            _isWallRunning = true;
        }
        else if ((!_wallRunHeld || !_canWallRun) && _isWallRunning)
        {
            _chMovement.WallRunEnd();
            _isWallRunning = false;
        }
        else if (_isWallRunning)
        {
            _chMovement.AddGravityScale(_md.gravityGain);
        }
        #endregion

        #region slide
        if (Time.time - _lastSlideTime > _md.slideDuration)
        {
            _isSliding = false;
            _changeCollider.EndSlide();
        }
        if (_lastTimeSlideInput > 0 && !_isWallRunning)
        {
            if (Time.time - _lastSlideTime > _md.timeBetweenSlides && _isGrounded)
            {
                _animComp.LookTo1D(_chMovement.LastDirection);
                _chMovement.Slide();
                _changeCollider.StartSlide();

                _lastSlideTime = Time.time;
                _isSliding = true;
                _isUsingPogo = false;
                _lastTimeSlideInput = -1;
                _pogoTouchedGround = -1;
            }
            else if (!_isUsingPogo)
            {
                if ( _chMovement.RBVel.y > 0 || !Physics2D.OverlapBox((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, new Vector2(_md.groundCheckSize.x, _md.minPogoHeight), 0, _md.groundLayer))
                {
                    _hitbox.DisableHitbox();
                    _animComp.SetPogoTr();
                    _isUsingPogo = true;
                    _pogoStartTime = Time.time;
                    _pogoStartVelocity = _chMovement.RBVel;
                    _pogoAnimationCompleted = false;
                }
            }
        }
        #endregion

        #region jump

        if (_hitbox.HitboxHit && _isWallJumping)
        {
            _isWallJumping = false;
            _lastWallJumpImpulse = Time.time;
            _lastJumpTimeInput = -1;
            _chMovement.WallJumpPart2();
        }

        if (_lastJumpTimeInput > 0)
        {
            //Do Grounded Jump-----------------------------------------------------------------------
            if (_lastGroundedTime > 0 && !_isJumping)
            {
                if (_canPogoJump)
                {
                    _chMovement.PogoJump();
                }
                else
                {
                    _animComp.LookTo1D(_chMovement.LastDirection);
                    _chMovement.Jump();
                    _changeCollider.EndSlide();
                }

                _pogoTouchedGround = -1;
                _isSliding = false;
                _isJumping = true;
                _lastJumpTimeInput = 0;
                _lastGroundedTime = 0;
            }
            else if (_remainingWallJumpNumber > 0 && !_isWallJumping && !_isUsingPogo)
            {
                if (_chMovement.RBVel.y > 0 || !Physics2D.OverlapBox((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, new Vector2(_md.groundCheckSize.x, _md.minPogoHeight), 0, _md.groundLayer))
                {
                    _chMovement.WallJump();
                    _hitbox.CreateHitbox(_chMovement.LastDirection); //IMPORTANTE: Habrá que emplear la variante de 4 parámetros en el futuro.
                    _lastJumpTimeInput = -1;
                    _isWallJumping = true;
                    _remainingWallJumpNumber--;
                }
            }
        }
        #endregion

        #region jumpChecks
        if (_chMovement.RBVel.y < 0.01f && _lastGroundedTime < 0)
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


        #region SorroundingChecksUpdate
        if (!_isGrounded)
        {
            _lastGroundedTime -= Time.deltaTime;
        }
        else
        {
            _hitbox.DisableHitbox();
            if (_isUsingPogo)
            {
               _isUsingPogo = false;
               _pogoTouchedGround = Time.time;
               _animComp.LookTo1D(_chMovement.LastDirection);
            }
            _isWallJumping = false;
            _lastGroundedTime = _md.jumpCoyoteTime;
            _remainingWallJumpNumber = _md.maxNumberOfWallJumps;
        }
        #endregion
        _canPogoJump = Time.time - _pogoTouchedGround <= _md.pogoEmpoweredJumpDuration && _pogoAnimationCompleted; 

        #region buffers
        if (!_isUsingPogo)
        {
            _lastTimeSlideInput -= Time.deltaTime;
            _lastJumpTimeInput -= Time.deltaTime;
        }



        /*
        if (_chMovement.Direction != _chMovement.LastDirection && _flipComplete)
        {
            Debug.Log("Flip");
            _redirTiming = _md.redirectionMargin;
            _flipComplete = false;
        }

        if (_redirTiming > 0)
        {
            _redirTiming -= Time.deltaTime;
        }
        else
        {
            _chMovement.DelayedDirection();
            _flipComplete = true;
        } 

        */

        #endregion




        #region changeGravity
        if (!_isWallRunning)
        {
            if ((_isJumping || _isJumpFalling) && Mathf.Abs(_chMovement.RBVel.y) < _md.jumpHangTimeThreshold)
            {
                _chMovement.ChangeGravityScale(_md.gravityScale * _md.jumpHangGravityMultiplier);
            }
            else if (_chMovement.RBVel.y < 0)
            {
                _chMovement.ChangeGravityScale(_md.gravityScale * _md.fallGravityMultiplier);
                //limitar la velocidad máxima de caida
                _chMovement.LimitMaxFallSpeed();
            }
            else
            {
                _chMovement.ChangeGravityScale(_md.gravityScale);
            }
            #endregion
        }

        #region Pogo
        if (_isUsingPogo)
        {
            if (Time.time - _pogoStartTime < _md.pogoXDuration)
            {
                _chMovement.ChangePlayerVelocity( new Vector2(Mathf.Lerp(_pogoStartVelocity.x, 0, (Time.time - _pogoStartTime) / _md.pogoXDuration), _chMovement.RBVel.y));
            }
            if (Time.time - _pogoStartTime < _md.pogoYDuration)
            {
                _chMovement.ChangePlayerVelocity( new Vector2(_chMovement.RBVel.x, Mathf.Lerp(_pogoStartVelocity.y, _md.pogoInitialUpVel, (Time.time - _pogoStartTime) / _md.pogoYDuration)));
            }
            else
            {
                _chMovement.AddImpulseForceToPlayer(Vector2.down * _md.pogoFallForce);
                if (!_pogoAnimationCompleted)
                    _pogoAnimationCompleted = true;
            }
        }
        #endregion

        float targetSpeed = xInput * _md.maxMoveSpeed;

        if (!_canPogoJump && !_isWallRunning) _chMovement.Run
        (
            targetSpeed,
            _isWallJumping || _isSliding || _isUsingPogo || Time.time - _lastWallJumpImpulse < _md.blockMovement2ndJumpTime,
            (_isJumping || _isJumpFalling), 
            _md.doConserveMomentum && Mathf.Abs(_chMovement.RBVel.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(_chMovement.RBVel.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && _lastGroundedTime < 0,
            ((_isJumping || _isJumpFalling) && Mathf.Abs(_chMovement.RBVel.y) < _md.jumpHangTimeThreshold)
        );




        #region Animator
        //changes the direction the player is facing
        /* This should be the scale of the sprite child of this gameObject
        if(xInput != 0)
        {
            transform.localScale = new Vector3(xInput * 1, 1, 1); 
        }
        */
        //activates the run action of character movement
        //Updating animation parameters
        if (xInput != 0 && (!_isWallJumping && !_isSliding && !_isUsingPogo && !_canPogoJump && !(Time.time - _lastWallJumpImpulse < _md.blockMovement2ndJumpTime)))
            _animComp.LookTo1D(xInput);

        _animComp.UpdateXInput(xInput);
        _animComp.SetWJ(_isWallJumping);
        _animComp.SetVelocityY(_chMovement.RBVel.y);
        _animComp.SetGrounded(_isGrounded);
        _animComp.SetSlide(_isSliding);
        _animComp.SetPogo(_isUsingPogo);
        _animComp.SetPogoCharge(_canPogoJump);
        _animComp.SetWallRun(_isWallRunning);
        #endregion
    }


    #region SorroundingChecks
    /// <summary>
    /// </summary>
    /// <returns>if player ground hitbox is touching ground</returns>
    private bool CheckGrounded()
    {
        return Physics2D.OverlapBox((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, _md.groundCheckSize, 0, _md.groundLayer);
    }

    #region EDITOR METHODS
    /// <summary>
    /// draws Gizmos showing character ground hitbox
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, _md.groundCheckSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + _md.yGroundCheckOffSet * Vector2.up, new Vector2(_md.groundCheckSize.x, _md.minPogoHeight));
    }
    #endregion

    #endregion
}
