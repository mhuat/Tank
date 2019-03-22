using UnityEngine;

public class Shell : MonoBehaviour
{
    public float m_lifeSpan; // Time the shell is alive.
	public float m_shellDamage; // Damage that's added to any source's base damage.

    private GameManager gM;

    void Start()
    {
        gM = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        m_lifeSpan -= Time.deltaTime;
        if (m_lifeSpan < 0f)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Health>() == null&& this.tag =="Enemy Shell")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.GetComponent<Health>() == null && this.tag == "Player Shell")
        {
            Destroy(this.gameObject);
        }
        if (other.gameObject.GetComponent<Health>() != null && this.tag == "Player Shell" && other.gameObject != GameManager.instance.player && this.gameObject.GetComponentInParent<PlayerController>() == null)
        {
            other.gameObject.GetComponent<Health>().m_health -= gM.player.GetComponent<TankData>().m_tankDamage + m_shellDamage;
            if (other.gameObject.GetComponent<TankData>() != null)
            {
                GameManager.instance.player.GetComponent<TankData>().m_score += other.gameObject.GetComponent<TankData>().m_pointValue;
            }
            else { Destroy(this.gameObject); }
            Destroy(this.gameObject);
        }
        else if (other.gameObject.GetComponent<Health>() != null && this.tag == "Enemy Shell" && other.gameObject.GetComponent<AIController>() == null) //&& other.gameObject!=gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().m_health -= GameManager.instance.enemy.GetComponent<TankData>().m_tankDamage + m_shellDamage;
            Destroy(gameObject);
        }
        else { return; }
        //Destroy(gameObject);
    }

}

//Notes:
//GameManager.instance.enemyHealth -= GameManager.instance.playerDamage;
//enemyRef.GetComponent<Health>().m_health -= playerRef.GetComponent<PlayerController>().m_shellDamage;
//GameManager.instance.aC.health -= GameManager.instance.pC.m_Damage;
/*
    if (other.gameObject.CompareTag("Player") && this.tag == "Enemy Shell")
    {
        GameManager.instance.enemy.GetComponent<Health>().m_health = GameManager.instance.enemy.GetComponent<Health>().m_health -
        GameManager.instance.enemy.GetComponent<AIController>().m_shellDamage;
        Destroy(gameObject);
    }
    if (other.gameObject.CompareTag("Enemy") && this.tag == "Player Shell")
    {
        GameManager.instance.player.GetComponent<Health>().m_health -=
            GameManager.instance.player.GetComponent<PlayerController>().m_shellDamage;
        Destroy(gameObject);
    }
    if (other.gameObject.CompareTag("Enemy") && this.tag == "Player Shell")
    {
        //other.gameObject.GetComponent<Health>().m_health-=GameManager.instance.pC.m_damage;
        Destroy(gameObject);
    }
    if (other.gameObject.CompareTag("Player") && this.tag == "Enemy Shell")
    {
        //GameManager.instance.player.GetComponent<Health>().m_health -=
        //GameManager.instance.player.GetComponent<PlayerController>().m_shellDamage;
        Destroy(gameObject);
    }
*/
