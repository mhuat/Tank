using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol : MonoBehaviour
{
    AIController aC;
    private NavMeshAgent myAgent; 

    [SerializeField]
    private int myPatrol;
    private int n;

    [SerializeField]
    List<Transform> PatrolPoints;
    
    public int destination;
    public bool next;
    public bool adam;

    void Awake(){
        PatrolPoints = new List<Transform>();
        aC = GetComponent<AIController>();
        PatrolPoints.Add(GameObject.Find("North").transform);
        PatrolPoints.Add(GameObject.Find("East").transform);
        PatrolPoints.Add(GameObject.Find("South").transform);
        PatrolPoints.Add(GameObject.Find("West").transform);
        PatrolPoints.Add(GameObject.Find("Northeast").transform);
        PatrolPoints.Add(GameObject.Find("Southeast").transform);
        PatrolPoints.Add(GameObject.Find("Southwest").transform);
        PatrolPoints.Add(GameObject.Find("Northwest").transform);
    }

    void Start()
    {
        myAgent = aC.m_agent;
        GetPatrol();
    }

    void Update()
    {
        if (!aC.canHear) GoPatrol();
        if (!aC.m_target) GoPatrol();
        if (Arrived() && myPatrol == 0) {
            next = !next;
            ChangeVerticalDestination();
            Verticle();
        }
        if (Arrived() && myPatrol == 1){
            adam = !adam;
            bool john = false;
            if (destination == 7) { john = true; }
            if (john) { destination = 3; }
            ChangeCircularDestination();
            Circular();
        }
        if (Arrived() && myPatrol == 2)
        {
            if (n == 4) n = 0;
            ChangeBoxDestination();
            Box();
        }
        if (Arrived() && myPatrol == 3)
        {
            ChangeRoamDestination();
            Roam();
        }

    }

    bool GetPatrol()
    {
        int rand = Random.Range(0, 4);
        myPatrol = rand;
        if (!aC.canHear) return true; else return false;
    }
    
    bool Arrived()
    {
        float dist = myAgent.remainingDistance;
        if (dist != Mathf.Infinity && myAgent.pathStatus == NavMeshPathStatus.PathComplete && myAgent.remainingDistance < 0.01)
            return true;
        else return false;
    }

    //Patrols Methods
    void GoPatrol()
    {
        switch (myPatrol)
        {
            case 0: Verticle(); break;
            case 1: Circular(); break;
            case 2: Box(); break;
            case 3: Roam(); break;
        };
    }

    void Verticle() {
        myAgent.destination = PatrolPoints[destination].position;
    }
    void Circular(){
        myAgent.destination = PatrolPoints[destination].position;
    }
    void Box(){
        myAgent.destination = PatrolPoints[destination].position;
    }
    void Roam(){
        myAgent.destination = PatrolPoints[destination].position;
    }

    void ChangeVerticalDestination(){
        if (next) destination = 0; else destination = 2;
    }
    void ChangeCircularDestination(){
        if (adam) destination += 4; else destination -=3;
    }
    void ChangeBoxDestination(){
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

/*
       //Debug.Log("Arrive: " + Arrived());
       //Debug.Log("Distance: " + myAgent.remainingDistance);
       //foreach(Transform p in PatrolPoints) Debug.Log(p);
       //destination += 2;
       //destination %= 4;
 */
