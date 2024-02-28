using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class HitboxComponent : MonoBehaviour
{
    //reference to self transform
    private Transform _myTransform;
    //accesor to get the value of _currentHitboxAlreadyHit from outside of this component
    public int TargetHit { get { return _targetHit; } }
    //each time a hitbox is generated this is set to 0
    //when it is set to 1 it means it hit something before the hitbox dissapeared
    private int _targetHit = 0;
    //Vector 2 containing
    [SerializeField]
    Vector2 _hitboxPosition;

    //Vector 2 containing the x and y size of the box created with the hitbox
    [SerializeField]
    Vector2 _hitboxSize;


    //in inspector it lets you decide which layers it is going to detect
    [SerializeField]
    private LayerMask _layerToCheck;


    /// <summary>
    /// Turns on the hitbox and lets you decide the offset and size of the hitbox.
    /// </summary>
    /// <param name="position"> Offset assuming the character is looking right</param>
    /// <param name="relativePosition"> Multiplies the position.x by -1 or 1</param>
    /// <param name="size"> Total size of the box created</param>
    /// <param name="time"> Total time checking collisions within hitbox</param>

    public void CreateHitbox(Vector2 position, Vector2 size, int relativePosition)
    {
        _targetHit = 0;
        _hitboxPosition = new Vector2(position.x * Mathf.Sign(relativePosition), position.y);
        _hitboxSize = size;
    }

    ///<summary>
    ///Disables the hitbox
    ///Sets collision detected to 0.
    ///</summary>
    public void DisableHitbox()
    {
        _hitboxSize = Vector2.zero;
        _targetHit = 0;
    }


    /// <summary>
    /// If this gameObject is selected on editor lets you see the size and position of the hitbox.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector2(_myTransform.position.x + _hitboxPosition.x, _myTransform.position.y + _hitboxPosition.y), _hitboxSize);
    }

    void Start()
    {
        _myTransform = transform;

    }

    /// <summary>
    /// Returns if Hitbox touches something of the layer indicated on the serizalize field of the hitbox component
    /// </summary>
    int CheckHit()
    {
        //Debug.Log (Physics2D.OverlapBox(new Vector2(_myTransform.position.x + _hitboxPosition.x, _myTransform.position.y + _hitboxPosition.y), _hitboxSize, 0f, _layerToCheck).GetType());
        Collider2D overlap = Physics2D.OverlapBox(new Vector2(_myTransform.position.x + _hitboxPosition.x, _myTransform.position.y + _hitboxPosition.y), _hitboxSize, 0f, _layerToCheck);
        if (overlap != null && _hitboxSize != Vector2.zero)
        {
            if (overlap.GetComponent<EnemyMovement>() != null) 
            {
                overlap.GetComponent<EnemyHit>().GotHit();
                return 2; 
                
            }
            else return 1;
        }
        else return 0;
        
    }


    void FixedUpdate()
    {
        _targetHit = CheckHit();
    }


}
