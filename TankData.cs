using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankData : MonoBehaviour
{
    [Header("Editable Variable Values:")]
    [Header("")]
    public float m_tankDamage;
    public float m_fireRate;
    public float m_pointValue;
    public float m_resetValue;
    public float m_score;
    public float m_shellForce;
    public GameObject shellPrefab;
    public GameObject tankBarrel;
    public bool destroyed;
}