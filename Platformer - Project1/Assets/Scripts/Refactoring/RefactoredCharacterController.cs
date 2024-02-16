using System.Collections;
using System.Collections.Generic;
using System.IO.Compression;
using UnityEngine;

public class RefactoredCharacterController : MonoBehaviour
{
    #region references

    private RefactoredCharacterMovement _chMovement;
    private MovementData _md;
    //reference to animation component
    private AnimationComponent _animComp;
    #endregion

    #region inputVariables
    //x value of the movement joystick
    int xInput;
    #endregion

    #region hiddenVariables
    [SerializeField]
    bool _isGrounded = false;
    [SerializeField]
    bool _isJumping = false;
    [SerializeField]
    bool _isSliding = false;
    [SerializeField]
    bool _isJumpFalling = false;
    [SerializeField]
    bool _isWallJumping = false;
    [SerializeField]
    bool _isUsingPogo = false;
    [SerializeField]
    bool _pogoAnimationCompleted = false;
    //float _xVelocityPreviousToWallJump = 0;
    int _remainingWallJumpNumber;
    //stores the rigidbody velocity when the pogo button is pressed
    Vector3 _pogoStartVelocity = Vector3.zero;


    #region Timers
    [SerializeField]
    float _lastGroundedTime = 0f;
    //jump buffer
    [SerializeField]
    float _lastJumpTimeInput = 0f;
    //timer for slide cooldown
    [SerializeField]
    float _lastSlideTime = -10f;
    //slide buffer
    [SerializeField]
    float _lastTimeSlideInput = 0f;
    //stores time mark when the pogo button is pressed
    [SerializeField]
    float _pogoStartTime = 0f;
    //stores time mark when you touch ground and _isUsingPogo == true
    [SerializeField]
    float _pogoTouchedGround = 0f;
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
        _remainingWallJumpNumber = _md.maxNumberOfWallJumps;
    }

    //Runs 50 times per second
    void FixedUpdate()
    {
        _isGrounded = CheckGrounded();

        #region slide
        if (Time.time - _lastSlideTime > _md.slideDuration)
        {
            _isSliding = false;
        }
        if (_lastTimeSlideInput > 0)
        {
            if (Time.time - _lastSlideTime > _md.timeBetweenSlides && _isGrounded)
            {
                _chMovement.Slide();

                _lastSlideTime = Time.time;
                _isSliding = true;
                _isUsingPogo = false;
                _lastTimeSlideInput = -1;
                _pogoTouchedGround = -1;
            }
            else if (!_isUsingPogo)
            {
                _isUsingPogo = true;
                _pogoStartTime = Time.time;
                _pogoStartVelocity = _chMovement.RBVel;
                _pogoAnimationCompleted = false;
            }
        }
        #endregion

        #region jump
        if (_lastJumpTimeInput > 0)
        {
            //Do Grounded Jump-----------------------------------------------------------------------
            if (_lastGroundedTime > 0 && !_isJumping)
            {
                if (Time.time - _pogoTouchedGround <= _md.pogoEmpoweredJumpDuration && _pogoAnimationCompleted)
                {
                    _chMovement.PogoJump();
                }
                else
                {
                    _chMovement.Jump();
                }

                _pogoTouchedGround = -1;
                _isSliding = false;
                _isJumping = true;
                _lastJumpTimeInput = 0;
                _lastGroundedTime = 0;
            }
            //DO WallJump----------------------------------------------------------------------------
            else if (_remainingWallJumpNumber > 0 && !_isWallJumping && !_isUsingPogo)
            {
                _chMovement.WallJump();
                _lastJumpTimeInput = -1;
                _isWallJumping = true;
            }//--------PROVISIONAL------------------------------------------------------------------------------
            
            
            else if (_isWallJumping)  //------------------
            {                           //------------------
                _isWallJumping = false;     //-----------------
                _lastJumpTimeInput = -1;    //------------------
                                            //-------------------
                _chMovement.WallJumpPart2();    //----------------------------------------
            }
            //__--------------------------------------------------------------------------------------
            
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
        if ((_isJumping || _isJumpFalling) && Mathf.Abs(_chMovement.RBVel.y) < _md.jumpHangTimeThreshold)
        {
            _chMovement.ChangeGravityScale(_md.gravityScale * _md.jumpHangGravityMultiplier);
        }
        else if (_chMovement.RBVel.y < 0)
        {
            _chMovement.ChangeGravityScale(_md.gravityScale * _md.fallGravityMultiplier);
            //limitar la velocidad m�xima de caida
            _chMovement.LimitMaxFallSpeed();
        }
        else
        {
            _chMovement.ChangeGravityScale(_md.gravityScale);
        }
        #endregion

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

        _chMovement.Run
        (
            targetSpeed,
            _isWallJumping || _isSliding || _isUsingPogo,
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
        if(xInput != 0)
            _animComp.LookTo1D(xInput);

        _animComp.UpdateXInput(xInput);
        _animComp.SetVelocityY(_chMovement.RBVel.y);
        _animComp.SetGrounded(_isGrounded);
        _animComp.SetSlide(_isSliding);
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
    }
    #endregion

    #endregion
}
