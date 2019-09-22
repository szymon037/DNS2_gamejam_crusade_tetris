﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseParent : MonoBehaviour
{

    public GameObject pauseHandler;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pauseHandler.SetActive(!pauseHandler.activeSelf);
        }

        Debug.Log($"Timescale: {Time.timeScale}");
    }
}
