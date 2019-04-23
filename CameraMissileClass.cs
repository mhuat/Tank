using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMissileClass : MonoBehaviour
{

    [SerializeField]//private Camera miniCam;
    private float force = 5f;
    public GameObject explosion;

    void Start()
    {
        //miniCam = GameObject.FindGameObjectWithTag("MiniCam").GetComponent<Camera>();
        AudioSource.PlayClipAtPoint(AudioManager.instace.clipList[4], transform.position, .8f);
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);
        if (SceneHandler.multiplayer)
        {
            GetComponentInChildren<Camera>().enabled = false;
        }
    }

    private void Update()
    {
        if (!SceneHandler.multiplayer)
        {
            Time.timeScale = 0.6f;
            float dist = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
            if (dist > 200f)
            {
                Explode();
            }
        }
        
        if (Input.GetKeyUp(KeyCode.V)||Input.GetKey(KeyCode.Keypad5))
        {
            Explode();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponentInParent<TankData>())
        {
            other.gameObject.GetComponentInParent<TankData>().health -= 100;
        }
        Explode();
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(AudioManager.instace.clipList[1], transform.position, .03f);
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
