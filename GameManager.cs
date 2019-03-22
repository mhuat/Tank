using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    //Booleans
    //private bool m_startGame; // use
    //private bool m_waveOne; // use
    //private bool m_hasSpawned; //Checks to see if the Player has spawned previously.

    //GameObjects
    public GameObject tankPrefab;
    public GameObject enemyTankPrefab;
    //[HideInInspector]
    public GameObject player;
    public GameObject enemy;

    //Instances
    public PlayerController pC;
    public AIController aC;

    //Lists
    public List<GameObject> L_Enemy = new List<GameObject>();

    [Header("Game Settings")]
    //Number Values
    [Tooltip("Indication of when Game Starts")]
    public float m_startCountDown; //Start Game timer.
    [Tooltip("Set Time between Enemy Tank Spawns")]
    public float m_enemyTankSpawnTimer; //Time between Enemy Spawns.
    [Tooltip("Set Number of Enemy Tanks to Spawn")]
    public int EnemyTanksToSpawn; //Number of Enemy Tanks to spawn.
    private float timer; //Count down timer container.
    private int EnemyTanksSpawned; //Number of Enemy Tanks spawned.
    //public int numOfEnemyTanksKilled; Total number of "Enemy Tanks" destroyed.

    //Static Variables
    public static GameManager instance;

    //Transforms
    [HideInInspector] public Transform playerSpawnLocation;
    [HideInInspector] public Transform enemySpawnLocation;
    [HideInInspector] public Transform centerPosition;
    [HideInInspector] public bool m_isDead;

    /// <summary>
    /// OnAwake(), Set Static GameObject as GameManager Instance.
    /// </summary>
    private void Awake(){
        if (instance == null) {
            instance = this;
        }else{ Destroy(gameObject); }
    }

    /// <summary>
    /// Initialize Spawn Location.
    /// </summary>
    private void Start(){
        player = null;
        m_isDead = true;
        //m_startGame = false;
        //m_hasSpawned = false;
        playerSpawnLocation = GameObject.Find("Player Spawn Location").transform;
        enemySpawnLocation = GameObject.Find("Enemy Spawn Location").transform;
        centerPosition = GameObject.Find("Center Position").transform;
    }

    /// <summary>
    /// If the player is dead, press any key to spawn a new player.
    /// </summary>
    private void Update(){
        //Debug.Log(L_Enemy.Count);
        //To ensure only one instance of the player exists in the scene. Will be applied as re-spawn as well.

        if (enemy == null)
        {
            enemy = GameObject.FindGameObjectWithTag("Enemy");
        }
        if (m_isDead)
        {
            player = null;
            pC = null;
        }
        if (player == null && Input.GetKey(KeyCode.R))
        {
            SpawnPlayer();
            //m_startGame = true;
            UIManager.UiManager.DisableText();
            player = GameObject.Find("Tank(Clone)");
        }
    }

    /// <summary>
    /// Initialize player as an instance of the "tankPrefab".
    /// </summary>
    void SpawnPlayer()
    {
        GameObject playerInstance = Instantiate(tankPrefab, playerSpawnLocation.position, playerSpawnLocation.rotation);
        player = playerInstance;
        player.tag = "Player";
        m_isDead = false;
        pC = player.GetComponent<PlayerController>();
    }

    void SpawnEnemyTank()
    {     
        GameObject enemyInstance = Instantiate(enemyTankPrefab, enemySpawnLocation.position, enemySpawnLocation.rotation) as GameObject;
        aC = enemyInstance.GetComponent<AIController>();
        L_Enemy.Add(enemyInstance);
        EnemyTanksSpawned += 1;
    }
    void WaveOfEnemies()
    {
        if (EnemyTanksSpawned < EnemyTanksToSpawn)
        {
            timer += Time.deltaTime;
            if (timer >= m_enemyTankSpawnTimer)
            {
                SpawnEnemyTank();
                timer = 0;
            }
        }
        else if (EnemyTanksSpawned == EnemyTanksToSpawn)
        {
            //m_waveOne = false;
        }
    }

    //End of File
}

/*
Notes:

/// <summary>
/// Sets the position and rotation to desired Starting position-
/// -of camera in the scene before being parented to a new player instance.
/// </summary>
public void ResetCamera()
{
    //Camera.main.transform.SetPositionAndRotation(new Vector3(0f, 13f, -30f), Quaternion.Euler(15f, 0f, 0f));
    //Camera.main.transform.parent = centerPosition.transform;
}

Update Function with Wave implementation:
void Update()
{
    if (Input.anyKey && player == null)
    {
        SpawnPlayer();
        m_startGame = true;
        UIManager.UiManager.DisableText();
    }
    if (player != null && m_startGame &&m_hasSpawned==false)
    {
        UIManager.UiManager.StartCountDown();
        m_startCountDown -= Time.deltaTime;
        {
            if (m_startCountDown <= 0f)
            {
                UIManager.UiManager.DisableText();
                m_startCountDown = 5f;
                m_startGame = false;
                m_waveOne = true;
                m_hasSpawned = true;
            }
        }
    }
    if (m_waveOne)
    {
        WaveOfEnemies();
    }
    if (player == null)
    {
        m_isDead = true;
        player = null;
        pC = null;
    }
    if (enemy == null)
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }
}
*/

