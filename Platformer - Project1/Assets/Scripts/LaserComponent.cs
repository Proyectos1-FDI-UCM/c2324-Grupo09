using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserComponent : MonoBehaviour
{
    [SerializeField]
    float _delayInitTime = 2f;
    bool _beginFalling = false;
    Transform _transform;
    float speed = 10f;

    void Start()
    {
        StartCoroutine(Laser());
        _transform = transform;
    }

    IEnumerator Laser()
    {
        yield return new WaitForSeconds(_delayInitTime);
        //_transform.localScale = Vector3.up * _transform.localScale.y * 50 + Vector3.right * _transform.localScale.x;
        //_transform.position += Vector3.down * (_transform.localScale.y / 2);
        _beginFalling = true;
        yield return new WaitForSeconds(_delayInitTime/2);

        Destroy(this.gameObject);

    }

    private void Update()
    {
        if (_beginFalling)
            _transform.position += Vector3.down * speed;
    }
}
