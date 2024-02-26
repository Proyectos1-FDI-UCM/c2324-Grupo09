using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    [SerializeField]
    int _stateChanger;
    ChangeState _changeState;
    ImpIA _impIA;
    private void Start()
    {
        _impIA = GetComponent<ImpIA>();
        _changeState=GetComponent<ChangeState>();
    }
    public void GotHit()
    {
        _impIA.OnHit();
    }


}
