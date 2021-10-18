using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Saran Krishnaraja
// Got Code from Brakceys FPS Tutorial videos

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public GameObject characterSelect;

    public static bool _CSFlag = false;

    static bool key = false;

    static bool gameOver = false;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene.");
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private const string PLAYER_ID_PREFIX = "Player ";

    private static Dictionary<string, NetworkPlayer> players = new Dictionary<string, NetworkPlayer>();

    public static void RegisterPlayer(string _netID, NetworkPlayer _player) // Register the player
    {
        string _playerID = PLAYER_ID_PREFIX + _netID;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;

        //Debug.Log("Registered Player: " + _playerID);
    }

    public static void UnRegisterPlayer(string _playerID) // Unregister the player
    {
        players.Remove(_playerID);
    }

    public static NetworkPlayer GetPlayer(string _playerID) // Get the player with that ID
    {
        return players[_playerID];
    }

    public static List<NetworkPlayer> GetAllPlayers()   // Get all the players
    {
        List<NetworkPlayer> temp = new List<NetworkPlayer>();

        //Debug.Log(players.Count);

        for (int i = 0; i < players.Count; i++)
        {
            temp.Add(players[PLAYER_ID_PREFIX + i.ToString()]);

        }
        return temp;
    }

    private void Update()
    {
        if (_CSFlag && !gameOver)
        {
            characterSelect.SetActive(false);
        }
    }

    public static void SetKey()
    {
        key = true;
    }

    public static bool GotKey()
    {
        return key;
    }

    public static void GameOver()
    {
        gameOver = true;
    }

    public static bool GetGameOver()
    {
        return gameOver;
    }
}