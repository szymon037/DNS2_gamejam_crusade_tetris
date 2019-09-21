using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static ulong HighScore = 0;
    public ulong CurrentScore = 0,
        PointsPerSecond = 10;
    public GameState CurrentState = GameState.MAIN_MENU;
    public string[] SceneNames;
    public float sizeOfBlock;
    public bool blockAdded = false;
    public TetrisBlockSpawner tetrisBlockSpawner;


    static GameManager instance;


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

        InitGame();
    }

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {

        if (blockAdded)
        {
            tetrisBlockSpawner.SpawnTetris();
            blockAdded = false;
        }

        switch (CurrentState)
        {
            case GameState.GAME:
                UpdateScoreOverTime();
                break;
            case GameState.END:
                Application.Quit();
                break;
        }

        SceneHandling();
    }

    void SceneHandling()
    {
        var ActualSceneName = SceneManager.GetActiveScene().name;

        if (ActualSceneName != SceneNames[(int)CurrentState])
        {
            SceneManager.LoadScene(SceneNames[(int)CurrentState]);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneNames[(int)CurrentState]));

            //SceneManager.UnloadSceneAsync(ActualSceneName);
        }
    }

    public void OnCoinPickUp(int value)
    {
        CurrentScore += (ulong)value;
    }

    public void RecordHighScore()
    {
        if(CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
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
}
