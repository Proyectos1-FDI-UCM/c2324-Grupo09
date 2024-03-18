using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using Unity.VisualScripting;

public class BossIA : MonoBehaviour
{
    //private Transform playerTransform;

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

    [SerializeField]
    private float timeTillPatronStartLVL1 = 2f;

    [SerializeField]
    private BoxCollider2D _bossHitbox;

    [Header("Pinchos")]
    [SerializeField]
    private GameObject pinchosTecho;
    [SerializeField]
    private GameObject pinchosParedL;
    [SerializeField]
    private GameObject pinchosParedR;
    [SerializeField]
    private GameObject pinchosSuelo;


    [Header("Pilars")]
    [SerializeField]
    private Transform _pilarReferenceTransform;
    [SerializeField]
    private float timeUntilPilarsTouchCeiling = 0.3f;
    [SerializeField]
    private float delayTimeBetweenPilars = 0.3f;
    [SerializeField]
    private float destroyPilarDelay = 1f;
    private DestryAfterTime[] pilarReferences = new DestryAfterTime[27];
    GameObject pilarPrefab;

    [Header("HandsSweep")]
    [SerializeField]
    private float stompingPrevisualize = 0.3f;
    [SerializeField]
    private float timeTillProjectileDestroy = 2f;
    [SerializeField]
    private Vector3 stompingHandSpawnOffset;
    GameObject stompingHandPrefab;
    StompingHandIA stompingHand;

    [Header("BlueImp")]
    [SerializeField]
    private float timeBetweenImps = 0.5f;
    [SerializeField]
    private float impSpeed = 50f;
    [SerializeField]
    private float yImpSpawnOffset = -20f;
    [SerializeField]
    private Vector3 HeadOffset;
    private GameObject bossHead;
    private GameObject eSpawner;
    GameObject head;
    EnemySpawner[] eS = new EnemySpawner[0];


    [Header("OpposingNahas")]
    [SerializeField]
    private GameObject restrictingWall;
    [SerializeField]
    float _yNagasSeparation = 20f;
    [SerializeField]
    float _yNagasSpawnOffset = 120f;
    [SerializeField]
    float _xNagasSpawnOffset = 70f;
    [SerializeField]
    float _timeBetweenNagas = 1f;

    [Header("HugeHand")]
    [SerializeField]
    float _yHugeHandOffset = 120;
    [SerializeField]
    float _ySweepingHandOffset = 0;
    [SerializeField]
    float _stompinhHandSizeMultiplier = 4f;
    GameObject _sweepingHandPrefab;
    GameObject _sweepingHand;
    Transform _playerTransform;

    [Header("Lasers")]
    [SerializeField]
    Transform _limitL;
    [SerializeField]
    Transform _limitR;
    GameObject _laserPrefab;

    /*
    [Header("Jumpers")]
    private GameObject jumperPrefab;

    */

    IEnumerator SpawnLasers()
    {
        while (BossStates.Wraithed == currentBS)
        {
            Debug.Log("Lasers");
            Instantiate(_laserPrefab, new Vector3(UnityEngine.Random.Range(_limitL.position.x, _limitR.position.x), _limitL.position.y, _limitL.position.z), Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.6f, 2f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ProyectileMovement>() != null)
        {
            ReceiveDamage();
        }
    }

    /// <summary>
    /// Para todos los procesos y espera a la señal del animator para continuar
    /// </summary>
    public void PlayerDied()
    {
        KillEverythingOnScreen();
        //-------------------------------------------------------provisional
        StartCoroutine(Restart());
        
        //------------------------------------------------------------------
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        _patronIndex = 0;
        GetNewPatronSeries();
        UseNextPatron();
        if (currentBS == BossStates.Wraithed) StartCoroutine(SpawnLasers());
    }

    /// <summary>
    /// Mata todo lo que haya en pantalla
    /// </summary>
    private void KillEverythingOnScreen()
    {

        StopAllCoroutines();

        for (int i = 0; i < pilarReferences.Length; i++)
        {
            pilarReferences[i]?.NegateBeginToFallInstead();
            pilarReferences[i]?.DestroyThis();
        }
        pilarReferences = new DestryAfterTime[27];
        stompingHand?.ProjectileDestroy();
        stompingHand?.DestroySelf();
        Destroy(_sweepingHand);
        stompingHand = null;

        for (int i = 0; i < eS.Length; i++)
        {
            eS[i]?.DestroySpawnedEnemy();
            Destroy(eS[i]?.gameObject);
        }
        eS = new EnemySpawner[0];
        if(head != null) Destroy(head);
        head = null;
        _bossHitbox.enabled = false;

        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);
        pinchosSuelo.SetActive(false);
    }


