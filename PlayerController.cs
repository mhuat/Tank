using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    [Header("")]
    [Header("Tank Data")]
    //Booleans
    private bool canFire;

    //Number Values
    public float m_forwardSpeed; //Move Forward Speed in Inspector.
    public float m_backwardSpeed; //Move Backward Speed in Inspector.
    public float m_rotationSpeed; //Rotation Speed in Inspector.

    //Instances
    public TankData tankData;
    
    //Transforms
    private Transform barrel;
    
    //RigidBody
    private Rigidbody rb; //Declare RigidBody
    
    private void Start(){
        rb = GetComponent<Rigidbody>(); //Initialize RigidBody.
        tankData = GetComponent<TankData>(); //Initialize TankData Instance.
        barrel = tankData.tankBarrel;
    }

    private void Update(){
        if (canFire == false) LoadShell();
        if (tankData.health <= 0){
            GameManager.instance.isDead = true;
        }
    }

    private void FixedUpdate(){
        if (Input.GetKey(KeyCode.S)) MoveBackward();
        if (Input.GetKey(KeyCode.W)) MoveForward();
        if (Input.GetKey(KeyCode.A)) RotateLeft();
        if (Input.GetKey(KeyCode.D)) RotateRight();
        if (Input.GetKey(KeyCode.Space)) FireShell();
        if(Input.GetKey(KeyCode.E)&&tankData.invisibilityAvailable) tankData.InvisibleOn();
        if(Input.GetKey(KeyCode.F)&&tankData.cameraMissileAvailable) tankData.LaunchCameraMissile();
    }
    
    public void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Death Zone")){
            GameManager.instance.isDead = true;
            Destroy(GameManager.instance.player);
        }
    }

    private void MoveBackward(){
        float speed = m_backwardSpeed * Time.deltaTime;
        rb.velocity = -transform.forward * speed;
    }

    private void MoveForward(){
        float speed = m_forwardSpeed * Time.deltaTime;
        rb.velocity = transform.forward * speed;
    }

    private void RotateLeft(){
        float speed = m_rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, -1, 0) * speed, Space.World);
    }
		
    private void RotateRight(){
        float speed = m_rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 1, 0) * speed, Space.World);
    }

	//Instantiate projectile.
    private bool FireShell(){
		if (canFire){ //Check to see if boolean is true.
            Instantiate(tankData.smoke, barrel.position, barrel.rotation);
            AudioSource.PlayClipAtPoint(AudioManager.instace.clipList[0], barrel.position, .15f);
            GameObject shellInstance = Instantiate(tankData.shellPrefab, new Vector3(tankData.tankBarrel.transform.position.x, tankData.tankBarrel.transform.position.y, tankData.tankBarrel.transform.position.z), tankData.tankBarrel.transform.rotation);
            shellInstance.tag = "Player Shell"; //Tag BulletInstance
            shellInstance.transform.SetParent(gameObject.transform); //Sets this bullet's parent to this instance's transform.
			canFire = false; //Unable to Fire, Commence LoadingShell()
			//Extensive line 81 not reusable. 
            //shellInstance.GetComponent<Rigidbody>().AddForce (shellInstance.transform.forward * tankData.m_shellForce, ForceMode.Impulse); //BulletInstance RigidBody reference.
        }
        return true;
    }

	//RELOAD
    private void LoadShell(){
        tankData.fireRate -= Time.deltaTime;
        if (tankData.fireRate <= 0f){
            canFire = true;
            tankData.fireRate = tankData.resetValue;
        }
    }
 
}
