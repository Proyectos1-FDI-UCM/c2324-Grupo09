using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxComponent : MonoBehaviour
{
    private Transform _myTransform;
    //private AttackComponent _attackComponent;
    public bool HitboxHit { get { return _currentHitboxAlreadyHit;  } }

    private bool _currentHitboxAlreadyHit = false;
    [SerializeField]
    Vector2 _hitboxPosition;
    [SerializeField]
    Vector2 _hitboxSize;

    [SerializeField]
    private LayerMask _layerToCheck;

    public void CreateHitbox(Vector2 position, Vector2 size, int relativePosition, int hitboxNumber)
    {
        _currentHitboxAlreadyHit = false;
        _hitboxPosition = new Vector2(position.x * relativePosition, position.y);
        _hitboxSize = new Vector2(size.x * relativePosition, size.y);
    }

    public void DisableHitbox()
    {
        _hitboxSize = Vector2.zero;
    }

    /*
    public void CanHitAgain()
    {
        currentHitboxAlreadyHit = false;
    }
    */

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

    void CheckHit()
    {
        if 
        (
            Physics2D.OverlapBox(new Vector2(_myTransform.position.x + _hitboxPosition.x, _myTransform.position.y + _hitboxPosition.y), _hitboxSize, 0f, _layerToCheck)
        )
            _currentHitboxAlreadyHit = true;
    }

    void Update()
    {
        if (_currentHitboxAlreadyHit) return;
        CheckHit();
    }


}
