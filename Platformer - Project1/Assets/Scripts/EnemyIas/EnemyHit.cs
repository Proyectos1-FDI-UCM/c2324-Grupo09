using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    ChangeState _changeState;
    ImpIA _impIA;
    BeneIA _beneIA;
    private void Start()
    {
        _impIA = GetComponent<ImpIA>();
        _beneIA = GetComponent<BeneIA>();
        _changeState =GetComponent<ChangeState>();
    }
    public void GotHit()
    {
        _impIA?.OnHit();
        _beneIA?.OnHit();
    }


}
