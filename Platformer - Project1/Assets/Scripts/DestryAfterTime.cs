using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryAfterTime : MonoBehaviour
{
    [SerializeField]
    float secondsToDestroy = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("DestroyThis", secondsToDestroy);
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject); 
    }
}
