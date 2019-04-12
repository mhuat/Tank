using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("AI Controller")]
    [Header("")]
    [Header("Tank Data")]

    //Booleans
    [SerializeField]
    private bool canFire = false;
    private bool ismAgentNotNull;
    public bool canHear;
    public bool canSee;
    private bool bearing = true;

    //[Header("Tank Data")]*******
    //Number Values
    public int pointValue;
    [Header("")] 
    [Header("Field Of View In Degrees")] 
    [SerializeField] [Range(0, 360)]
    private float fieldOfView;
    private float fleeDistance = 20f;
    [Header("")]
    public float hearDistance = 10f;
    public float sightDistance = 10f;
    public float rotationSpeed;

    //Enum
    public enum Personality
    {
        Alert,
        Clever,
        Shy,
        Skillful
    };

    public enum Mood
    {
        Happy,
        Mad,
        Sad,
        Scared
    };

    [Header("Tank Specs")]
    //GameObjects
    public GameObject instance;
    public GameObject m_target;

    //Instances
    public TankData tankData;
    [Header("")] public Personality personality;
    public Patrol patrol;
    private RaycastHit hit;

    //Transforms
    private Transform tf;
    Transform target;
    [HideInInspector] public NavMeshAgent agent;
    private Transform barrel;

    //RigidBody
    private Rigidbody rb;
    
    
    //Instance was initialized to avoid repetition  
    void Awake()
    {
        instance = gameObject;
        agent = instance.GetComponent<NavMeshAgent>(); //Initialize instance's m_agent.
        tankData = instance.GetComponent<TankData>(); //Initialize instance's TankData Instance.
        tf = instance.GetComponent<Transform>(); //Initialize instance's transform.
        hearDistance = instance.GetComponent<AIController>().hearDistance; //Initialize instance's hearDistance.
        sightDistance = instance.GetComponent<AIController>().sightDistance;
        fieldOfView = instance.GetComponent<AIController>().fieldOfView;
        personality = instance.GetComponent<AIController>().personality;
        patrol = instance.GetComponent<Patrol>();
        barrel = tankData.tankBarrel;
        ismAgentNotNull = agent != null;
        rb = instance.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ismAgentNotNull = agent != null;
    }

    void Update()
    {

        if (tankData.health <= 0)
        {
            //agent = null;
            //ismAgentNotNull = false;
            GameManager.instance.score += pointValue;
        }

        if (GameManager.instance.player && !m_target)
        {
            m_target = GameManager.instance.player; //If there is an active Player, initialize player reference.
        }

        if (!GameManager.instance.player)
        {
            canHear = false;
            canSee = false;
        }

        if (GameManager.instance.player)
        {
            CanSee();
            CanHear();
        }

        if (!canHear && ismAgentNotNull){
            agent.isStopped = false;
            target = patrol.patrolPoints[patrol.destination];
        }
        else
        {
            if (personality == Personality.Alert && m_target)
            {
                if (canSee){
                    LoadShell();
                    FireShell();
                }

                if (canHear && ismAgentNotNull)
                {
                    agent.isStopped = true;
                    float torque = 20;
                    float turn = Input.GetAxis("Horizontal");
                    rb.AddTorque(transform.up * torque * turn);
                    Vector3 targetDir = m_target.transform.position - tf.position;
                    float step = rotationSpeed * Time.deltaTime;
                    Vector3 newDir = Vector3.RotateTowards(tf.forward, targetDir, step, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDir);
                    /* //Good
                    target = GameManager.instance.player.transform;
                    Vector3 relativePos = target.position - tf.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);
                    tf.rotation = Quaternion.Lerp(tf.rotation, rotation, rotationSpeed * Time.deltaTime);*/
                }
            }

            if (personality == Personality.Shy && m_target)
            {
                if(canHear) Flee();
            }

            if (personality == Personality.Clever && m_target)
            {
                if (canSee){
                    LoadShell();
                    FireShell();
                }
                if (canHear)
                {
                    float dist = Vector3.Distance(m_target.transform.position, tf.position);
                    if (dist > 15f)
                    {
                        ShiftRotation();
                        MoveToTarget();
                    }
                    else
                    {
                        ShiftRotation();
                        agent.isStopped = true;
                    }

                }
            }
            if (personality == Personality.Skillful && m_target)
            {
                if (canSee){
                    LoadShell();
                    FireShell();
                }
                if (canHear&& ismAgentNotNull)
                {
                    agent.isStopped = true;
                    ShiftRotation();
                }
            }
        }
    }

    void ShiftRotation()
    {
        float torque = 20f;
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(transform.up * torque * turn);
        Vector3 targetDir = m_target.transform.position - tf.position;
        float step = rotationSpeed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(tf.forward, targetDir, step, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }

    void MoveToTarget()
    {
        agent.stoppingDistance = 10f;
        //Chase the Player instance.
        agent.destination =
            m_target.transform
                .position; //Takes the agent component's destination of the AI and sets it to target's updated position.
    }

    void Flee()
    {
        float distance = Vector3.Distance(transform.position, m_target.transform.position);
        if (distance < fleeDistance)
        {
            Vector3 dir = transform.position - m_target.transform.position;
            Vector3 newPos = tf.position + dir;
            agent.SetDestination(newPos);
        }
    }

    void ChangeState(bool baren)
    {
        if (bearing == false)
        {
            agent.speed = 7;
        }

        if (bearing == true)
        {
            agent.speed = 3.5f;
        }
    }

    void CanHear()
    {
        float noiseDistance =
            Vector3.Distance(m_target.transform.position,
                tf.position); //Unity's Distance Formula to return a float.
        if (noiseDistance <= hearDistance)
            canHear = true; //m_agent.stoppingDistance = 10f; return true; } //When Heard Do{};
        else canHear = false; //m_agent.stoppingDistance = 0; return false; } //If not heard Do{};
    }

    void CanSee()
    {
        Vector3 direction = m_target.transform.position - tf.position;
        float angle = Vector3.Angle(direction, tf.forward);
        if (angle <= fieldOfView * 0.5f)
        {
            if (Physics.Raycast(tf.position, tf.TransformDirection(Vector3.forward), out hit, sightDistance))
            {
                canSee = hit.transform.CompareTag("Player");
            }
        }
    }

    void LoadShell(){
        tankData.fireRate -= Time.deltaTime;
        if (tankData.fireRate <= 0f){
            canFire = true;
            tankData.fireRate = tankData.resetValue;
        }
    }
    
    bool FireShell(){
        if (canFire)
        {
            Instantiate(tankData.smoke, barrel.position, barrel.rotation);
            GameObject shellInstance = Instantiate(tankData.shellPrefab, barrel);
            
            shellInstance.tag = "Enemy Shell"; //Tag bullet instance.
            shellInstance.transform.SetParent(gameObject.transform); //Sets this bullet's parent to this instance's transform.
            canFire = false;
        }

        return true;
    }

    //End of class
}

