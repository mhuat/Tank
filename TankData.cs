using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankData : MonoBehaviour
{
    [Header("Editable Variable Values:")]
    [Header("")]
    public float fireRate;
    public float health;
    private float maxHealth;
    public float resetValue;
    public float shellForce;
    public float tankDamage;
    private float timer=4.0f;
    public Transform tankBarrel;
    public Transform airStrikeArea;
    readonly List<Renderer> rList = new List<Renderer>();
    public GameObject shellPrefab;
    public GameObject smoke;
    public GameObject explosion;
    private ParticleSystem explosionParticle;
    public GameObject airStrike;
    public GameObject cameraMissile;
    public bool airStrikeAvailable;
    public bool cameraMissileAvailable;
    public bool invisibilityAvailable;
    public bool inv;
    private AIController enemyREf;
    public PlayerController playerController;

    private void Start()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
        maxHealth = health;
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            rList.Add(r);
        }

        if (gameObject.GetComponentInParent<AIController>())
        {
            enemyREf = gameObject.GetComponentInParent<AIController>();
        }

        if (gameObject.GetComponentInParent<PlayerController>())
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        explosionParticle = explosion.GetComponentInChildren<ParticleSystem>();
    }
    
    private void Update()
    {
        if (health >= maxHealth)
        {
            health = maxHealth;
        }

        if (health <= 0)
        {
            AudioSource.PlayClipAtPoint(AudioManager.instace.clipList[3], transform.position, 1f);
            Instantiate(explosion, transform.position, transform.rotation);
            explosionParticle.Play();
            Destroy(gameObject);
            if (enemyREf)
            {
                GameManager.instance.numOfEnemies -= 1;
            }
        }

        if (inv){
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 4.0f;
                InvisibleOff();
                inv = false;
                invisibilityAvailable = false;
            }
        }

    }
    
    
    public void LaunchAirStrike()
    {
        if (GetComponentInParent<PlayerController>())
        {
            Instantiate(airStrike, airStrikeArea);
        }else Instantiate(airStrike, airStrikeArea);
    }

    public void LaunchCameraMissile()
    {
        if (GetComponentInParent<PlayerController>())
        {
            Instantiate(cameraMissile,
                new Vector3(tankBarrel.transform.position.x, tankBarrel.transform.position.y,
                    tankBarrel.transform.position.z), tankBarrel.transform.rotation);//tankBarrel.transform.rotation);
            cameraMissileAvailable = false;
        }
    }

    public void InvisibleOn()
    {
        foreach (var r in rList)
        {
            r.enabled = false;
            inv = true;
            GameManager.instance.player = null;
        }
    }
    void InvisibleOff()
    {
        foreach (Renderer r in rList)
        {
            r.enabled = true;
            inv = false;
            GameManager.instance.player = gameObject;
        }
    }
}