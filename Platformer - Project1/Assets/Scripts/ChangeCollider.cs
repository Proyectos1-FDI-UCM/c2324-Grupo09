using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollider : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    [SerializeField]
    Vector2 _newOffset;
    [SerializeField]
    Vector2 _newSize;

    // Start is called before the first frame update
    Vector2 _oldOffset;
    Vector2 _oldSize;
        void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _oldOffset = _boxCollider.offset;
        _oldSize = _boxCollider.size;
    }

    // Update is called once per frame
   public void StartSlide()
    {
       
        _boxCollider.offset = _newOffset;
        _boxCollider.size =_newSize;   
    }
    public void EndSlide() 
    {
        _boxCollider.offset = _oldOffset;
        _boxCollider.size = _oldSize;
    }
}
