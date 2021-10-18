using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

// Timer Code from: https://www.youtube.com/watch?v=E6qEPJUAZNk

public class GameUIManager : NetworkBehaviour
{
    // Key
    public Text keyText;
    [SyncVar] string keyString = "No one has the Key!";
    public GameObject yesKey;
    public GameObject noKey;

    // Generators
    int generatorsDone;
    public GameObject GenOff1;
    public GameObject GenOff2;
    public GameObject GenOff3;
    public GameObject GenOn1;
    public GameObject GenOn2;
    public GameObject GenOn3;

    // Timer 
    public Text uiText;
    public float mainTimer;
    [SyncVar] float timer;
    bool canCount = true;
    bool doOnce = false;
    public GameObject green;
    public GameObject orange;
    public GameObject red;

    public static GameUIManager instance;

    /*
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameUIManager in scene.");
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);

        keyString = "No one has the Key!";
        timer = mainTimer;
    }

    // Use this for initialization
    void Start()
    {
        keyString = "No one has the Key!";
        timer = mainTimer;
    }

    // Update
    private void Update()
    {
        UpdateTimer();  // Updates timer
        UpdateKey();

        if (Input.GetKeyDown(KeyCode.K))
        {
            FinishedGenerator();
        }
    }

    #region Gen

    public void FinishedGenerator()
    {
        generatorsDone++;
        if (generatorsDone == 1)
        {
            GenOff1.SetActive(false);
            GenOff2.SetActive(true);
            GenOff3.SetActive(true);
            GenOn1.SetActive(true);
            GenOn2.SetActive(false);
            GenOn3.SetActive(false);
        }
        else if (generatorsDone == 2)
        {
            GenOff1.SetActive(false);
            GenOff2.SetActive(false);
            GenOff3.SetActive(true);
            GenOn1.SetActive(true);
            GenOn2.SetActive(true);
            GenOn3.SetActive(false);
        }
        else if (generatorsDone == 3)
        {
            GenOff1.SetActive(false);
            GenOff2.SetActive(false);
            GenOff3.SetActive(false);
            GenOn1.SetActive(true);
            GenOn2.SetActive(true);
            GenOn3.SetActive(true);
        }
    }

    #endregion

    #region Key
    // Updates the UI on who has the Key
    public void GotKey(int playerNum)
    {
        noKey.SetActive(false);
        yesKey.SetActive(true);
        keyString = "Player " + playerNum + " has the key!";
    }

    void UpdateKey()
    {
        keyText.text = keyString;
    }

    public bool IsKeyDoorUnlocked()
    {
        if(generatorsDone == 3)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Timer

    // Timer
    void UpdateTimer()
    {
        //Debug.Log("Main Timer: " + mainTimer);
        //Debug.Log("Timer: " + timer);

        if (timer >= 0.0f && canCount)
        {
            timer -= Time.deltaTime;

            if (timer >= ((mainTimer / 3) * 2)) // If the timer is greater than 2/3 of the way done
            {
                green.SetActive(true);
                orange.SetActive(false);
                red.SetActive(false);
            }
            else if (timer >= (mainTimer / 3)) // If the timer is greater than 1/3 of the way done
            {
                green.SetActive(false);
                orange.SetActive(true);
                red.SetActive(false);
            }
            else
            {
                green.SetActive(false);
                orange.SetActive(false);
                red.SetActive(true);
            }

            int milliseconds = (int)(timer * 1000) % 100;
            int seconds = (int)(timer % 60);
            int minutes = (int)(timer / 60) % 60;

            string timerString;

            if (seconds >= 10)
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
            keyText.enabled = false;
            uiText.enabled = false;
            if(isServer)
            {
                SceneManager.LoadScene("Win Warden");
            }
            else
            {
                SceneManager.LoadScene("Lose Prisoner");
            }
        }
    }

    // Reset timer
    void ResetTimer()
    {
        timer = mainTimer;
        canCount = true;
        doOnce = false;
    }
    #endregion

    // Used in player script and turns off UI
    public void GameOver()
    {
        canCount = false;
        doOnce = true;
        uiText.text = "0.00";
        timer = 0.0f;
        keyText.enabled = false;
        uiText.enabled = false;
        if (isServer)
        {
            SceneManager.LoadScene("Lose Warden");
        }
        else
        {
            SceneManager.LoadScene("Win Prisoner");
        }
    }
    */
}
