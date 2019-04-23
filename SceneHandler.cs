using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static bool multiplayer;
    public static bool FriendlyFire;
    public Image optionsImage;
    public Slider audioVolume;
    public Toggle ff;
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
        ff.gameObject.SetActive(optionsOpened);
        AudioListener.volume=audioVolume.value;
        FriendlyFire = ff.isOn;
        optionsImage.enabled = optionsOpened;
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
