using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Movement Data", order = 10)]
public class MovementData : ScriptableObject
{
    //max Horizontal Speed
    public float maxMoveSpeed = 1f;
    //[Range(0.0F, 1.0F)]
    public float acceleration = 1f;
    //[Range(0.0F, 1.0F)]
    public float decceleration = 1f;
    [Range(0.0F, 1.0F)]
    public float accelInAir = 1f;
    [Range(0.0F, 1.0F)]
    public float deccelInAir = 1f;
    //permite ir a mayor velocidad de la normal en la dirección de movimiento actual
    public bool doConserveMomentum;

    public float jumpForce = 1f;
    //1/10 - 1/20 of a second
    public float jumpBufferTime = 0.1f;
    public float jumpCoyoteTime = 0.1f;

    [Range(0.0F, 1.0F)]
    public float jumpCutMultiplier = 0;

    [Range(1.0F, 10.0F)]
    public float jumpHangAccelerationMultiplier = 1;
    [Range(1.0F, 10.0F)]
    public float jumpHangMaxSpeedMultiplier = 1;



    public float gravityScale = 1;
    //mayor igual de 1
    public float fallGravityMultiplier = 1;
    public float maxFallSpeed = 10f;
    [Range(0F, 1F)]
    public float jumpHangTimeThreshold = 0;
    [Range(0F, 1F)]
    public float jumpHangGravityMultiplier = 1;
}
