using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duration : MonoBehaviour
{
    public float m_duration;
    void Update(){
        m_duration -= Time.deltaTime;
        if (m_duration < 0f) Destroy(gameObject);
    }
}
