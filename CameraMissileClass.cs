using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMissileClass : MonoBehaviour
{
    [SerializeField]
    private Camera miniCam;
    private float force = 5f;

    void Start()
    {

        miniCam = GameObject.FindGameObjectWithTag("MiniCam").GetComponent<Camera>();
        GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Impulse);

    }

    private void Update()
    {
        Time.timeScale = 0.7f;
        float dist = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);
        if (dist > 200f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponentInParent<TankData>())
        {
            other.gameObject.GetComponentInParent<TankData>().health -= 100;
        }

        Destroy(gameObject);
    }
}
