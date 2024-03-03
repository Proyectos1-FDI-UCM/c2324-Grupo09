using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLayerDetect : MonoBehaviour
{
    //[SerializeField]
   // private LayerMask m_Mask;
    private LayerMask _colisionLayer;
    private int i=7;
    RefactoredCharacterController m_CharacterController;
    // Start is called before the first frame update
    void Start()
    {
        
    }
   
    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.layer);

        _colisionLayer = collision.gameObject.layer;
        if (i == _colisionLayer) { Debug.Log("3"); }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.layer);
        _colisionLayer = collision.gameObject.layer;
        if (i == _colisionLayer) { Debug.Log("2"); }
       
    }
}

