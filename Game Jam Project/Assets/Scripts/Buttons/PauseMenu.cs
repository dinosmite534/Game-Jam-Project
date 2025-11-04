using Unity.VisualScripting;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    public void Resume()
    {
        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }

    public void Exitgame()
    { 
    Application.Quit();
    }

    public void MainMenu()
    { 
    //open main menu scene
    
    }
}