//Usable
//Wave Manipulation(put these two lines of code in the start method.)
//previous = GameManager.instance.L_Enemy.Count; 
//next = previous + 1;
//new Vector3(tankData.tankBarrel.transform.position.x + 1, tankData.tankBarrel.transform.position.y, tankData.tankBarrel.transform.position.z)
//shellInstance.GetComponent<Rigidbody>().AddForce(shellInstance.transform.forward * tankData.m_shellForce, ForceMode.Impulse); //Add force to shell instance's rigid body.

#region Notes
/*
 * 
 *     /
    void MoveToStart(){ //Return to spawn location.
        m_agent.SetDestination(GameManager.instance.enemySpawnLocation.transform.position); //Sets the tank's agent destination to the enemySpawnLocation.
    }//MoveToStart
private void CanSeePlayer()
{
    //null reference
    if (GameManager.instance.player != null)
    {
        Vector3 targetDir = transform.position - GameManager.instance.player.transform.position;
        float angleToPlayer = Vector3.Angle(targetDir, transform.forward);

        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        if (angleToPlayer > startFov && angleToPlayer < endFov)
        {
            canSee = true;
        }
        //Debug.DrawLine(this.transform.position, new Vector3(0, 0, visionDistance), Color.blue);
        /*
        Vector3 targetDir =  ; //The difference from the player instance's position to this gameObject's position results in target's direction. 

        //Debug.Log(targetDir);
        float angleToPlayer = Vector3.Angle(targetDir, transform.forward); //Reference to the angle shot in the player intance's direction.
                                                                            //Debug.DrawLine(angleToPlayer, , Color.red, 1000f);
        Debug.DrawRay(tankData.transform.position, targetDir, Color.red, angleToPlayer);
        if (angleToPlayer > startFov && angleToPlayer < endFov) // 180° FOV 

    }
    else if(GameManager.instance.player == null)
    {
        canFire = false;
        canSee = false;
        Vector3 targetDir = GameManager.instance.enemySpawnLocation.transform.position - this.transform.position; //The difference from the player instance's position to this gameObject's position results in target's direction. 

        //Debug.Log(targetDir);
        float angleToTarget = Vector3.Angle(targetDir, transform.forward); //Reference to the angle shot in the player intance's direction.
                                                                            //Debug.DrawLine(angleToPlayer, , Color.red, 1000f);
        Debug.DrawRay(tankData.transform.position, targetDir, Color.red, angleToTarget);
    }
}

    void CanSee()
    {
        //find vector that points to target- 
        Vector3 vectorToTarget = GameManager.instance.player.transform.position - tf.position;
        //check to see if infield of view
        if (Vector3.Angle(tf.forward, vectorToTarget) < fieldOfView)
        {
            //create a ray(check to see if something is in the way)
            //calculate line of sight
            //starting point, direction no distance(magnitude)
            Ray theRay = new Ray();
            theRay.origin = tf.position;
            theRay.direction = vectorToTarget;
            RaycastHit hitInformation;
            if (Physics.Raycast(theRay, out hitInformation, viewDistance))
            {
                //we hit something
                //check what we hit
                if (hitInformation.collider.gameObject == m_target)
                {

                    canSee = true;

                }
            }
        }
        else
        {
            canSee = false;
        }
        //Debug.DrawLine(transform.position, transform.forward, Color.red);
        //lineRenderer.SetPosition(0, transform.position);
        //lineRenderer.SetPosition(1, transform.forward * 20 + transform.position);
        
        float maxRange = 40f;
        Vector3 leftLateral = new Vector3(-instance.transform.position.x, instance.transform.position.y, instance.transform.position.z + maxRange);
        Vector3 rightLateral = new Vector3(instance.transform.position.x, instance.transform.position.y, instance.transform.position.z + maxRange);
        //left.localPosition = new Vector3(tf.position.x, tf.position.y, tf.position.z+40f);
        //float leftDistance = Vector3.Distance(leftLateral, tf.position);
        Debug.DrawLine(instance.transform.position, leftLateral, Color.green);
        Debug.DrawLine(rightLateral, tf.position, Color.blue);
    }


    void OnDestroy() //Remove this object from the L_Enemy list when destroyed.
    {
        GameManager.instance.L_Enemy.Remove(this.gameObject);
    }
3/3

void LookAtPlayer()
{
    //tf.Rotate(m_target.transform.position, Space.Self);
    timeCount = timeCount - Time.deltaTime;
    Vector3 lookVector = GameManager.instance.player.transform.position - tf.position;
    Quaternion rotateTowards = Quaternion.LookRotation(lookVector);
    tf.rotation = Quaternion.Slerp(tf.rotation, rotateTowards, timeCount);

                //timeCount = timeCount - Time.deltaTime;
            //Vector3 lookVector = m_target.transform.position-tf.position;
           // Quaternion rotateTowards = Quaternion.LookRotation(lookVector);
            //tf.rotation = Quaternion.Slerp(tf.rotation, rotateTowards, timeCount);
}

bool CanHear()
{
    float noiseDistance = Vector3.Distance(tf.position, m_target.transform.position);
    if (noiseDistance <= hearDistance)
    {
        print("Heard!");
        return (true);
    }
    else
    {
        return (false);
    }
    //Debug.DrawLine(tf.position, GameManager.instance.player.transform.position, Color.red, Mathf.Infinity);
    //float noiseDistance = Vector3.Distance(tf.position, GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position);
    //if (noiseDistance <= hearDistance) {print("Heard!"); return (true);}else return (false);

                //if (noiseDistance <= hearDistance) { canHear = true; LookAtPlayer(); MoveToTarget(); m_agent.stoppingDistance = 5; } //Stopping distance should be changed for each individual mood and Personality combination.
            //else (noiseDistance > hearDistance) {canHear = false; m_agent.stoppingDistance = 0; }//MoveToStart(); 
}

bool CanSee()
{
    Vector3 localForward = transform.worldToLocalMatrix.MultiplyVector(transform.forward);
    Vector3 targetDir = GameManager.instance.player.transform.position - transform.position;
    float angle = Vector3.Angle(targetDir, localForward);
    //Debug.DrawLine(targetDir, transform.forward, Color.red);
    //Debug.Log(angle);
    if (angle < dFOV)
    {
        print("Spotted!");
        return true;
    }
    else { return false; }
}

//Debug.DrawLine(GameManager.instance.player.transform.position, instance.transform.position, Color.red);

*/
/*
void LookAtPlayer(){
    var targetPoint = m_target.transform.position;
    var targetRotation = Quaternion.LookRotation(targetPoint - transform.position, Vector3.up);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    //transform.LookAt(m_target.transform);

    transform.rotation = Quaternion.Slerp(transform.rotation, m_target.transform.rotation, timeCount);
    timeCount = timeCount + Time.deltaTime;

    Vector3 lookVector = GameManager.instance.player.transform.position - tf.position;
    Quaternion rotateTowards = Quaternion.LookRotation(lookVector);
    tf.rotation = Quaternion.RotateTowards(tf.rotation, rotateTowards, timeCount);
}*/
//Hello
/*
//Clever
if (m_target != null && personality == Personality.Clever)
{

    if (CanSee()) { LookAtPlayer(); LoadShell(); FireShell(); }
    if (CanHear()) { LookAtPlayer(); MoveToTarget(); }
}
//Skillful
if (m_target != null && personality == Personality.Skillful)
{
    if (CanSee()) { bearring = false; LookAtPlayer(); LoadShell(); FireShell(); }
}
//Shy
if (m_target != null && personality == Personality.Shy)
{
    if (CanSee()) { LookAtPlayer(); Flee(); }
    if (CanHear()) { m_agent.stoppingDistance = 0; LookAtPlayer(); Flee(); }
}
if (personality == Personality.Skillful)
{
    if (m_target == null) bearring = true;
    ChangeState(bearring);
}*/


#endregion