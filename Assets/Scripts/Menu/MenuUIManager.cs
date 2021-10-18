using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIManager : MonoBehaviour
{

    public void StartPrisonBreak()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void QuitPrisonBreak()
    {
        //quit game functionality
        Application.Quit();
        Debug.Log("Game is exiting");
    }
}

