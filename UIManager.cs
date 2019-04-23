using System;
using System.Globalization;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Image = UnityEngine.Experimental.UIElements.Image;

public class UIManager : MonoBehaviour
{
    [Header("User Interface Manager")] 

    //UI
    public Text m_displayText; //Text to Output to Display.
    public Text scoreText;
    public Text twoScore;
    public Text damage;
    public Text stocksText; 
    public Slider playerHealth;
    public Slider twoHealth;
    public Slider loading;

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
            if (!GameManager.instance.isDead)
            {
                TankData tkRef = GameManager.instance.player.GetComponentInParent<TankData>();
                damage.text = "Damage: " + tkRef.tankDamage;
                playerHealth.value = GameManager.instance.player.GetComponent<TankData>().health;
                loading.value = GameManager.instance.player.GetComponentInParent<TankData>().fireRate;
            }
        }
        else
        {
            if (GameManager.instance.player)
            {
                playerHealth.value = GameManager.instance.player.GetComponent<TankData>().health;
                scoreText.text = "Your Score: " + Convert.ToString(GameManager.instance.score);
                damage.text = "";
            }

            if (GameManager.instance.playerTwo)
            {
                twoHealth.value = GameManager.instance.playerTwo.GetComponent<TankData>().health;
                twoScore.text = "Your Score: " + Convert.ToString(GameManager.instance.scoreTwo);
                damage.text = "";
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
