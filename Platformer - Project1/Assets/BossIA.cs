using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;

public class BossIA : MonoBehaviour
{
    [SerializeField]
    [OnValueChanged("StateChanged")]
    ///Estado actual del boss
    private BossStates currentBS = BossStates.Init;
    /// <summary>
    /// Array con todo los patrones del Boss ordenados debidamente
    /// </summary>
    private Action<int>[] _bossPatrons;
    /// <summary>
    /// Array con los patrones del boss en el orden en el que los va a ejecutar.
    /// </summary>
    private Action<int>[] _currentBossPatronSeries;
    /// <summary>
    /// Valor del patron por el que se llega el boos, referido a la posicion de la array de arriba
    /// </summary>
    private int _patronIndex = 0;
    [Header("Pinchos")]
    [SerializeField]
    private GameObject pinchosTecho;
    [SerializeField]
    private GameObject pinchosParedL;
    [SerializeField]
    private GameObject pinchosParedR;


    [Header("Pilars")]
    GameObject pilarPrefab;
    [SerializeField]
    private Transform _pilarReferenceTransform;
    [SerializeField]
    private float initialDelay = 1f;
    [SerializeField]
    private float timeUntilPilarsTouchCeiling = 0.3f;
    [SerializeField]
    private float delayTimeBetweenPilars = 0.3f;
    [SerializeField]
    private float destroyPilarDelay = 1f;
    private DestryAfterTime[] pilarReferences = new DestryAfterTime[27];

    [Header("HandsSweep")]
    GameObject stompingHandPrefab;
    [SerializeField]
    private float timeTillStompFalls = 1.5f;
    [SerializeField]
    private float stompingPrevisualize = 0.3f;
    [SerializeField]
    private float timeTillProjectileDestroy = 2f;
    [SerializeField]
    private Vector3 stompingHandSpawnOffset;



    void Start()
    {
        pilarPrefab = Resources.Load<GameObject>("Pilar");
        stompingHandPrefab = Resources.Load<GameObject>("StompingHand");

        //https://stackoverflow.com/questions/7712137/array-containing-methods
        _bossPatrons = new Action<int>[5];
        _bossPatrons[0] = EmergingWalls;    //Desbloqueado desde el principio
        _bossPatrons[1] = HandsSweep;       //Desbloqueado desde el principio
        _bossPatrons[2] = FallingBenes;     //Desbloqueado tras golpear 1 vez al boss
        _bossPatrons[3] = OpossingNahas;    //Desbloqueado tras golpear 2 veces al boss
        _bossPatrons[4] = BlueImpOne;       //Patr�n que le permite recibir da�o y que siempre
                                            //se ejecutar� al final de la serie de patrones generados.

        GetNewPatronSeries();
        UseNextPatron();
        //_bossPatrons[0]((int)currentBS);
    }

    /// <summary>
    /// Esto se llama al recibir un golpe y tener que avanzar a la siguiente fase del boss.
    /// </summary>
    void NextBossState()
    {
        currentBS = (BossStates)((int)currentBS--);
        GetNewPatronSeries();
    }

    /// <summary>
    ///Llama al siguiente patr�n. En caso de haber llegado al �ltimo vuelve a shufflear
    //entre los patrones de este estado y vuelve a empezar
    /// </summary>
    void UseNextPatron()
    {
        if (_patronIndex != 5 - (int)currentBS)
        {
            _patronIndex++;
        }
        else
        {
            GetNewPatronSeries();
        }
        _currentBossPatronSeries[_patronIndex]((int)currentBS);
    }

    /// <summary>
    /// Genera una nueva serie con los patrones que tiene desbloqueados el Boss en la fase actual
    /// </summary>
    void GetNewPatronSeries()
    {
        _patronIndex = -1;
        int[] positionsShuffled = new int[5 - (int)currentBS];
        _currentBossPatronSeries = new Action<int>[6 - (int)currentBS];
        for (int i = 0; i < positionsShuffled.Length; i++)
        {
            positionsShuffled[i] = i;
        }
        Randomizer.Shuffle(positionsShuffled);
        string order = "";
        for (int i = 0; i < positionsShuffled.Length; i++)
        {
            order += (positionsShuffled[i] + ", ");
        }
        Debug.Log(order);
        for (int i = 0; i < positionsShuffled.Length; i++)
        {
            _currentBossPatronSeries[i] = _bossPatrons[positionsShuffled[i]];
        }
        _currentBossPatronSeries[positionsShuffled.Length] = _bossPatrons[4];
    }

    #region BossPatronFunctions

