using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Author: Saran Krishnaraja
// Code Based: https://tutorials.daspete.at/unity3d/simon-says

public class DoorMiniGame : NetworkBehaviour {

    List<int> beeps = new List<int>();
    List<int> playerBeeps = new List<int>();

    // Button Gameobjects
    public List<GameObject> buttonsOn;
    public List<GameObject> buttonsOff;

    public Player player;

    // Vars
    public bool win = false;
    public bool lose = false;
    public bool startGame = false;              // Pressed X at the door
    bool once = true;
    bool inputEnabled = false;

    // Camera
    public GameObject miniGameCamera;

    [SerializeField]
    bool playerMovement;

    public AudioSource audioSource;
    public AudioClip doorOpen;

    // Use this for initialization
    void Start () {

        if (!hasAuthority)
            return;

        for (int i = 0; i < 5; i++)
        {
            beeps.Add(Random.Range(1, 5));
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (!hasAuthority)
            return;

        if (startGame == true && once == true)
        {
            StartCoroutine(SimonSays());
            //CmdStopPlayerMovement();
            once = false;
        }

        if (startGame == true)
        {
            if (!inputEnabled)
            {
                return;
            }
            //Debug.Log("Getting Input");

            if (Input.GetButtonDown("J1_X"))
            {
                //Debug.Log("X");

                Check(0);
            }
            else if (Input.GetButtonDown("J1_Square"))
            {
                //Debug.Log("Square");

                Check(1);
            }
            else if (Input.GetButtonDown("J1_Triangle"))
            {
                //Debug.Log("Triangle");

                Check(2);
            }
            else if (Input.GetButtonDown("J1_O"))
            {
                //Debug.Log("O");

                Check(3);
            }
        }

    }

    // Starts the Game
    public void StartGame()
    {
        if (!hasAuthority)
            return;

        //Debug.Log("Started Minigame");
        win = false;
        lose = false;
        startGame = true;
        miniGameCamera.SetActive(true);

        player.playerCamera.SetActive(false);
    }

    IEnumerator SimonSays()
    {
        inputEnabled = false;

        SetBeeps();

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < beeps.Count; i++)
        {
            foreach (var item in buttonsOn)     // Set the on buttons off
            {
                item.SetActive(false);
            }
            foreach (var item in buttonsOff)    // Set the off buttons on
            {
                item.SetActive(true);
            }

            yield return new WaitForSeconds(0.6f);

            // Turn the on buttons on and turn the off button off
            buttonsOn[beeps[i]].SetActive(true);
            buttonsOff[beeps[i]].SetActive(false);

            yield return new WaitForSeconds(1f);

            foreach (var item in buttonsOn)     // Set the on buttons off
            {
                item.SetActive(false);
                //item.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            foreach (var item in buttonsOff)    // Set the off buttons on
            {
                item.SetActive(true);
            }
        }

        inputEnabled = true;

        yield return null;
    }

    void Beep(int index)
    {
        foreach (var item in buttonsOn)     // Set the on buttons off
        {
            item.SetActive(false);
        }
        foreach (var item in buttonsOff)    // Set the off buttons on
        {
            item.SetActive(true);
        }

        // Turn the on buttons on and turn the off button off
        buttonsOn[index].SetActive(true);
        buttonsOff[index].SetActive(false);
    }

    void SetBeeps()
    {
        beeps = new List<int>();
        playerBeeps = new List<int>();

        int beepCount = Random.Range(3, 5);

        for (int i = 0; i < beepCount; i++)
        {
            beeps.Add(Random.Range(0, 4));
        }

    }

    void Check(int index)
    {
        inputEnabled = false;

        Beep(index);

        playerBeeps.Add(index);

        if (beeps[playerBeeps.Count - 1] != index)
        {
            StartCoroutine(Lose());
            return;
        }

        if (beeps.Count == playerBeeps.Count)
        {
            Win();
        }

        inputEnabled = true;
    }

    IEnumerator Lose()
    {
        foreach (var item in buttonsOn)     // Set the on buttons off
        {
            item.SetActive(false);
        }
        foreach (var item in buttonsOff)    // Set the off buttons on
        {
            item.SetActive(true);
        }

        // Turn on the right camera
        miniGameCamera.SetActive(false);
        player.playerCamera.SetActive(true);
        //player.mAnimator.SetBool("Shocked", true);

        // Shock the player
        //yield return new WaitForSeconds(5f);

        //player.mAnimator.SetBool("Shocked", true);
        // Set the stats
        win = false;
        lose = true;
        startGame = false;
        once = true;
        yield return null;
    }

    void Win()
    {
        // Set the stats
        win = true;
        lose = false;
        startGame = false;
        once = true;

        foreach (var item in buttonsOn)     // Set the on buttons on
        {
            item.SetActive(true);
        }
        foreach (var item in buttonsOff)    // Set the off buttons off
        {
            item.SetActive(false);
        }

        // Turn on the right camera
        miniGameCamera.SetActive(false);
        player.playerCamera.SetActive(true);

        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(doorOpen);
    }

    public bool didYouWin()
    {
        return win;
    }

    public bool didYouLose()
    {
        return lose;
    }


    //[Command]
    //void CmdColorRed()
    //{
    //    RpcColorRed();
    //}
    //[ClientRpc]
    //void RpcColorRed()
    //{
    //    foreach (var item in buttons)
    //    {
    //        item.GetComponent<MeshRenderer>().material.color = Color.red;
    //    }
    //}

    //[Command]
    //void CmdColorGreen(int temp)
    //{
    //    RpcColorGreen(temp);
    //}
    //[ClientRpc]
    //void RpcColorGreen(int temp)
    //{
    //    buttons[beeps[temp]].GetComponent<MeshRenderer>().material.color = Color.green;
    //}
}
