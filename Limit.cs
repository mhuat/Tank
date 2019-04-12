using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInParent<TankData>())
        {
            other.gameObject.GetComponentInParent<TankData>().health -= 1000;
        }
    }
}
