using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Author: Saran Krishnaraja

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField]
    GameObject characterSelect;

    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    Behaviour[] componentsToDisableWarden;
    [SerializeField]
    Behaviour[] componentsToDisablePrisoner;

    public GameObject Warden;
    public GameObject Prisoner;

    public DoorMiniGame miniGameScript;

    private Rigidbody myRigidbody;
    private Collider myCollider;

    private Player myPlayerScript;
    static int players = 0;

    public GameObject brawler;
    [SerializeField] private Animator brawlerAnimator;

    public GameObject tank;
    [SerializeField] private Animator tankAnimator;

    public GameObject masked;
    [SerializeField] private Animator maskedAnimator;

    static bool wardenSpawned = false;
    static bool wardenSpawned2 = false;

    public int playerNum;

    bool isDroneOn = false;

    public GameObject characterSelectWarden;

    public bool readyUp = false;
    public int characterIndex = 0;

    public CharacterSelect _cS;

    public GameObject WardenCamera;
    public GameObject PrisonerCamera;

    public Vector3[] spawnPositions;

    public GameObject UI;

    public Vector3[] spawnPositionsUI;

    public bool lost = false;

    public Collider _collider;

    public GameObject doorMiniGame;

    private void Start()
    {
        transform.position = spawnPositionsUI[players];

        players++;
        playerNum = players;

        Debug.Log(playerNum);

        DisableComponents(false);

        if (isServer)   // You're the server
        {
            if (isLocalPlayer)   // You're the warden
            {
                // Make sure there is only one warden!
                if (!wardenSpawned)
                {
                    characterSelect.SetActive(false);
                    characterSelectWarden.SetActive(true);
                    readyUp = true;
                    characterIndex = 3; // Warden index

                    wardenSpawned = true;
                }
                else
                {
                    characterSelect.SetActive(true);
                    characterSelectWarden.SetActive(false);
                }

            }
            else // You're the prisoner
            {
                characterSelect.SetActive(true);
                characterSelectWarden.SetActive(false);
            }
        }
        else // You're the client
        {
            if (!isLocalPlayer)   // You're the warden
            {
                // Make sure there is only one warden!
                if (!wardenSpawned)
                {
                    characterSelect.SetActive(false);
                    characterSelectWarden.SetActive(true);
                    readyUp = true;
                    characterIndex = 3; // Warden index

                    wardenSpawned = true;

                }
                else
                {
                    characterSelect.SetActive(true);
                    characterSelectWarden.SetActive(false);
                }
            }
            else // You're the prisoner
            {
                characterSelect.SetActive(true);
                characterSelectWarden.SetActive(false);
            }
        }

        //if (isServer)
        //{
        //    text.color = Color.green;

        //    characterSelect.SetActive(false);
        //    characterSelectWarden.SetActive(true);
        //    readyUp = true;
        //    characterIndex = 3; // Warden index
        //}
        //else
        //{
        //    text.color = Color.red;

        //    characterSelect.SetActive(true);
        //    characterSelectWarden.SetActive(false);
        //}

    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        uint temp = GetComponent<NetworkIdentity>().netId.Value - 2;
        string _netID = temp.ToString();
        NetworkPlayer _player = GetComponent<NetworkPlayer>();

        GameManager.RegisterPlayer(_netID, _player);
    }

    void DisableComponents(bool temp)
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = temp;
        }
    }

    void DisableWardenComponents(bool temp)
    {
        for (int i = 0; i < componentsToDisableWarden.Length; i++)
        {
            componentsToDisableWarden[i].enabled = temp;
        }
    }

    void DisablePrisonerComponents(bool temp)
    {
        for (int i = 0; i < componentsToDisablePrisoner.Length; i++)
        {
            componentsToDisablePrisoner[i].enabled = temp;
        }
    }

    public void DroneFoundPlayer()
    {
        isDroneOn = true;
    }

    public bool DidDroneFindPlayer()
    {
        return isDroneOn;
    }

    public void CharacterSelect()
    {
        transform.position = spawnPositions[playerNum-1];
        character();

        // Set the character selection object to false
        characterSelect.SetActive(false);
        characterSelectWarden.SetActive(false);
        _cS.enabled = false;
    }


    // Function for charcter select
    public void character()
    {
        //Debug.Log("CharacterIndex: " + characterIndex);
        switch (characterIndex)
        {
            case 0: // Brawler
                myPlayerScript = GetComponent<Player>();
                myPlayerScript.mAnimator = brawlerAnimator;
                brawler.SetActive(true);
                tank.SetActive(false);
                masked.SetActive(false);
                Prisoner.SetActive(true);
                Warden.SetActive(false);
                DisableWardenComponents(true);
                WardenCamera.SetActive(false);
                PrisonerCamera.SetActive(true);
                //transform.Rotate(new Vector3(0, 270, 0));
                if (!isLocalPlayer)
                {
                    doorMiniGame.SetActive(false);
                }
                break;
            case 1: // Tank
                myPlayerScript = GetComponent<Player>();
                myPlayerScript.mAnimator = tankAnimator;
                brawler.SetActive(false);
                tank.SetActive(true);
                masked.SetActive(false);
                Prisoner.SetActive(true);
                Warden.SetActive(false);
                DisableWardenComponents(true);
                WardenCamera.SetActive(false);
                PrisonerCamera.SetActive(true);
                //transform.Rotate(new Vector3(0, 270, 0));
                if (!isLocalPlayer)
                {
                    doorMiniGame.SetActive(false);
                }
                break;
            case 2: // Masked Girl
                myPlayerScript = GetComponent<Player>();
                myPlayerScript.mAnimator = maskedAnimator;
                brawler.SetActive(false);
                tank.SetActive(false);
                masked.SetActive(true);
                Prisoner.SetActive(true);
                Warden.SetActive(false);
                DisableWardenComponents(true);
                WardenCamera.SetActive(false);
                PrisonerCamera.SetActive(true);
                //transform.Rotate(new Vector3(0, 270, 0));
                if (!isLocalPlayer)
                {
                    doorMiniGame.SetActive(false);
                }
                break;
            case 3: // Warden
                Warden.SetActive(true);
                Prisoner.SetActive(false);
                UI.SetActive(true);
                DisablePrisonerComponents(true);
                WardenCamera.SetActive(true);
                PrisonerCamera.SetActive(false);
                transform.position = new Vector3(0, 0, 0);
                myRigidbody = GetComponent<Rigidbody>();
                myRigidbody.freezeRotation = true;
                myRigidbody.isKinematic = false;
                myRigidbody.useGravity = false;
                myRigidbody.constraints = RigidbodyConstraints.FreezeAll;
                _collider.enabled = false;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        /*
        // Start after the character select is done
        if(GameManager._CSFlag)
        {
            // This is for when all the prisoners lose all their lives
            List<NetworkPlayer> playersInGame = GameManager.GetAllPlayers();    // Get all the players
            int numOfPlayersLost = 0;

            foreach (NetworkPlayer player in playersInGame) // Check every player if they lost
            {
                if (lost)
                {
                    numOfPlayersLost++;
                }
            }

            //Debug.Log(playersInGame.Count);

            if (playersInGame.Count - 1 == numOfPlayersLost)  // If all the prisoners lose
            {
                Debug.Log("GAME OVER");
                if (isServer)
                {
                    SceneManager.LoadScene("Win Warden");
                }
                else
                {
                    SceneManager.LoadScene("Lose Prisoner");
                }
            }
        }*/
        
    }
}
