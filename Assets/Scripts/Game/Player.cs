using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using Inventory;

// Author: Saran Krishnaraja
public class Player : NetworkBehaviour {

    public bool stopPlayerMovement = false;     // Stop the player movement
    public float moveSpeed;
    public Animator mAnimator;
    public GameObject playerCamera;
    public GameObject gameCaughtUI;
    public GameObject pauseMenuUI;
    [SyncVar] public int lives = 3;
    public GameObject healthUI3;
    public GameObject healthUI2;
    public GameObject healthUI1;

    public GameObject keySpotLight;

    private Rigidbody myRigidbody;

    Command leftStick_Input = new Move();
    float x;
    float y;
    bool sprint;
    Vector3 spawnPosition;

    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Vector3 lookDir;

    public bool X_Input;

    public DoorMiniGame miniGame;

    bool haveKey = false;

    public int playerNum;

    public AudioSource audioSource;
    public AudioClip footSteps;

    public NetworkPlayer _netPlayer;

    private NetworkManager networkManager;

    //int keyID;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();

        if (!hasAuthority)
            return;

        PauseMenu.IsOn = false;

        spawnPosition = transform.position;
        stopPlayerMovement = false;

        networkManager = NetworkManager.singleton;

        //keyID = InventoryWithMemoryManager.addItem();
    }

    // Update is called once per frame
    void Update ()
    {
        if (!hasAuthority)
        {
            playerCamera.SetActive(false);
            return;
        }
        else
        {
            if (!stopPlayerMovement)
            {
                CmdUpdateMovement();
            }
            CmdButtonPressed();
            CmdCheckCaught();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
                PauseMenu.IsOn = pauseMenuUI.activeSelf;
            }

            checkWin();
        }
    }

    // Update function for pyhsics
    void FixedUpdate()
    {
        CmdFixedUpdateMovement();
    }

    public void KeyPickUp()
    {
        NetworkPlayer temp = gameObject.GetComponent<NetworkPlayer>();
        GameManager.SetKey();
        CmdKeyPickUp();
    }
    [Command]
    void CmdKeyPickUp()
    {
        RpcKeyPickUp();
    }
    [ClientRpc]
    void RpcKeyPickUp()
    {
        keySpotLight.SetActive(true);
        haveKey = true;
    }

        // Check if you got the key and if you leave the map
    void checkWin()
    {
        // Check if you got the key
        if(haveKey)
        {
            //Debug.Log("X: " + gameObject.transform.position.x + " Z: " + gameObject.transform.position.z);

            if (gameObject.transform.position.x < -36f || gameObject.transform.position.x > 36f || gameObject.transform.position.z > 61f || gameObject.transform.position.z < -61f)
            {
                CmdWin();
            }
        }

        if (lives <= 0 && playerCamera.activeSelf)
        {
            CmdLose();
        }
    }

    // Win
    [Command]
    void CmdWin()
    {
        RpcWin();
    }
    [ClientRpc]
    void RpcWin()
    {
        GameManager.GameOver();
        if (isServer)
        {
            SceneManager.LoadScene("Lose Warden");
        }
        else
        {
            SceneManager.LoadScene("Win Prisoner");
        }
    }

    // NEED TO DO ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // Check to see if everyone is dead
    // Implement spectator mode
    [Command]
    void CmdLose()
    {
        RpcLose();
    }
    [ClientRpc]
    void RpcLose()
    {
        stopPlayerMovement = true;
        //playerCamera.SetActive(false);
        //_netPlayer.lost = true;
        if (isServer)
        {
            LeaveRoom();
            SceneManager.LoadScene("Win Warden");
        }
        else
        {
            LeaveRoom();
            SceneManager.LoadScene("Lose Prisoner");
        }
    }

    // Get the Movement Pressed and store them
    [Command]
    void CmdUpdateMovement()
    {
        RpcUpdateMovement();
    }
    [ClientRpc]
    void RpcUpdateMovement()
    {
        // Inputs
        x = Input.GetAxis("J1Horizontal");
        y = Input.GetAxis("J1Vertical");
        sprint = Input.GetButton("J1_L3");
    }

    // Get the buttons pressed and store them
    [Command]
    void CmdButtonPressed()
    {
        RpcButtonPressed();
    }
    [ClientRpc]
    void RpcButtonPressed()
    {
        X_Input = Input.GetButton("J1_X");
    }

    // Update the movement using command pattern
    [Command]
    void CmdFixedUpdateMovement()
    {
        RpcFixedUpdateMovement();
    }
    [ClientRpc]
    void RpcFixedUpdateMovement()
    {
        // Command Pattern Execute
        leftStick_Input.Execute(x, y, sprint, moveSpeed, myRigidbody, transform, mAnimator, audioSource, footSteps, leftStick_Input);   // Moves the Character
    }

    // When the player gets caught
    public void PlayerGotCaught()
    {
        if (!hasAuthority)
            return;

        CmdCaught();
    }

    // Turn on the UI for when you're caught and
    // Remove a life and
    // Stop the movement and
    // Spawn the player back to the jail cell
    [Command]
    void CmdCaught()
    {
        RpcCaught();
    }
    [ClientRpc]
    void RpcCaught()
    {
        if(!gameCaughtUI.activeSelf)
        {
            //mAnimator.SetBool("Caught", true);
            stopPlayerMovement = true;
            gameCaughtUI.SetActive(true);
        }
    }

    [Command]
    void CmdCheckCaught()
    {
        RpcCheckCaught();
    }
    [ClientRpc]
    void RpcCheckCaught()
    {
        if (gameCaughtUI.activeSelf && X_Input)
        {
            lives--;
            //mAnimator.SetBool("Caught", false);

            switch(lives)
            {
                case 2:
                    healthUI3.SetActive(false);
                    healthUI2.SetActive(true);
                    healthUI1.SetActive(false);
                    break;
                case 1:
                    healthUI3.SetActive(false);
                    healthUI2.SetActive(false);
                    healthUI1.SetActive(true);
                    break;
                case 0:
                    healthUI3.SetActive(false);
                    healthUI2.SetActive(false);
                    healthUI1.SetActive(false);
                    break;
            }

            transform.position = spawnPosition;
            stopPlayerMovement = false;
            gameCaughtUI.SetActive(false);
        }
    }

    void LeaveRoom()
    {
        Debug.Log("Leave Room");
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }
}
