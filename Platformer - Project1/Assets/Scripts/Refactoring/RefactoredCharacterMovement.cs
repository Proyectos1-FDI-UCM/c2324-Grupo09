using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactoredCharacterMovement : MonoBehaviour
{
    #region references
    [SerializeField]
    private MovementData _md;
    private Rigidbody2D _rb;
    #endregion

    #region parameters
    private int _lastDirection;
    private int _wallJumpStartDirection;
    private float _xVelocityPreviousToWallJump;
    #endregion

    #region accesors
    public MovementData md { get { return _md; }}
    public Vector2 RBVel { get { return _rb.velocity; } }
    public int LastDirection { get { return _lastDirection; } }
    #endregion


    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Applies a force in direction to the player input.
    /// If moving to a superior velocity to the grounded max speed and character is not touching ground character can conserve momentum
    /// </summary>
    public void Run(float targetSpeed, bool movementIsBlocked, bool isOnAir, bool conditionToPreserveMomentum, bool isInJumpApex)
    {
        if (targetSpeed != 0) _lastDirection = (int)Mathf.Sign(targetSpeed);

        if (movementIsBlocked) return;

        //calculate the direction we want to move in and our desired velocity
        //float targetSpeed = direction * _md.maxMoveSpeed;
        float accelRate = 0;
        //-----------------------------------------------targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);
        
        
        //change acceleration rate depending on situation
        if (isOnAir)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? (_md.acceleration * _md.accelInAir) : (_md.decceleration * _md.deccelInAir);
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _md.acceleration : _md.decceleration;
        

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (conditionToPreserveMomentum)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //add bonus spead at the apex of a jump
        if (isInJumpApex)
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
    }


    /// <summary>
    /// Jumps, checks if character has just done a Pogo in order to give him an increase on the jump Force
    /// </summary>
    public void Jump()
    {
        //float jumpForceMultiplier = 1f;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        /*
         * if (Time.time - _pogoTouchedGround <= _md.pogoEmpoweredJumpDuration && _pogoAnimationCompleted)
        {
            jumpForceMultiplier = _md.empoweredJumpForceMultiplier;
            _lastTimeSlideInput = -1;
        }
        */
        _rb.AddForce(Vector2.up * _md.jumpForce, ForceMode2D.Impulse);
    }

    public void JumpCut()
    {
        _rb.AddForce(Vector2.down * _rb.velocity.y * (1 - _md.jumpCutMultiplier), ForceMode2D.Impulse);
    }

    public void PogoJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        /*
         * if (Time.time - _pogoTouchedGround <= _md.pogoEmpoweredJumpDuration && _pogoAnimationCompleted)
        {
            jumpForceMultiplier = _md.empoweredJumpForceMultiplier;
            _lastTimeSlideInput = -1;
        }
        */
        _rb.AddForce(Vector2.up * _md.jumpForce * _md.empoweredJumpForceMultiplier, ForceMode2D.Impulse);
    }

    #region walljump
    /// <summary>
    /// Generates the initial horizontal Force and blocks movement while it lasts
    /// </summary>
    public void WallJump()
    {
        float sameDirectionFactor = _md.wallJumpSameDirectionForceMultiplier;
        _wallJumpStartDirection = _lastDirection;
        if (Mathf.Sign(_wallJumpStartDirection) != Mathf.Sign(_rb.velocity.x) || Mathf.Abs(_rb.velocity.x) < _md.wallJumpForceApplyThreshold)
        {
            sameDirectionFactor = 1;
            _rb.velocity = new Vector2(0, _rb.velocity.y);
        }
        
        //_rb.velocity = new Vector2(_rb.velocity.x, 0);
        _rb.AddForce(Vector2.up * _md.wallJumpForce.y + sameDirectionFactor * _md.wallJumpForce.x * _lastDirection * Vector2.right, ForceMode2D.Impulse);
        //_isJumping = true;
    }

    public void WallJumpPart2()
    {
        Debug.Log("JumpPt2");
        _xVelocityPreviousToWallJump = Mathf.Abs(_rb.velocity.x) /* * -1 */ * (_wallJumpStartDirection);
        //inserte pausa de antes
        _rb.velocity = -1 * Vector2.right * _xVelocityPreviousToWallJump;
        _rb.AddForce(_md.wallJump2ndJumpForceY * Vector2.up - 
            Vector2.right * _lastDirection * _md.wallJump2ndJumpForceX, ForceMode2D.Impulse);
    }

    #endregion


    /// <summary>
    /// Initiates Slide, blocking movement
    /// Gives player a good boost on horizontal speed
    /// If current horizontal Speed is greater than a const the impulse recieved is multiplied by a reduction coeficient
    /// </summary>
    public void Slide()
    {
        float sameDirectionFactor = _md.slideSameDirectionForceMultiplier;
        if (Mathf.Sign(_lastDirection) != Mathf.Sign(_rb.velocity.x) || Mathf.Abs(_rb.velocity.x) < _md.slideForceApplyThreshold)
        {
            _rb.velocity = new Vector2(0, _rb.velocity.y);
            sameDirectionFactor = 1;
        }
        _rb.AddForce(sameDirectionFactor * Vector2.right * _lastDirection * _md.slideHorizontalForce, ForceMode2D.Impulse);
    }



    public void ChangeGravityScale(float newValue)
    {
        _rb.gravityScale = newValue;
    }

    public void LimitMaxFallSpeed()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Max(_rb.velocity.y, -_md.maxFallSpeed));
    }

    public void ChangePlayerVelocity(Vector2 newVel)
    {
        _rb.velocity = newVel;
    }

    public void AddImpulseForceToPlayer(Vector2 newVel)
    {
        _rb.AddForce(newVel, ForceMode2D.Impulse);
    }
}
