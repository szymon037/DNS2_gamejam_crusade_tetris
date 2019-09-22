using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuFunctions : MonoBehaviour
{

    public Button playButton;
    public GameObject menu;
    public GameObject[] objectsToDisable;


    public void StartGame() {
        Time.timeScale = 1;
        Text t = playButton.gameObject.GetComponentInChildren<Text>();
        t.text = "RESUME";
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => ContinueGame());
        GameManager.instance.CurrentState = GameManager.GameState.GAME;
        SwitchMenu(false);
    }

    void ContinueGame() {
        Time.timeScale = 1;
        SwitchMenu(false);
    }

    void Awake() {
        Time.timeScale = 0;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchMenu(!menu.gameObject.activeSelf);
            if (menu.activeSelf) {
                GameManager.instance.CurrentState = GameManager.GameState.MAIN_MENU;
            } else {
                GameManager.instance.CurrentState = GameManager.GameState.GAME;
            }
        }
    }

    public void Exit() {
        Application.Quit();
    }

    void SwitchMenu(bool value) {
        menu.SetActive(value);
        foreach (var obj in objectsToDisable) {
            obj.SetActive(value);
        }
    }
}
