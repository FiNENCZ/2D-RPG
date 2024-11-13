using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPauseMenu : Singleton<UIPauseMenu>
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
            } else 
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
     
    public void RestartGame()
    {
        Debug.Log("Restart");
        SceneManagement.Instance.RestartGame();
        Resume();
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
