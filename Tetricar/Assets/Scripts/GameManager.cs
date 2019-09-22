using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static ulong HighScore = 0;
    public ulong CurrentScore = 0,
        PointsPerSecond = 10;
    public GameState CurrentState = GameState.MAIN_MENU;
    public GameState PreviousState;
    public string[] SceneNames;
    public float sizeOfBlock;
    public bool blockAdded = false;
    public TetrisBlockSpawner tetrisBlockSpawner;
    private int points = 0;

    public static GameManager instance;
    public Text text;
    public Text highscoreText;
    public CameraShaker carCameraShaker;
    public Material[] materials;
    public SoundManager soundManager;


    public enum GameState
    {
        MAIN_MENU,
        GAME,
        CREDITS,
        END
    }

    float dtime = 0.0f;

    void Start()
    {
        tetrisBlockSpawner = GameObject.FindWithTag("TetrisBlockSpawner").GetComponent<TetrisBlockSpawner>();
        sizeOfBlock = 10f;

        switch (CurrentState)
        {
            case GameState.MAIN_MENU:
                soundManager.SwitchToMenuMusic();
                break;
            case GameState.GAME:
                soundManager.SwitchToGameMusic();
                break;
        }

        InitGame();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        try {
            HighScore = (ulong)PlayerPrefs.GetInt("Highscore");
        } catch (System.Exception) {}

        highscoreText.text = $"x {HighScore}";
    }

    void Update()
    {

        if (blockAdded)
        {
            tetrisBlockSpawner.SpawnTetris();
            blockAdded = false;
            soundManager.PlayBlockSound();
        }


        if (CurrentState != PreviousState) {
            switch (CurrentState)
            {
                case GameState.GAME:
                    soundManager.SwitchToGameMusic();
                    UpdateScoreOverTime();
                    break;
                case GameState.END:
                    RecordHighScore();
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;
                case GameState.MAIN_MENU:
                    soundManager.SwitchToMenuMusic();
                    break;
            }
            PreviousState = CurrentState;
        }

     //   SceneHandling();
    }

    void SceneHandling()
    {
        var ActualSceneName = SceneManager.GetActiveScene().name;

        if (ActualSceneName != SceneNames[(int)CurrentState])
        {
            SceneManager.LoadScene(SceneNames[(int)CurrentState]);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames[(int)CurrentState]));

            switch (CurrentState)
            {
                
                case GameState.GAME:
                    break;
            }

            //SceneManager.UnloadSceneAsync(ActualSceneName);
        }
    }

    public void OnCoinPickUp(int value)
    {
        CurrentScore += (ulong)value;
    }

    public void RecordHighScore()
    {
        if((ulong)points > HighScore)
        {
            HighScore = (ulong)points;
            PlayerPrefs.SetInt("Highscore", (int)HighScore);
            PlayerPrefs.Save();
        }
    }

    public void RestartScore()
    {
        CurrentScore = 0;
        dtime = 0.0f;
    }

    void UpdateScoreOverTime()
    {
        if (dtime < 1.0f)
        {
            dtime += Time.deltaTime;
        }
        else
        {
            CurrentScore += PointsPerSecond;
            dtime = 0.0f;
        }
    }

    public void InitGame()
    {
        tetrisBlockSpawner.SpawnTetris();
    }

    public void AddPoint()
    {
        points++;
        text.text = "x " + points.ToString();
        soundManager.PlayPickUpSound();
    }
}
