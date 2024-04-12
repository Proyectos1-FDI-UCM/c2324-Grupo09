using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAnimatorComponent : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    [SerializeField]
    Sprite[] _spikeSprites;
    [SerializeField]
    int _fps = 8;
    //[SerializeField]
    float delay;
    int _currentFrame;
    float _timeSpent;

    // Start is called before the first frame update
    void Start()
    {
        delay = 1f / _fps;
        _timeSpent = 0;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currentFrame = Random.Range(0, _spikeSprites.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time-_timeSpent > delay)
        {
            _spriteRenderer.sprite = _spikeSprites[_currentFrame];
            if(_currentFrame == _spikeSprites.Length - 1)
            {
                _currentFrame = 0;
            }
            else
            {
                _currentFrame++;
            }

            _timeSpent = Time.time;
        }

    }
}
