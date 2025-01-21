using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class scoreManager : MonoBehaviour
{   
    public TextMeshProUGUI scoreText; // score text
    public TextMeshProUGUI gameOverScoreText; // score after game over and the highscore
    int score = 0;
    int highscore = 0;

    public static scoreManager instance;

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        highscore = PlayerPrefs.GetInt("Highscore" , 0);
        gameOverScoreText.text = "Score: " + score.ToString() + "Highscore: " + highscore.ToString();
        scoreText.text = "Score: " + score.ToString();    
    }
    
    public void AddScore(int value)
    {
        score += value;
        scoreText.text = "Score: " + score.ToString();

        if(score > highscore)
        {
            PlayerPrefs.SetInt("Highscore", score);
        }

        gameOverScoreText.text = "Score: " + score.ToString() + "\nHighscore: " + PlayerPrefs.GetInt("Highscore").ToString();
    }
}
