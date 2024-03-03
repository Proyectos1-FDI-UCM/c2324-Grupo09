using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryAfterTime : MonoBehaviour
{
    [SerializeField]
    float secondsToDestroy = 0.5f;

    [SerializeField]
    GameObject[] somethingElseToDestroy; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("DestroyThis", secondsToDestroy);
    }

    private void DestroyThis()
    {
        Destroy(this.gameObject);
        for(int i = 0; i < somethingElseToDestroy.Length; i++)
        {
            somethingElseToDestroy[i].SetActive(false);
        }
    }


}
