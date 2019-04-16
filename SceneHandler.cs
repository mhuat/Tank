using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static bool multiplayer;
    public Slider audioVolume;
    public bool optionsOpened;

    public void StartGame()
    {
        multiplayer = false;
        SceneManager.LoadScene(1);
    }
    
    public void MultiPlayer()
    {
        multiplayer = true;
        SceneManager.LoadScene(1);
    }

    public void Retry()
    {
        if (!multiplayer)
        {
            StartGame();
        }
        else
        {
            MultiPlayer();
        }
    }

    public void OpenOptionsMenu()
    {
        optionsOpened = !optionsOpened;
        audioVolume.gameObject.SetActive(optionsOpened);
        AudioListener.volume=audioVolume.value;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
