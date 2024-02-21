using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollider : MonoBehaviour
{
    private CapsuleCollider2D _capsuleCollider;
    // Start is called before the first frame update
    void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
   public void StartSlide()
    {
        _capsuleCollider.offset = new Vector2(0.4f,-5);
        _capsuleCollider.size = new Vector2(7, 6);   
    }
    public void EndSlide() 
    {
        _capsuleCollider.offset = new Vector2(0.4f,-2.56f);
        _capsuleCollider.size = new Vector2(7, 12);
    }
}
