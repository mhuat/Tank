using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class Item : MonoBehaviour
{
    public Type type;
    
    [System.Serializable]
    public enum Type
    {
        AirStrike,
        BoostDamage,
        CameraMissile,
        HealthPack,
        Invisibility
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponentInParent<TankData>())
        {
            TankData tankDataRef = other.gameObject.GetComponentInParent<TankData>();
            if (type == Type.AirStrike)
            {
                tankDataRef.airStrikeAvailable = true;
            }

            if (type == Type.BoostDamage)
            {
                tankDataRef.tankDamage += 30;
            }

            if (type == Type.CameraMissile)
            {
                tankDataRef.cameraMissileAvailable = true;
            }

            if (type == Type.HealthPack)
            {
                tankDataRef.health += 50;
            }

            if (type == Type.Invisibility)
            {
                tankDataRef.invisibilityAvailable = true;
            }
        }
        Destroy(gameObject);
    }   
}
