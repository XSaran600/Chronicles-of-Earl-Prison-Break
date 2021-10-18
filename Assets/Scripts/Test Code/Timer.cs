using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Code from: https://www.youtube.com/watch?v=E6qEPJUAZNk

public class Timer : MonoBehaviour {

    [SerializeField] private Text uiText;
    [SerializeField] private float mainTimer = 300f;

    private float timer;
    private bool canCount = true;
    private bool doOnce = false;

    // Use this for initialization
    void Start ()
    {
        timer = mainTimer;
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateTimer();
	}

    public void UpdateTimer()
    {
        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;

            int milliseconds = (int)(timer * 1000) % 1000;
            int seconds = (int)(timer % 60);
            int minutes = (int)(timer / 60) % 60;

            string timerString;

            if (minutes != 0)
            {
                timerString = string.Format("{0:0}:{1:00}", minutes, seconds);
            }
            else
            {
                timerString = string.Format("{0:0}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            }

            //uiText.text = timer.ToString("F");
            uiText.text = timerString;

        }
        else if (timer <= 0.0f && !doOnce)
        {
            canCount = false;
            doOnce = true;
            uiText.text = "0.00";
            timer = 0.0f;
            GameOver();
        }
    }

    public void ResetTimer()
    {
        timer = mainTimer;
        canCount = true;
        doOnce = false;
    }

    void GameOver()
    {
        // Load new scene
        SceneManager.LoadScene("Lose");
    }
}