    /// <summary>
    /// Este metodo debería reiniciar el estado y hacer la animación de me has dado
    /// Esto o no interrumpir las corroutines, solo hacer que parpadee x segundos para denotarlo sin romper flujo de nada
    /// </summary>
    private void ReceiveDamage()
    {
        StopAllCoroutines();
        _bossHitbox.enabled = false;
        KillEverythingOnScreen();
        NextBossState();
    }

    void Start()
    {
        //playerTransform = FindObjectOfType<RefactoredCharacterController>().transform;

        _bossHitbox = GetComponent<BoxCollider2D>();
        _bossHitbox.enabled = false;
        pilarPrefab = Resources.Load<GameObject>("Pilar");
        stompingHandPrefab = Resources.Load<GameObject>("StompingHand");
        bossHead = Resources.Load<GameObject>("BossHead");
        eSpawner = Resources.Load<GameObject>("Spawner");
        _laserPrefab = Resources.Load<GameObject>("Laser");
        _sweepingHandPrefab = Resources.Load<GameObject>("SweepingHand");
        _playerTransform = FindObjectOfType<RefactoredCharacterController>().transform;
        //jumperPrefab = Resources.Load<GameObject>("JumpPad");

        //https://stackoverflow.com/questions/7712137/array-containing-methods
        _bossPatrons = new Action<int>[5];
        _bossPatrons[0] = HugeHandsSweep;   //Desbloqueado desde el principio
        _bossPatrons[1] = HandsSweep;       //Desbloqueado desde el principio
        _bossPatrons[2] = EmergingWalls;     //Desbloqueado tras golpear 1 vez al boss
        _bossPatrons[3] = HandsSweep;    //Desbloqueado tras golpear 2 veces al boss
        _bossPatrons[4] = BlueImpOne;       //Patrón que le permite recibir daño y que siempre
                                            //se ejecutará al final de la serie de patrones generados.

        //StartCoroutine("OpposingNagasLVL1");

        //StartCoroutine(HugeHandSweepLVL1());
        //_bossPatrons[4](3);

        //------------------------------------------------------------------------------------------------------------------------------------------------------
        if (currentBS == BossStates.Wraithed) StartCoroutine(SpawnLasers());
        GetNewPatronSeries();
        UseNextPatron();

        
        //_bossPatrons[0]((int)currentBS);
    }

    /// <summary>
    /// Esto se llama al recibir un golpe y tener que avanzar a la siguiente fase del boss.
    /// </summary>
    void NextBossState()
    {
        //Debug.Log((int)currentBS--);
        currentBS = (BossStates)((int)currentBS--);
        if (currentBS == BossStates.Wraithed) StartCoroutine(SpawnLasers());
        Debug.Log(currentBS);
        GetNewPatronSeries();
        UseNextPatron();
    }

