using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Patrol : MonoBehaviour
{
    AIController aC;
    private NavMeshAgent myAgent;

    [SerializeField] private int myPatrol;
    private GameObject myTile;
    private int n;

    //Lists
    public List<Transform> patrolPoints = new List<Transform>();
    public List<GameObject> tileSpawns = new List<GameObject>();
    public int destination;
    public bool next;
    public bool adam;

    private void Awake()
    {
        aC = GetComponent<AIController>();

        foreach (GameObject tile in GameObject.FindGameObjectsWithTag("TS"))
        {
            tileSpawns.Add(tile);

        }

        int rand = Random.Range(0, tileSpawns.Count);
        myTile = tileSpawns[rand];
        foreach (var tf in myTile.GetComponentsInChildren<Transform>())
        {
            if (tf.gameObject.CompareTag("CS"))
            {
                patrolPoints.Add(tf);
            }
        }
    }

    private void Start()
    {
        myAgent = aC.agent;
        GetPatrol();
    }

    private void Update()
    {

        if (aC.canHear == false) GoPatrol();
        if (!aC.m_target) GoPatrol();
        if (AvoidEnemy() && myPatrol != 1) destination = Random.Range(0, 3);
        if (Arrived() && myPatrol == 0)
        {
            next = !next;
            ChangeVerticalDestination();
            GoPatrol();
        }
        else if (Arrived() && myPatrol == 1)
        {
            adam = !adam;
            bool john = false;
            john = destination == 7;
            if (john)
            {
                destination = 3;
            }

            ChangeCircularDestination();
            GoPatrol();
        }
        else if (Arrived() && myPatrol == 2)
        {
            if (n == 4) n = 0;
            ChangeBoxDestination();
            GoPatrol();
        }
        else if (Arrived() && myPatrol == 3)
        {
            ChangeRoamDestination();
            GoPatrol();
        }
        else return;

    }

    public void GetPatrol()
    {
        int rand = Random.Range(0, 4);
        myPatrol = rand;
    }

    private bool Arrived()
    {
        float dist = myAgent.remainingDistance;
        if (dist != Mathf.Infinity && myAgent.pathStatus == NavMeshPathStatus.PathComplete &&
            myAgent.remainingDistance < 0.01)
            return true;
        else return false;
    }

    private bool AvoidEnemy()
    {
        if (Physics.Raycast(transform.position, transform.forward.normalized, out RaycastHit hit, aC.sightDistance)
        ) //Shoot Raycast
            if (hit.collider.CompareTag("Enemy")) //Check for other Enemies
                return true;
            else
                return false;
        else return false;
    }

    //Patrols Methods
    void GoPatrol()
    {
        myAgent.destination = patrolPoints[destination].position;
    }

    void ChangeVerticalDestination()
    {
        if (next) destination = 0;
        else destination = 2;
        //destination += 2;
        //destination %= 4;

    }

    void ChangeCircularDestination()
    {
        if (adam) destination += 4;
        else destination -= 3;
    }

    void ChangeBoxDestination()
    {
        destination = 3;
        n += 1;
        destination += n;
    }

    void ChangeRoamDestination()
    {
        int random = Random.Range(0, 8);
        destination = random;
    }
}
