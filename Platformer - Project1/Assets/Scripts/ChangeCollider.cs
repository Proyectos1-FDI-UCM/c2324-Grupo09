using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollider : MonoBehaviour
{
    private CapsuleCollider2D _capsuleCollider;
    [SerializeField]
    Vector2 _newOffset;
    [SerializeField]
    Vector2 _newSize;

    // Start is called before the first frame update
    Vector2 _oldOffset;
    Vector2 _oldSize;
        void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _oldOffset = _capsuleCollider.offset;
        _oldSize = _capsuleCollider.size;
    }

    // Update is called once per frame
   public void StartSlide()
    {
       
        _capsuleCollider.offset = _newOffset;
        _capsuleCollider.size =_newSize;   
    }
    public void EndSlide() 
    {
        _capsuleCollider.offset = _oldOffset;
        _capsuleCollider.size = _oldSize;
    }
}
