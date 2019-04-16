using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("User Interface Manager")]
    
    //Static Variables
    //public static UIManager UiManager;    

    //UI
    public Text m_displayText; //Text to Output to Display.
    public Text scoreText;
    public Text twoScore;
    public Text stocksText; 
    public Slider playerHealth;
    public Slider twoHealth;

    /*private void Awake()
    {
        if (UiManager == null)
        {
            UiManager = this;
        }
        else { Destroy(gameObject); }
    }*/

    private void Start ()
    {
        if (!SceneHandler.multiplayer)
        {
            m_displayText = GameObject.Find("Display Text").GetComponent<Text>();
            playerHealth = GetComponentInChildren<Slider>();
        }
    }

	private void Update ()
	{
        if (!SceneHandler.multiplayer)
        {
            if (GameManager.instance.isDead)
            {
                DisplayText();
            }
            else
            {
                DisableText();
            }

            stocksText.text = "Stocks: " + Convert.ToString(GameManager.instance.stocks);
            scoreText.text = "Your Score: " + Convert.ToString(GameManager.instance.score);
            if (GameManager.instance.player)
            {
                playerHealth.value = GameManager.instance.player.GetComponent<TankData>().health;
            }
        }
        else
        {
            if (GameManager.instance.player)
            {
                playerHealth.value = GameManager.instance.player.GetComponent<TankData>().health;
                scoreText.text = "Your Score: " + Convert.ToString(GameManager.instance.score);
            }

            if (GameManager.instance.playerTwo)
            {
                twoHealth.value = GameManager.instance.playerTwo.GetComponent<TankData>().health;
                twoScore.text = "Your Score: " + Convert.ToString(GameManager.instance.scoreTwo);
            }
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
        m_displayText.text = "|Game Starts in: "+GameManager.instance.startCountDown.ToString("f0")+" seconds|";
    }

    public void DisableText()
    {
        m_displayText.enabled = false;
    }
}
