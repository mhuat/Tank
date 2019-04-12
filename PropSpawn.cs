using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawn : MonoBehaviour
{
    
    public GameObject prop;
    public List<GameObject> propList = new List<GameObject>();

    private void Start(){
        int randInt = Random.Range(0, propList.Count);
        prop = propList[randInt];
        Instantiate(prop, transform);
    }
}
