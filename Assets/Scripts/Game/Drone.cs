using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=aEPSuGlcTUQ

public class Drone : MonoBehaviour {

    public float moveSpeed = 3f;
    public float rotSpeed = 100f;

    private bool isWandering = false;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;

    public float viewDistance;
    bool foundPlayer = false;
    NetworkPlayer myPlayer;
    //GameObject[] players;
    float startingYPos;

    // So this will be on so the warden cannot see anything till the player is found then this will be turned off which will allow the warden to see the player
    public Behaviour StaticCC;  // For the drone camera

    // Use this for initialization
    void Start()
    {
        startingYPos = transform.position.y;
        //players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(foundPlayer == false)
        {
            Wandering();
            CanSeePlayer();
        }
        else
        {
            // Stop the random wandering
            StopAllCoroutines();

            // Reset the rotation for the camera
            transform.rotation = Quaternion.identity;

            // Follow the player
            Vector3 temp = myPlayer.transform.position;
            temp.y = startingYPos;
            temp.z = myPlayer.transform.position.z - 4;
            transform.position = Vector3.MoveTowards(transform.position, temp, moveSpeed * Time.deltaTime);
        }
    }

    #region Wander

    void Wandering()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotSpeed);
        }
        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotSpeed);
        }
        if (isWalking == true)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
    }

    IEnumerator Wander()
    {
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateLorR = Random.Range(0, 3);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 5);

        isWandering = true;

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        isWandering = false;
    }

    #endregion

    bool CanSeePlayer()
    {
        List<NetworkPlayer> playersInGame = GameManager.GetAllPlayers();    // Get all the players

        foreach (NetworkPlayer player in playersInGame) // Check if the guard is infront of every player in the game
        {
            if (player.characterIndex == 3)     // If your the warden return false
                return false;

            if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
            {
                if (player.DidDroneFindPlayer() == false)    // Check if a drone is already on this player
                {
                    StaticCC.enabled = false;
                    myPlayer = player;
                    foundPlayer = true;
                    player.DroneFoundPlayer(); // Make sure another drone doesn't go on this player
                    return true;
                }
            }
        }
        return false;

        //foreach (GameObject player in players) // Check if the guard is infront of every player in the game
        //{
        //    if (Vector3.Distance(transform.position, player.transform.position) < viewDistance)
        //    {
        //        StaticCC.enabled = false;
        //        myPlayer = player;
        //        foundPlayer = true;
        //        return true;
        //    }
        //}
        //return false;
    }
}
