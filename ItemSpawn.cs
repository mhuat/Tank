using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawn : MonoBehaviour
{
    public List<GameObject> itemPrefabs = new List<GameObject>();
    public GameObject item;
    public float respawnTimer;
    public float timer;

    void Start(){
        if (!item) {
            Spawn();
        }
    }

    void Update(){
        if (!item){
            timer += Time.deltaTime;
            if (respawnTimer <= timer){
                timer = 0;
                Spawn();
            }
        }  
    }

    void Spawn(){
        item = Instantiate(itemPrefabs[Random.Range(0, itemPrefabs.Count)], transform);
    }
}
