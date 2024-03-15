using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    #region references
    private ParticleSystem _myParticles;
    #endregion
    void Start()
    {
        _myParticles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if(!_myParticles.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}
