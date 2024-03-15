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
    HinIA _hinIA;
    private void Start()
    {
        _impIA = GetComponent<ImpIA>();
        _beneIA = GetComponent<BeneIA>();
        _nahaIA = GetComponentInParent<NahaIA>();
        _hinIA = GetComponent<HinIA>();
        _changeState =GetComponent<ChangeState>();
    }
    public void GotHit()
    {
        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.EnemyDeath, this.transform.position);
        _impIA?.OnHit();
        _beneIA?.OnHit();
        _nahaIA?.OnHit();
        _hinIA?.OnHit();

        if (_destroyOnHit)
        {
            Destroy(gameObject);
        }
        if(_nahaIA != null)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }


}
