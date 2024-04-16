using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: SerializeField] public EventReference WallJump {  get; private set; }
    [field: SerializeField] public EventReference Slide { get; private set; }
    [field: SerializeField] public EventReference Trampoline { get; private set; }
    [field: SerializeField] public EventReference Death { get; private set; }
    [field: SerializeField] public EventReference CollectAbility { get; private set; }
    [field: SerializeField] public EventReference EnemyDeath { get; private set; }
    [field: SerializeField] public EventReference Steps { get; private set; }
    [field: SerializeField] public EventReference Music { get; private set; }
    [field: SerializeField] public EventReference Pogo { get; private set; }
    [field: SerializeField] public EventReference FallingPlatform { get; private set; }

    [field: SerializeField] public EventReference MenuMusic { get; private set; }
    [field: SerializeField] public EventReference BossMusic { get; private set; }

    static public FMODEvents Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
}
