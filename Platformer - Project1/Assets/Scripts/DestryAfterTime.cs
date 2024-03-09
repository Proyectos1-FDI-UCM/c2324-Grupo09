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
    bool frozen = false;
    float currentGravity = 0;
    public float CurrentGravity {  get { return currentGravity; }}
    [SerializeField]
    private FallDirection fDirection= FallDirection.down;
    public int FallDirectionValue
    {
        get => (int)fDirection;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("DestroyThis", secondsToDestroy);
    }

    public void DestroyThis()
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
        if (falling && !frozen)
        {
            currentGravity = Mathf.Min(currentGravity + fallGravity, maxFallSpeed);
            _myTransform.position += Vector3.up * Time.fixedDeltaTime * (int)fDirection * currentGravity;
        }
    }

    public void reverseFallSpeed()
    {
        currentGravity = 0;
        fDirection = (FallDirection)((int)fDirection * -1);
    }

    public void NegateBeginToFallInstead()
    {
        beginToFallInstead = false;
    }

    public void SetFrozen(bool val)
    {
        frozen = val;
    }




}

enum FallDirection
{
    up = 1,
    down = -1
}