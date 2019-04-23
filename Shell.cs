using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Shell : MonoBehaviour
{
    public GameObject smoke;

    public TankData tkRef;

    void Start()
    {
        tkRef = GetComponentInParent<TankData>();
        GetComponent<Rigidbody>().AddForce (transform.forward * tkRef.shellForce, ForceMode.Impulse);
    }

    public void OnCollisionEnter(Collision other)
    {
        AudioSource.PlayClipAtPoint(AudioManager.instace.clipList[1], transform.position, .03f);
        Destroy(gameObject);
        Instantiate(smoke, transform.position, Quaternion.identity);
        if (other.gameObject.GetComponent<TankData>()!= null){
            if (SceneHandler.multiplayer&&!SceneHandler.FriendlyFire)
            {
                if (other.gameObject.GetComponent<PlayerController>()&&GetComponentInParent<PlayerController>())
                {
                    other.gameObject.GetComponent<TankData>().health+=0;
                }
            }
            else
            {
                other.gameObject.GetComponent<TankData>().health-=GetComponentInParent<TankData>().tankDamage;
            }
           if (other.gameObject.GetComponentInParent<AIController>())
           {
               if (!SceneHandler.multiplayer)
               {
                   GameManager.instance.score += other.gameObject.GetComponentInParent<AIController>().pointValue;
               }
               else
               {
                   if (tkRef.gameObject.CompareTag("Player"))
                   {
                       GameManager.instance.score += other.gameObject.GetComponentInParent<AIController>().pointValue;
                       other.gameObject.GetComponent<TankData>().health -= GetComponentInParent<TankData>().tankDamage;
                   }
                   if (tkRef.gameObject.CompareTag("PlayerTwo"))
                   {
                       GameManager.instance.scoreTwo += other.gameObject.GetComponentInParent<AIController>().pointValue;
                       other.gameObject.GetComponent<TankData>().health -= GetComponentInParent<TankData>().tankDamage;
                   }
               }
           }
        }
    }

}

//Notes:
/*
        //float tdh = other.gameObject.GetComponent<TankData>().m_health;
        //float damage = GetComponentInParent<TankData>().m_tankDamage;
       if (other.gameObject.GetComponent<Health>() == null)
       {
           Destroy(this.gameObject);
       }
       if (other.gameObject.GetComponent<Health>() != null && this.tag == "Player Shell" && other.gameObject != GameManager.instance.player)
       {
           Debug.Log(other.gameObject.name);
           other.gameObject.GetComponent<Health>().m_health-= gM.player.GetComponent<TankData>().m_tankDamage + m_shellDamage;
           if (other.gameObject.GetComponent<TankData>() != null)
           {
               GameManager.instance.player.GetComponent<TankData>().m_score += other.gameObject.GetComponent<TankData>().m_pointValue;
           }else { Destroy(gameObject); }
       }
       else if (other.gameObject.GetComponent<Health>() != null && this.tag == "Enemy Shell" && other.gameObject.GetComponent<AIController>() == null) //&& other.gameObject!=gameObject.CompareTag("Enemy"))
       {
           other.gameObject.GetComponent<Health>().m_health -= GameManager.instance.enemy.GetComponent<TankData>().m_tankDamage + m_shellDamage;
           Destroy(gameObject);
       }*/


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