    private void EmergingWalls(int i)
    {
        if (i == 3)
        {
            StartCoroutine("SpawnPilarsLVL1");
        }
    }

    private void HandsSweep(int i)
    {
        if (i == 3)
        {
            StartCoroutine("HandSweepLVL1");
        }
    }

    private void FallingBenes(int i)
    {

    }

    private void OpossingNahas(int i)
    {

    }
 
    private void BlueImpOne(int i)
    {

    }

    #region auxiliarBossPatronFunctions

    #region Pillars
    /// <summary>
    /// spawnea pilares del centro de la pantalla a los extremos y pasado un tiempo los elimina
    /// LVL1
    /// </summary>
    IEnumerator SpawnPilarsLVL1()
    {
        pinchosTecho.SetActive(true);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);

        pilarReferences[0] = Instantiate(pilarPrefab, _pilarReferenceTransform.position, Quaternion.identity, _pilarReferenceTransform).GetComponent<DestryAfterTime>();
        yield return new WaitForSeconds(initialDelay);
        pilarReferences[0].DestroyThis();
        StartCoroutine("FrozePillar", 0);
        int i = 1;
        while (i != pilarReferences.Length)
        {

            yield return new WaitForSeconds(delayTimeBetweenPilars);
            pilarReferences[i]   = Instantiate(pilarPrefab, _pilarReferenceTransform.position + (i * 4 * Vector3.right), Quaternion.identity, _pilarReferenceTransform).GetComponent<DestryAfterTime>();
            pilarReferences[i].DestroyThis();
            StartCoroutine("FrozePillar",i);
            pilarReferences[i+1] = Instantiate(pilarPrefab, _pilarReferenceTransform.position + (i * 4 * Vector3.left), Quaternion.identity, _pilarReferenceTransform).GetComponent<DestryAfterTime>();
            pilarReferences[i+1].DestroyThis();
            StartCoroutine("FrozePillar", i+1);
            i += 2;
        }

        yield return new WaitForSeconds(destroyPilarDelay);

        pilarReferences[0].SetFrozen(false);
        pilarReferences[0].reverseFallSpeed();
        i = 1;
        while (i != pilarReferences.Length)
        {

            yield return new WaitForSeconds(delayTimeBetweenPilars);
            pilarReferences[i].SetFrozen(false);
            pilarReferences[i].reverseFallSpeed();
            pilarReferences[i + 1].SetFrozen(false);
            pilarReferences[i + 1].reverseFallSpeed();
            i += 2;
        }

        yield return new WaitForSeconds(destroyPilarDelay);

        pilarReferences[0].NegateBeginToFallInstead();
        pilarReferences[0].DestroyThis();
        i = 1;
        while (i != pilarReferences.Length)
        {

            yield return new WaitForSeconds(delayTimeBetweenPilars);
            pilarReferences[i].NegateBeginToFallInstead();
            pilarReferences[i].DestroyThis();
            pilarReferences[i + 1].NegateBeginToFallInstead();
            pilarReferences[i + 1].DestroyThis();
            i += 2;
        }

        //Patr�n se ha terminado 

        UseNextPatron();
    }

    IEnumerator FrozePillar(int i)
    {
        yield return new WaitForSeconds(timeUntilPilarsTouchCeiling);
        pilarReferences[i].SetFrozen(true);
    }
    #endregion

    IEnumerator HandSweepLVL1()
    {
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(true);
        pinchosParedR.SetActive(true);
        int rdNumber = (int)Mathf.Sign(UnityEngine.Random.Range(-1, 1));
        StompingHandIA stompingHand = Instantiate(stompingHandPrefab, _pilarReferenceTransform.position + stompingHandSpawnOffset.y * Vector3.up + (stompingHandSpawnOffset.x * Vector3.right * rdNumber), Quaternion.identity, _pilarReferenceTransform).GetComponent<StompingHandIA>();
        yield return new WaitForSeconds(timeTillStompFalls);
        stompingHand.PrevisualizeStomp();
        yield return new WaitForSeconds(stompingPrevisualize);
        stompingHand.BeginToFall(rdNumber * -1);
        yield return new WaitForSeconds(timeTillProjectileDestroy);
        stompingHand.ProjectileDestroy();
        stompingHand.DestroySelf();
        UseNextPatron();
        //--------------------------------------------------------------------------------------------------UseNextPatron();
    }

    #endregion
    #endregion
}

enum BossStates
{
    Init = 3,
    Damaged = 2,
    Wraithed = 1,
    Dead = 0
}

public static class  Randomizer
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}