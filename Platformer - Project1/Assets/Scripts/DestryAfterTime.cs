using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class DestryAfterTime : MonoBehaviour
{
    Transform _myTransform;

    [SerializeField]
    float secondsToDestroy = 0.5f;
    [SerializeField]
    GameObject[] somethingElseToDestroy;

    [SerializeField]
    bool beginToFallInstead = false;

    [SerializeField]
    [ShowIf("beginToFallInstead")]
    float fallGravity = 9.8f;
    [SerializeField]
    [ShowIf("beginToFallInstead")]
    float maxFallSpeed;
    bool falling = false;
    float currentGravity = 0;
    public float CurrentGravity {  get { return currentGravity; }}



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("DestroyThis", secondsToDestroy);
    }

    private void DestroyThis()
    {
        for(int i = 0; i < somethingElseToDestroy.Length; i++)
        {
            somethingElseToDestroy[i].SetActive(false);
        }
        if (beginToFallInstead)
            falling = true;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        _myTransform = transform;
    }

    void FixedUpdate()
    {
        if (falling)
        {
            currentGravity = Mathf.Min(currentGravity + fallGravity, maxFallSpeed);
            _myTransform.position += Vector3.down * Time.fixedDeltaTime * currentGravity;
        }
    }




}
