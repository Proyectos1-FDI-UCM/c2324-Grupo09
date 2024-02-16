using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxComponent : MonoBehaviour
{
    //reference to self transform
    private Transform _myTransform;

    //accesor to get the value of _currentHitboxAlreadyHit from outside of this component
    public bool HitboxHit { get { return _currentHitboxAlreadyHit;  } }

    //each time a hitbox is generated this is set to 0
    //when it is set to 1 it means it hit something before the hitbox dissapeared
    private bool _currentHitboxAlreadyHit = false;

    [SerializeField]
    private float _defaultCheckTime = 0.5f;
    private float _checkTime;
    //Vector 2 containing
    [SerializeField]
    Vector2 _defaultPosition;
    Vector2 _hitboxPosition;

    //Vector 2 containing the x and y size of the box created with the hitbox
    [SerializeField]
    Vector2 _defaultSize;
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
    public void CreateHitbox(Vector2 position, Vector2 size, int relativePosition, float time)
    {
        _currentHitboxAlreadyHit = false;
        _hitboxPosition = new Vector2(position.x* Mathf.Sign(relativePosition), position.y);
    _hitboxSize = new Vector2(size.x* Mathf.Sign(relativePosition), size.y);
    _checkTime = time;
    }

    /// <summary>
    /// Uses a preset timer value.
    /// </summary>
    public void CreateHitbox(Vector2 position, Vector2 size, int relativePosition)
    {
        _currentHitboxAlreadyHit = false;
        _hitboxPosition = new Vector2(position.x * Mathf.Sign(relativePosition), position.y);
        _hitboxSize = new Vector2(size.x * Mathf.Sign(relativePosition), size.y);
        _checkTime = _defaultCheckTime;
    }

    /// <summary>
    /// Activates the hitbox with the values given in the inspector.
    /// </summary>
    public void CreateHitbox(int relativePosition, float time)
    {
        _currentHitboxAlreadyHit = false;
        _hitboxPosition = new Vector2(_defaultPosition.x* relativePosition, _defaultPosition.y);
    _hitboxSize = new Vector2(_defaultSize.x* relativePosition, _defaultSize.y);
    _checkTime = time;
    }

    /// <summary>
    /// Uses a preset time value
    /// </summary>
    public void CreateHitbox(int relativePosition)
    {
        _currentHitboxAlreadyHit = false;
        _hitboxPosition = new Vector2(_defaultPosition.x * relativePosition, _defaultPosition.y);
        _hitboxSize = new Vector2(_defaultSize.x * relativePosition, _defaultSize.y);
        _checkTime = _defaultCheckTime;

    }


    ///<summary>
    ///Disables the hitbox
    ///Sets collision detected to 0.
    ///</summary>
    public void DisableHitbox()
    {
        _hitboxSize = Vector2.zero;
        _currentHitboxAlreadyHit = false;
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
    bool CheckHit()
    {
        return Physics2D.OverlapBox(new Vector2(_myTransform.position.x + _hitboxPosition.x, _myTransform.position.y + _hitboxPosition.y), _hitboxSize, 0f, _layerToCheck);
    }

    void FixedUpdate()
    {

        if (_checkTime > 0 && !_currentHitboxAlreadyHit)
        {

            if (CheckHit())
            {
                Debug.Log("Hit");
                _currentHitboxAlreadyHit = true;
                _checkTime = 0;
            }
            else _checkTime -= Time.deltaTime;
        }
        else DisableHitbox();
    }


}
