using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{   
    public player player;

    // load main game
    public void PlayGame()
    {
        SceneManager.LoadScene("game");

        if(player.ispaused)
        {
            Time.timeScale = 1f;
            player.ispaused = false;
        }
    }

    // quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    // resume game
    public void ResumeGame()
    {
        player.pauseMenuUI.SetActive(false);

        if(player.ispaused)
        {
            Time.timeScale = 1f;
            player.ispaused = false;
        }
    }

    // load main menu
    public void MainMenuGame()
    {
        SceneManager.LoadScene("menu");
    }

    // restart after game over
    public void RestartGame()
    {   
        player.gameOverUI.SetActive(false);
        
        if(player.ispaused)
        {
            Time.timeScale = 1f;
            player.ispaused = false;
        }

        SceneManager.LoadScene("game");
    }

}
