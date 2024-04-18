using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

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
    private Action[] _bossPatrons;
    /// <summary>
    /// Array con los patrones del boss en el orden en el que los va a ejecutar.
    /// </summary>
    private Action[] _currentBossPatronSeries;
    /// <summary>
    /// Valor del patron por el que se llega el boos, referido a la posicion de la array de arriba
    /// </summary>
    private int _patronIndex = 0;
    [SerializeField]
    private Transform _bossTPInitPos;

    [SerializeField]
    private float HeadDamagedOffset = -30;
    [SerializeField]
    private float timeTillPatronStartLVL1 = 2f;

    [SerializeField]
    private BoxCollider2D _bossHitbox;
    [SerializeField]
    private float _bossDamagedTime = 2f;

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
    float timeTillSpikesGone = 5f;
    [SerializeField]
    float _nahaSpeed = 10f;
    [SerializeField]
    private float _nahaOffset = -20f;
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
    GameObject _bossLancePreviewPrefab;
    [SerializeField]
    Transform _bossImagePosition;
    [SerializeField]
    Transform _limitL;
    [SerializeField]
    Transform _limitR;
    GameObject _laserPrefab;
    [SerializeField]
    float _laserSize = 6f;
    GameObject bossImg;
    GameObject _spikeWarning;
    GameObject _spikeWarningInstance;
    GameObject[] lasers = new GameObject[0];
    BossHealth _bossHealth;

    GameObject _bossTPIN;

    bool IdleSpawned = false;
    GameObject idleBossSpawnedGO;
    GameObject startingAnim;

    /*
    [Header("Jumpers")]
    private GameObject jumperPrefab;

    */
    /*
    IEnumerator SpawnLasers()
    {
        while (BossStates.Wraithed == currentBS)
        {
            Debug.Log("Lasers");
            Instantiate(_laserPrefab, new Vector3(UnityEngine.Random.Range(_limitL.position.x, _limitR.position.x), _limitL.position.y, _limitL.position.z), Quaternion.identity);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.6f, 2f));
        }
    }
    */

    public void BossStartAnim()
    {
        startingAnim = Instantiate(_bossTPIN,_bossTPInitPos.position, Quaternion.identity);
        StartCoroutine(WaitAndStart());
    }
    IEnumerator WaitAndStart()
    {
        yield return new WaitForSeconds(3);
        Instantiate(_bossTPIN, _bossTPInitPos.position, Quaternion.identity);
        yield return new WaitForSeconds(1.5f);
        GetNewPatronSeries();
        UseNextPatron();
    }

    public bool CheckSpawnBossIdle(ref GameObject spawnedBoss)
    {
        if (!IdleSpawned) idleBossSpawnedGO = spawnedBoss;
        else
        {
            Destroy(spawnedBoss);
            spawnedBoss = idleBossSpawnedGO;
        }
        IdleSpawned = !IdleSpawned;
        return !IdleSpawned;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<ProyectileMovement>() != null)
        {
            ReceiveDamage();
        }
    }

    /// <summary>
    /// Para todos los procesos y espera a la se�al del animator para continuar
    /// </summary>
    public void PlayerDied()
    {

        KillEverythingOnScreen();
        //-------------------------------------------------------provisional
        StartCoroutine(Restart());
        //--------------------------------------------------------------
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        _patronIndex = 0;
        GetNewPatronSeries();
        UseNextPatron();
        //if (currentBS == BossStates.Wraithed) StartCoroutine(SpawnLasers());
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
            pilarReferences[i]?.startDestroyingitForReal();
        }
        pilarReferences = new DestryAfterTime[27];
        stompingHand?.ProjectileDestroy();
        stompingHand?.DestroySelf();
        Destroy(_sweepingHand);
        stompingHand = null;

        if(bossImg!=null)
            Destroy(bossImg);
        bossImg = null;

        for (int i = 0; i < eS.Length; i++)
        {
            try { 
                eS[i]?.DestroySpawnedEnemy();
                Destroy(eS[i]?.gameObject);
            }
            catch
            { 
            }
        }

        for(int i = 0; i < lasers.Length; i++)
        {
            Destroy(lasers[i]?.gameObject);
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
    /// Este metodo deber�a reiniciar el estado y hacer la animaci�n de me has dado
    /// Esto o no interrumpir las corroutines, solo hacer que parpadee x segundos para denotarlo sin romper flujo de nada
    /// </summary>
    private void ReceiveDamage()
    {
        StopAllCoroutines();
        _bossHitbox.enabled = false;
        KillEverythingOnScreen();
        _bossHealth.ChangeHealth(((int)currentBS)-1);
        GameObject toDestroy = Instantiate(bossHead, _pilarReferenceTransform.position + HeadOffset + Vector3.up * HeadDamagedOffset, Quaternion.identity);
        toDestroy.GetComponentInChildren<Animator>().SetBool("DamageReceived", true);
        StartCoroutine(NextBossStateCountdown(toDestroy));
    }

    private IEnumerator NextBossStateCountdown(GameObject toDestroy)
    {
        yield return new WaitForSeconds(_bossDamagedTime);
        Destroy(toDestroy);
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
        _bossLancePreviewPrefab = Resources.Load<GameObject>("BossLancePreviewImage");
        _playerTransform = FindObjectOfType<RefactoredCharacterController>().transform;
        _bossTPIN = Resources.Load<GameObject>("TPboosIN");
        _spikeWarning = Resources.Load<GameObject>("SpikeWarning");
        _bossHealth = FindObjectOfType<BossHealth>();
        //jumperPrefab = Resources.Load<GameObject>("JumpPad");

        //https://stackoverflow.com/questions/7712137/array-containing-methods
        _bossPatrons = new Action[5];
        _bossPatrons[0] = HandsSweep;//HugeHandsSweep;   //Desbloqueado desde el principio
        _bossPatrons[1] = StartLasersPatron;       //Desbloqueado desde el principio
        _bossPatrons[2] = EmergingWalls;     //Desbloqueado tras golpear 1 vez al boss
        _bossPatrons[3] = OpossingNahas;    //Desbloqueado tras golpear 2 veces al boss
        _bossPatrons[4] = BlueImpOne;       //Patr�n que le permite recibir da�o y que siempre
                                            //se ejecutar� al final de la serie de patrones generados.

        //StartCoroutine("OpposingNagasLVL1");
        //_bossPatrons[3]();
        //StartCoroutine(HugeHandSweepLVL1());
        //_bossPatrons[4](3);

        
        //------------------------------------------------------------------------------------------------------------------------------------------------------
        //if (currentBS == BossStates.Wraithed) StartCoroutine(SpawnLasers());
        //GetNewPatronSeries();
        //UseNextPatron();
        //_bossPatrons[0]((int)currentBS);
    }

    /// <summary>
    /// Esto se llama al recibir un golpe y tener que avanzar a la siguiente fase del boss.
    /// </summary>
    void NextBossState()
    {

        //Debug.Log((int)currentBS + " " + (((int)currentBS)-1));
        currentBS = (BossStates)(((int)currentBS) - 1);
        
       
        Debug.Log(currentBS);
        /*if (currentBS == BossStates.Wraithed) StartCoroutine(SpawnLasers());
        else*/
        if (currentBS == BossStates.Dead)
        {
            StartCoroutine(PlayerWins());
            return;
        }
        GetNewPatronSeries();
        UseNextPatron();
    }

    IEnumerator PlayerWins()
    {
        try
        {
            AudioManager.Instance.StopMusic(FMODEvents.Instance.BossMusic);
        }
        catch
        {
            Debug.Log("Falta el audio :(");
        }
        GameObject obj = Instantiate(Resources.Load<GameObject>("BossExplosion"), _pilarReferenceTransform.position + HeadOffset, Quaternion.identity, _pilarReferenceTransform);
        yield return new WaitForSeconds(1);
        Destroy(obj);
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadSceneAsync(0);

        yield return new WaitForSeconds(0.05f);
        FindObjectOfType<Canvas>().GetComponent<ScriptMenu>().Credits();
        Destroy(this.gameObject);
    }


    /// <summary>
    ///Llama al siguiente patr�n. En caso de haber llegado al �ltimo vuelve a shufflear
    //entre los patrones de este estado y vuelve a empezar
    /// </summary>
    void UseNextPatron()
    {
        if (_patronIndex == 5 - (int)currentBS)
        {
            GetNewPatronSeries();
        }
        _patronIndex++;
        _currentBossPatronSeries[_patronIndex]();
    }

    /// <summary>
    /// Genera una nueva serie con los patrones que tiene desbloqueados el Boss en la fase actual
    /// </summary>
    void GetNewPatronSeries()
    {
        _patronIndex = -1;
        int[] positionsShuffled = new int[5 - (int)currentBS];
        _currentBossPatronSeries = new Action[6 - (int)currentBS];
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
        //Debug.Log(order);
        //--------------------------------------------------

        for (int i = 0; i < positionsShuffled.Length; i++)
        {
            _currentBossPatronSeries[i] = _bossPatrons[positionsShuffled[i]];
        }
        _currentBossPatronSeries[positionsShuffled.Length] = _bossPatrons[4];
    }

    #region BossPatronFunctions

    private void EmergingWalls()
    {
        //if (i == 3)
        //{
            StartCoroutine(SpawnPilarsLVL1());
        //}
    }

    private void HandsSweep()
    {
        StartCoroutine(HandSweepLVL1());
    }

    private void HugeHandsSweep()
    {
        //if (i == 3)
        //{
            StartCoroutine(HugeHandSweepLVL1());
        //}
    }

    private void StartLasersPatron()
    {
        StartCoroutine(LasersPatron());
    }

    
    private void OpossingNahas()
    {
        StartCoroutine("NahaSimplePattern");
    }
    
    private void BlueImpOne()
    {
        //if (i == 3)
        //{
            StartCoroutine(BlueImpLVL1());
        //}
    }

    #region auxiliarBossPatronFunctions

    IEnumerator LasersPatron()
    {
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(true);
        pinchosParedR.SetActive(true);
        pinchosSuelo.SetActive(false);

        bossImg = Instantiate(_bossLancePreviewPrefab, _bossImagePosition.position, Quaternion.identity);
        int l = 1;
        int r = (int)Math.Abs((_limitL.position.x - _limitR.position.x) / (_laserSize));
        int[] n = new int[] { ((int)Math.Abs((_limitL.position.x - _limitR.position.x) / (_laserSize))) / 2, 1, (int)Math.Abs((_limitL.position.x - _limitR.position.x) / (_laserSize)) - 1 }; 
        for(int m = 0; m < 3; m++)
        {
            lasers = new GameObject[r+2];
            r = 0;
            //int n = UnityEngine.Random.Range(1, (int)Math.Abs((_limitL.position.x - _limitR.position.x) / (_laserSize*(l + 1))));
            for (int i = 0; i < n[m] - 3; i++)
            {
                lasers[r] = Instantiate(_laserPrefab, new Vector3(_limitL.position.x + i * _laserSize, _limitL.position.y, _limitL.position.z), Quaternion.identity);
                r++;
            }
            for (int i = n[m] + 3 ; i < (int)Math.Abs((_limitL.position.x - _limitR.position.x) / _laserSize); i++)
            {
                lasers[r] = Instantiate(_laserPrefab, new Vector3(_limitL.position.x + i * _laserSize, _limitL.position.y, _limitL.position.z), Quaternion.identity);
                r++;
            }

            yield return new WaitForSeconds(2);
        }
        Destroy(bossImg);
        bossImg = null;
        yield return new WaitForSeconds(0.5f);
        UseNextPatron();
    }

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
        pilarReferences[0].startDestroyingitForReal();
        StartCoroutine("FrozePillar", 0);
        int i = 1;
        while (i != pilarReferences.Length)
        {

            yield return new WaitForSeconds(delayTimeBetweenPilars);
            pilarReferences[i]   = Instantiate(pilarPrefab, _pilarReferenceTransform.position + (i * 4 * Vector3.right), Quaternion.identity, _pilarReferenceTransform).GetComponent<DestryAfterTime>();
            pilarReferences[i].startDestroyingitForReal();
            StartCoroutine("FrozePillar",i);
            pilarReferences[i+1] = Instantiate(pilarPrefab, _pilarReferenceTransform.position + (i * 4 * Vector3.left), Quaternion.identity, _pilarReferenceTransform).GetComponent<DestryAfterTime>();
            pilarReferences[i+1].startDestroyingitForReal();
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
        pilarReferences[0].startDestroyingitForReal();
        i = 1;
        while (i != pilarReferences.Length)
        {

            yield return new WaitForSeconds(delayTimeBetweenPilars);
            pilarReferences[i].NegateBeginToFallInstead();
            pilarReferences[i].startDestroyingitForReal();
            pilarReferences[i + 1].NegateBeginToFallInstead();
            pilarReferences[i + 1].startDestroyingitForReal();
            i += 2;
        }

        yield return new WaitForSeconds(timeTillPatronStartLVL1);

        //Patr�n se ha terminado 

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
        if(rdNumber == 1)
        {
            stompingHand.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
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
        eS = new EnemySpawner[3];
        head = Instantiate(bossHead, _pilarReferenceTransform.position + HeadOffset, Quaternion.identity, _pilarReferenceTransform);
        yield return new WaitForSeconds(timeTillPatronStartLVL1);
        _spikeWarningInstance = Instantiate(_spikeWarning, new Vector3(-272, -65, -3.02301717f), Quaternion.identity);
        for(int i = 0; i < eS.Length-1; i++)
        {
            eS[i] = Instantiate(eSpawner, head.transform.position + Math.Abs(head.transform.localScale.x)*(2F / 3) * Vector3.right + yImpSpawnOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<EnemySpawner>();
            eS[i].Spawn(EnemyType.RegularImp, Vector2.right, impSpeed);
            yield return new WaitForSeconds(timeBetweenImps);
        }
        Destroy(_spikeWarningInstance);
        pinchosSuelo.SetActive(true);

        eS[2] = Instantiate(eSpawner, head.transform.position + Math.Abs(head.transform.localScale.x)*(2F/3) * Vector3.right + yImpSpawnOffset * Vector3.up, Quaternion.identity, _pilarReferenceTransform).GetComponent<EnemySpawner>();
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

    IEnumerator NahaSimplePattern()
    {
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);
        pinchosSuelo.SetActive(false);
        eS = new EnemySpawner[1];
        head = Instantiate(bossHead, _pilarReferenceTransform.position + HeadOffset, Quaternion.identity, _pilarReferenceTransform);
        eS[0] = Instantiate(eSpawner, _bossImagePosition.position + Vector3.right * _nahaOffset, Quaternion.identity).GetComponent<EnemySpawner>();
        eS[0].ChangeDirectionLooking(-1);
        eS[0].Spawn(EnemyType.Naha, Vector2.right, _nahaSpeed, 8);
        _spikeWarningInstance = Instantiate(_spikeWarning, new Vector3(-272, -65, -3.02301717f), Quaternion.identity);
        yield return new WaitForSeconds(2*timeTillPatronStartLVL1);
        Destroy(_spikeWarningInstance);
        pinchosSuelo.SetActive(true);
        yield return new WaitForSeconds(timeTillSpikesGone);
        pinchosTecho.SetActive(false);
        pinchosParedL.SetActive(false);
        pinchosParedR.SetActive(false);
        pinchosSuelo.SetActive(false);
        eS[0].DestroySpawnedEnemy();
        Destroy(eS[0].gameObject);



        Destroy(head);
        yield return new WaitForSeconds(timeTillPatronStartLVL1/2);
        UseNextPatron();
    }

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