    /// <summary>
    ///Llama al siguiente patrón. En caso de haber llegado al último vuelve a shufflear
    //entre los patrones de este estado y vuelve a empezar
    /// </summary>
    void UseNextPatron()
    {
        if (_patronIndex == 5 - (int)currentBS)
        {
            GetNewPatronSeries();
        }
        _patronIndex++;
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

        //Debug----------------------------------------------
        string order = "";
        for (int i = 0; i < positionsShuffled.Length; i++)
        {
            order += (positionsShuffled[i] + ", ");
        }
        Debug.Log(order);
        //--------------------------------------------------

        for (int i = 0; i < positionsShuffled.Length; i++)
        {
            _currentBossPatronSeries[i] = _bossPatrons[positionsShuffled[i]];
        }
        _currentBossPatronSeries[positionsShuffled.Length] = _bossPatrons[4];
    }

    #region BossPatronFunctions

    private void EmergingWalls(int i)
    {
        //if (i == 3)
        //{
            StartCoroutine(SpawnPilarsLVL1());
        //}
    }

    private void HandsSweep(int i)
    {
        StartCoroutine(HandSweepLVL1());
    }

    private void HugeHandsSweep(int i)
    {
        //if (i == 3)
        //{
            StartCoroutine(HugeHandSweepLVL1());
        //}
    }

    /*
    private void OpossingNahas(int i)
    {
        if (i == 3)
        {
            StartCoroutine("OpposingNagasLVL1");
        }
    }
    */

    private void BlueImpOne(int i)
    {
        //if (i == 3)
        //{
            StartCoroutine(BlueImpLVL1());
        //}
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
        pinchosSuelo.SetActive(false);
        //restrictingWall.SetActive(false);

        pilarReferences[0] = Instantiate(pilarPrefab, _pilarReferenceTransform.position, Quaternion.identity, _pilarReferenceTransform).GetComponent<DestryAfterTime>();
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
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

        //Patrón se ha terminado 

        UseNextPatron();
    }

    IEnumerator FrozePillar(int i)
    {
        yield return new WaitForSeconds(timeUntilPilarsTouchCeiling);
        pilarReferences[i].SetFrozen(true);
    }
    #endregion

    #region HandSweep
    IEnumerator HandSweepLVL1()
    {
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(true);
        pinchosParedR.SetActive(true);
        pinchosSuelo.SetActive(false);
        //restrictingWall.SetActive(false);
        int rdNumber = (int)Mathf.Sign(UnityEngine.Random.Range(-1, 1));
        stompingHand = Instantiate(stompingHandPrefab, _pilarReferenceTransform.position + stompingHandSpawnOffset.y * Vector3.up + (stompingHandSpawnOffset.x * Vector3.right * rdNumber), Quaternion.identity, _pilarReferenceTransform).GetComponent<StompingHandIA>();
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
        stompingHand.PrevisualizeStomp();
        yield return new WaitForSeconds(stompingPrevisualize);
        stompingHand.BeginToFall(rdNumber * -1);
        yield return new WaitForSeconds(timeTillProjectileDestroy);
        stompingHand.ProjectileDestroy();
        stompingHand.DestroySelf();
        UseNextPatron();
    }
    #endregion

