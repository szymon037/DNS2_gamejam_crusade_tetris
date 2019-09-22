using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    
    void OnEnable() {
        PauseGame();
    }

    void OnDisable() {
        UnpauseGame();
    }

    void PauseGame() {
        Time.timeScale = 0;
    }

    void UnpauseGame() {
        Time.timeScale = 1;
    }
}
