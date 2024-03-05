using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    [SerializeField]
    bool _destroyOnHit = false;
    ChangeState _changeState;
    ImpIA _impIA;
    BeneIA _beneIA;
    NahaIA _nahaIA;
    private void Start()
    {
        _impIA = GetComponent<ImpIA>();
        _beneIA = GetComponent<BeneIA>();
        _nahaIA = GetComponentInParent<NahaIA>();
        _changeState =GetComponent<ChangeState>();
    }
    public void GotHit()
    {
        _impIA?.OnHit();
        _beneIA?.OnHit();
        _nahaIA?.OnHit();
        if (_destroyOnHit)
        {
            Destroy(gameObject);
        }
    }


}