    #region BlueImp
    IEnumerator BlueImpLVL1()
    {
        _bossHitbox.enabled = true;
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);
        pinchosSuelo.SetActive(false);
        //restrictingWall.SetActive(false);
        head = Instantiate(bossHead, _pilarReferenceTransform.position + HeadOffset, Quaternion.identity, _pilarReferenceTransform);
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
        eS = new EnemySpawner[3];
        for(int i = 0; i < eS.Length-1; i++)
        {
            eS[i] = Instantiate(eSpawner, head.transform.position + Math.Abs(head.transform.localScale.x)*Vector3.right + yImpSpawnOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<EnemySpawner>();
            eS[i].Spawn(EnemyType.RegularImp, Vector2.right, impSpeed);
            yield return new WaitForSeconds(timeBetweenImps);
        }
        pinchosSuelo.SetActive(true);

        eS[2] = Instantiate(eSpawner, head.transform.position + Math.Abs(head.transform.localScale.x) * Vector3.right + yImpSpawnOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<EnemySpawner>();
        eS[2].Spawn(EnemyType.BlueImp, Vector2.right, impSpeed);

        yield return new WaitForSeconds(3f);
        pinchosSuelo.SetActive(false);

        yield return new WaitForSeconds(4f);
        for (int i= 0; i < eS.Length; i++)
        {
            eS[i].DestroySpawnedEnemy();
            Destroy(eS[i].gameObject);
        }
        Destroy(head);
        _bossHitbox.enabled = false;
        UseNextPatron();
    }
    #endregion

    #region HugeHandSweep
    IEnumerator HugeHandSweepLVL1()
    {
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(true);
        pinchosParedR.SetActive(true);
        pinchosSuelo.SetActive(false);
        //restrictingWall.SetActive(false);
        int rdNumber = (int)Mathf.Sign(UnityEngine.Random.Range(-1, 1));
        if(_playerTransform.position.x > -181 || _playerTransform.position.x < -360)
        {
            stompingHand = Instantiate(stompingHandPrefab, Vector3.right * Mathf.Sign(_playerTransform.position.x +190) * 88 + _yHugeHandOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<StompingHandIA>();
        }
        else
        {
            stompingHand = Instantiate(stompingHandPrefab, _playerTransform.position.y * Vector3.up + Vector3.right * _playerTransform.position.x + _yHugeHandOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<StompingHandIA>();
        }
        stompingHand.transform.localScale = new Vector3(stompingHand.transform.localScale.x * _stompinhHandSizeMultiplier, stompingHand.transform.localScale.y, stompingHand.transform.localScale.z);
        stompingHand.GetComponent<StompingHandIA>().spawnProjectile = false;
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
        stompingHand.PrevisualizeStomp();
        yield return new WaitForSeconds(stompingPrevisualize);
        stompingHand.BeginToFall(rdNumber * -1);
        int[] toChose = { -160, -380 };
        int[] givenDirection = { -1, 1 };
        int randomChosen = UnityEngine.Random.Range(0, 2);
        _sweepingHand = Instantiate(_sweepingHandPrefab, Vector3.right * toChose[randomChosen] + _ySweepingHandOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform);
        _sweepingHand.GetComponent<EnemyMovement>().Direction(Vector3.right * givenDirection[randomChosen]);
        yield return new WaitForSeconds(timeTillProjectileDestroy/4 + stompingPrevisualize);
        Destroy(_sweepingHand);
        stompingHand.DestroySelf();
        UseNextPatron();
    }
    #endregion

    /*

    #region Jumpers
    IEnumerator JumpersLVL1()
    {
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);
        pinchosSuelo.SetActive(false);
        restrictingWall.SetActive(false);
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
        GameObject jumper = Instantiate(jumperPrefab, )
        UseNextPatron();
    }
    #endregion
    */

    #region OpposingNagas
    IEnumerator OpposingNagasLVL1()
    {
        restrictingWall.SetActive(true);
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);
        pinchosSuelo.SetActive(false);
        EnemySpawner[] eS = new EnemySpawner[4];
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
        float randomNumber = Mathf.Sign(UnityEngine.Random.Range(-1, 1));
        for(int i= 0; i < 3; i++)
        {
            eS[i] = Instantiate(eSpawner, _pilarReferenceTransform.position + randomNumber * Math.Abs(_xNagasSpawnOffset) * Vector3.right + (_yNagasSpawnOffset + i * _yNagasSeparation) * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<EnemySpawner>();
            randomNumber *= -1;
            eS[i].ChangeDirectionLooking(randomNumber);
            eS[i].Spawn(EnemyType.Naha, randomNumber * Vector3.right, 60);
            
            if (i == 2) pinchosSuelo.SetActive(true);

            yield return new WaitForSeconds(_timeBetweenNagas);
        }
        
    }
    #endregion
    
    #region FallingBenes


    IEnumerator FallingBenesLVL1()
    {

        yield return new WaitForSeconds(timeTillPatronStartLVL1);
    }
    
    #endregion
    

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