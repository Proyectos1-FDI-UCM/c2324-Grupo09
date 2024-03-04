using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BrittleWallComponent : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ProyectileMovement>() != null)
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
