using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gameTimer : MonoBehaviour
{
    public TMP_Text timerText;
    public bool gameRunning = false;
    public GamePlay gamePlay;

    private float elapsedTime = 0f;

    void Update()
    {
        if (gameRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(elapsedTime % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void StartGame()
    {
        gameRunning = true;
        elapsedTime = 0f; // Reset if needed
    }

    public void StopGame()
    {
        gameRunning = false;
        gamePlay.counter.text = "Finished";
        gamePlay.counter.color = Color.blue;
        gamePlay.Finish();
    }
}
