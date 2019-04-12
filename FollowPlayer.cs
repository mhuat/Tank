using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowPlayer : MonoBehaviour
{
    public Image playerIcon;
    public Image enemyIcon;
    private GameObject playerRef;
    private GameManager gmRef;

    private void Start()
    {
        gmRef = GameManager.instance;
    }


    void Update(){
        if (gmRef.player)
        {
            playerRef = gmRef.player;
            float xPos = playerRef.transform.position.x;
            float zPos = playerRef.transform.position.z;
            transform.position = new Vector3(xPos, 50f, zPos);
        }
    }
}
//electrumite