using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


//Gabrielle Hollaender 100623554
// Edited by: Saran Krishnaraja

public class CharacterSelect : NetworkBehaviour
{

    public GameObject[] characterManager;
    public GameObject[] characterText;

    private int index;

    private bool m_isAxisInUse = false;

    public NetworkPlayer _nPlayer;

    public Text text;

    static bool readyFlag = false;

    private void Start()
    {
        foreach (GameObject character in characterManager)
        {
            //Debug.Log(character);
            //only will show the character selected
            character.SetActive(false);
        }

        characterText[0].SetActive(true);
        characterManager[0].SetActive(true);
        index = 0;

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetAxis("PS4_DPadHorizontal") != 0)
        {
            if (m_isAxisInUse == false)
            {
                if (!_nPlayer.readyUp)
                {
                    // Call your event function here.
                    if (Input.GetAxis("PS4_DPadHorizontal") == -1)   // Left Arrow
                    {
                        CmdLeftArrow();
                    }
                    else if (Input.GetAxis("PS4_DPadHorizontal") == 1)   // Right Arrow
                    {
                        CmdRightArrow();
                    }
                    m_isAxisInUse = true;
                }
            }
        }
        if (Input.GetAxisRaw("PS4_DPadHorizontal") == 0)
        {
            m_isAxisInUse = false;
        }
        if (Input.GetButtonDown("J1_X"))  // Select
        {
            CmdSelect();
        }
        if (readyFlag)
        {
            CmdCheck();
        }

    }

    [Command]
    void CmdRightArrow()
    {
        RpcRightArrow();
    }
    [ClientRpc]
    void RpcRightArrow()
    {
        //turn off current model being rendered
        characterManager[index].SetActive(false);
        characterText[index].SetActive(false);
        index++;
        
        //if we reach the end of the character list, return to index 0
        if (index == characterManager.Length)
        {
            index = 0;
        }
        
        characterManager[index].SetActive(true);
        characterText[index].SetActive(true);
    }

    [Command]
    void CmdLeftArrow()
    {
        RpcLeftArrow();
    }
    [ClientRpc]
    void RpcLeftArrow()
    {
        characterManager[index].SetActive(false);
        characterText[index].SetActive(false);
        index--;
        
        if (index < 0)
        {
            index = characterManager.Length - 1;
        }
        
        characterManager[index].SetActive(true);
        characterText[index].SetActive(true);
    }

    [Command]
    void CmdSelect()
    {
        RpcSelect();
    }
    [ClientRpc]
    void RpcSelect()
    {
        text = characterText[index].GetComponent<Text>();

        text.color = Color.green;

        _nPlayer.readyUp = true; // Set the player ready up to true
        _nPlayer.characterIndex = index; // Set the character they chose

        //Debug.Log("Players ready up status: " + _nPlayer.readyUp);
        //Debug.Log("Players index: " + _nPlayer.characterIndex);


        // Loop through all the players in the game 
        List<NetworkPlayer> players;
        int count = 0;
        players = GameManager.GetAllPlayers();

        //Debug.Log("Player Count: " + players.Count);

        if (players.Count >= 2) // More than two players
        {
            foreach (NetworkPlayer player in players)
            {
                if (player.readyUp)
                {
                    count++;
                }
            }
            if (count == players.Count)
            {
                // Start Game
                readyFlag = true;
            }
        }
        else
        {
            Debug.Log("Not enought players to start the game");
        }
    }

    [Command]
    void CmdCheck()
    {
        RpcCheck();
    }
    [ClientRpc]
    void RpcCheck()
    {
        //Debug.Log("Game Starts");

        GameManager._CSFlag = true;

        _nPlayer.CharacterSelect();
        enabled = false;
    }
}
