using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStart;
    public static event GameDelegate OnGameOver;

    public static GameManager Instance;
    public GameObject Startpage,CountPage, EndPage;
    public Text scoreText;
   
    void OnEnable()
    {
        CountDownText.OnCountdownFinished += OnCountdownFinished;
        TapControl.OnPlayerDied += OnPlayerDied;
        TapControl.OnPlayerScored += OnPlayerScored;

    }
    void OnDisable()
    {
        CountDownText.OnCountdownFinished -= OnCountdownFinished;
        TapControl.OnPlayerDied -= OnPlayerDied;
        TapControl.OnPlayerScored -= OnPlayerScored;
    }
    void OnPlayerDied()
    {
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if(score> savedScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        SetPageState(pageState.GameOver);
    }
    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }
    void OnCountdownFinished()
    {
        SetPageState(pageState.None);
        OnGameStart();
        score = 0;
        gameOver = false;
    }
    enum pageState
    {
        None,
        Start,
        GameOver,
        CountDown
    }
    public int score = 0;
    bool gameOver = false;
    public bool GameOver
    {
        get { return gameOver; }
    }
     void Awake()
    {
        Instance = this;    
    }
    void SetPageState(pageState state)
    {
        switch (state)
        {
            case pageState.None:
                Startpage.SetActive(false);
                EndPage.SetActive(false);
                CountPage.SetActive(false);
                break;
            case pageState.Start:
                Startpage.SetActive(true);
                EndPage.SetActive(false);
                CountPage.SetActive(false);
                break;
            case pageState.GameOver:
                Startpage.SetActive(false);
                EndPage.SetActive(true);
                CountPage.SetActive(false);
                break;
            case pageState.CountDown:
                Startpage.SetActive(false);
                EndPage.SetActive(false);
                CountPage.SetActive(true);
                break;
        }
    }
    public void ComfirmGameOver()
    {
        OnGameOver();
        scoreText.text = "0";
        SetPageState(pageState.Start);
    }
    public void StartGame()
    {
        SetPageState(pageState.CountDown);
    }


}
