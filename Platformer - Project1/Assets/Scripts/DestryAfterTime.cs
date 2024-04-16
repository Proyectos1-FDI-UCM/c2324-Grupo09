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

    Vector3 _originalPos;

    public int FallDirectionValue
    {
        get => (int)fDirection;
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<RefactoredCharacterController>() != null)
            StartCoroutine(DestroyThis());
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(secondsToDestroy);
        startDestroyingitForReal();
    }

    public void startDestroyingitForReal()
    {
        for (int i = 0; i < somethingElseToDestroy.Length; i++)
        {
            somethingElseToDestroy[i].SetActive(false);
        }
        if (beginToFallInstead)
        {
            falling = true;
            try { AudioManager.Instance?.PlayOneShot(FMODEvents.Instance.FallingPlatform, this.transform.position); }
            catch { Debug.Log("Falta el audio"); }
        }

        else
            if (this != null) Destroy(this?.gameObject);
    }

    void Start()
    {
        _myTransform = transform;
        _originalPos = _myTransform.position;
    }

    private void LateUpdate()
    {
        if (falling && !frozen)
        {
            currentGravity = Mathf.Min(currentGravity + fallGravity, maxFallSpeed);
            _myTransform.position += Vector3.up * Time.deltaTime * (int)fDirection * currentGravity;
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

    public void Reset()
    {
        StopAllCoroutines();
        falling = false;
        currentGravity = 0;
        _myTransform.position = _originalPos;
    }




}

enum FallDirection
{
    up = 1,
    down = -1
}