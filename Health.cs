using UnityEngine;

public class Health : MonoBehaviour
{
    //Number Values
    public float m_health;

	void Start ()
	{
	    m_health = GetComponent<Health>().m_health; //Set Health in the Inspector.
	}
	
    void Update ()
	{
	    if (m_health<=0f)
	    {
            Destroy(gameObject);
	    }
	}
}
