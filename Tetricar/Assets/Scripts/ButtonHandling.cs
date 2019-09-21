using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHandling : MonoBehaviour
{
    public GameManager.GameState TargetGameState;

    public void ChangeGameState()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().CurrentState = TargetGameState;
    }
}
