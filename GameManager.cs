using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.UIElements;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    public static GameManager instance;

    //Booleans
    public bool gameOver;
    public bool win;
    public bool paused = false;
    [HideInInspector] public bool isDead;
    [HideInInspector] public bool spawned;
    [HideInInspector] public bool spawnedEnemy;

    //GameObjects
    [Tooltip("Player Tank Prefab")]
    public GameObject tankPrefab;
    [Header("Player Instance")]
    [SerializeField]
    public GameObject player;

    //Lists
    //public List<GameObject> L_Enemy = new List<GameObject>();
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    public List<Transform> playerSpawns = new List<Transform>();
    public List<Transform> enemySpawns= new List<Transform>();

    [Header("Game Settings")]
    //Number Values
    public float startCountDown;
    public int stocks;
    public int score;
    public int numOfEnemies;
     
    private void Awake(){
        if (instance == null) {
            instance = this;
        }else{ Destroy(gameObject); }
        gameOver = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start(){
        //player = null;
        //tile = GameObject.Find("TT1(Clone)");
        isDead = true;
        spawnedEnemy = false;
        foreach (var tf in GameObject.FindGameObjectsWithTag("PS"))
        {
            playerSpawns.Add(tf.transform);
        }
        foreach (var tf in GameObject.FindGameObjectsWithTag("ES"))
        {
            enemySpawns.Add(tf.transform);
        }
    }

    private void Update(){
        //Debug.Log(L_Enemy.Count);
        //To ensure only one instance of the player exists in the scene. Will be applied as re-spawn as well.
        if (stocks <= 0)
        {
            gameOver = true;
            if(gameOver)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                SceneManager.LoadScene(0);
            }
        }

        if (numOfEnemies<=0&&spawned)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(2);
        }

        if (isDead&&spawned)
        {
            player = null;
            stocks -= 1;
            spawned = false;
        }
        if (!player&& Input.GetKeyDown(KeyCode.R)&&!spawned)
        {
            SpawnPlayer();
            if (!spawnedEnemy)
            {
                SpawnEnemyTank();
            }
            UIManager.UiManager.DisableText();;
        }

        if (Input.GetKeyDown(KeyCode.P)){
            paused = !paused;
        }
        if (paused){
            Time.timeScale = 0;
        }else if (!paused){
            Time.timeScale = 1;
        }

    }

    void SpawnPlayer()
    {
        int rand1 = Random.Range(0, playerSpawns.Count);
        GameObject playerInstance = Instantiate(tankPrefab, playerSpawns[rand1].position, playerSpawns[rand1].rotation);
        player = playerInstance;
        player.tag = "Player";
        isDead = false;
        spawned = true;
        //player = GameObject.Find("T95-2(Clone)");
        //pC = player.GetComponent<PlayerController>();
    }

    void SpawnEnemyTank()
    {
        foreach (GameObject enemy in enemyPrefabs)
        {
            GameObject enemyInstance = 
            Instantiate(enemy, enemySpawns[Random.Range(0, enemySpawns.Count)]);         
            //L_Enemy.Add(enemyInstance);
            numOfEnemies += 1;
        }
        spawnedEnemy = true;
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

