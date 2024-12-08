using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    private bool isTimerRunning = false;
    public float timeLeft;

    public TextMeshProUGUI timerText;
    private string sceneName = "Endgame_Test";

    void Start()
    {
        isTimerRunning = true;
    }

    void Update()
    {
        if(isTimerRunning)
        {
            if (timeLeft > 0f)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                timeLeft = 0f;
                isTimerRunning = false;
            }
        }
        else
        {
            LoadGameOverScene();
        }
    }

    void UpdateTimer(float currentTimer)
    {
        currentTimer += 1;

        float minutes = Mathf.FloorToInt(currentTimer / 60);
        float seconds = Mathf.FloorToInt(currentTimer % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    void LoadGameOverScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
