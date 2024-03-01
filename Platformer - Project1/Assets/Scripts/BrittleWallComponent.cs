using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BrittleWallComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.GetComponent<ProyectileMovement>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
