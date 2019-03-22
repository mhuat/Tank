using JetBrains.Annotations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    [Header("")]
    [Header("Tank Data")]
    //Booleans
    private bool canFire;
    [HideInInspector]
    public bool falling;
    //[HideInInspector]
    public bool moving;

    //Number Values
    public float m_forwardSpeed; //Move Forward Speed in Inspector.
    public float m_backwardSpeed; //Move Backward Speed in Inspector.
    public float m_rotationSpeed; //Rotation Speed in Inspector.

    //GameObjects

    //Instances
    public TankData tankData;

    //Static Variables

    //RigidBody
    private Rigidbody m_rb; //Declare RigidBody

    private void Start(){
        m_rb = GetComponent<Rigidbody>(); //Initialize RigidBody.
        tankData = GetComponent<TankData>(); //Initialize TankData Instance.
    }

    private void Update(){
        if (canFire == false) LoadShell();
        if (gameObject.GetComponent<Health>().m_health <= 0){
            GameManager.instance.m_isDead = true;
            Destroy(gameObject);
        }
        //Debug.Log("Player Score: " + tankData.m_score);
    }

    private void FixedUpdate(){
        if (Input.anyKey) Control(); else moving = false; //Check if there's any Player Input before executing methods.
        if (!moving) { }
        if (falling){
            m_rb.velocity -= Vector3.down * Time.deltaTime; //Decrease velocity by desired variable.
            m_rb.mass += Time.deltaTime; //Increase mass by desired variable.
        } //Exception case.
    }

    private void Control(){
        if (Input.GetKey(KeyCode.S)) MoveBackward();
        if (Input.GetKey(KeyCode.W)) MoveForward();
        if (Input.GetKey(KeyCode.A)) RotateLeft();
        if (Input.GetKey(KeyCode.D)) RotateRight();
        if (Input.GetKey(KeyCode.Space)) FireShell();
    }

    private void MoveBackward(){
        moving = true;
        float speed = m_backwardSpeed * Time.deltaTime;
        m_rb.velocity = -transform.forward * speed;
    }

    private void MoveForward(){
        moving = true;
        float speed = m_forwardSpeed * Time.deltaTime;
        m_rb.velocity = transform.forward * speed;
    }

    private void RotateLeft(){
        moving = true;
        float speed = m_rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, -1, 0) * speed, Space.World);
    }
		
    private void RotateRight(){
        moving = true;
        float speed = m_rotationSpeed * Time.deltaTime;
        transform.Rotate(new Vector3(0, 1, 0) * speed, Space.World);
    }

	//Instantiate projectile.
    private void FireShell(){
		if (canFire){ //Chceck to see if boolean is true.
            GameObject shellInstance = Instantiate(tankData.shellPrefab, new Vector3(tankData.tankBarrel.transform.position.x, tankData.tankBarrel.transform.position.y, tankData.tankBarrel.transform.position.z), tankData.tankBarrel.transform.rotation); //Instantiate Shellprefab at the player's barrel transform position/rotation. 
			shellInstance.GetComponent<Rigidbody> ().AddForce (shellInstance.transform.forward * tankData.m_shellForce * Time.deltaTime, ForceMode.Impulse); //BulletInstance RigidBody reference.
            shellInstance.tag = "Player Shell"; //Tag BulletInstance
			canFire = false; //Unable to Fire, Commence LoadingShell()
        }
    }

	//RELOAD
    private void LoadShell(){
        tankData.m_fireRate -= Time.deltaTime;
        if (tankData.m_fireRate <= 0f){
            canFire = true;
            tankData.m_fireRate = tankData.m_resetValue;
        }
    }

    /// <summary>
    /// On Collision with other objects, trigger corresponding event.
    /// </summary>
    /// <param name="other"></param>
    public void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Death Zone")){
            GameManager.instance.m_isDead = true;
            Destroy(GameManager.instance.player);
        }
    }

	/// <summary>
	/// Casts a ray from thje player to the ground at an infinite distance.
	/// If a death zone is hit with the raycast, Do: ("falling = true")
	/// </summary>
    void RayCastToGround(){
        RaycastHit ground = new RaycastHit();
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out ground, Mathf.Infinity)){
            if (ground.collider.tag == "Death Zone"){
                falling = true;
            }
        }
    }
}
