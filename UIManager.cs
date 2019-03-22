using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("User Interface Manager")]

    //Booleans

    //Number Values

    //GameObjects

    //Static Variables
    public static UIManager UiManager;

    //Text
    [HideInInspector] public Text m_displayText; //Text to Output to Display.

    /// <summary>
    /// OnAwake(), Set Static GameObject as GameManager Instance.
    /// </summary>
    private void Awake()
    {
        if (UiManager == null)
        {
            UiManager = this;
        }
        else { Destroy(gameObject); }
    }

    private void Start ()
    {
        m_displayText = GameObject.Find("Display Text").GetComponent<Text>();
    }

	private void Update ()
	{
	    if (GameManager.instance.m_isDead)
	    {
            DisplayText();
	    }
	}

    public void DisplayText()
    {
        m_displayText.enabled = true;
        m_displayText.color = Color.green;
        m_displayText.fontSize = 24;
        m_displayText.text = "| Press R when Ready |";
    }
    public void StartCountDown()
    {
        m_displayText.enabled = true;
        m_displayText.text = "|Game Starts in: "+GameManager.instance.m_startCountDown.ToString("f0")+" seconds|";
    }

    public void DisableText()
    {
        m_displayText.enabled = false;
    }

}
