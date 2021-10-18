using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Author: Saran Krishnaraja
// Code from: https://www.youtube.com/watch?v=zVBrz7ZGJdU&index=21&list=WL&t=0s

public class DoorOpen : MonoBehaviour {

    //public Transform pos1;
    public Transform pos1;

    public float doorSpeed = 2f;

    bool isDoorClosed = true;

    public bool isDoorLocked = false;

    private void Awake()
    {
        isDoorClosed = true;
        isDoorLocked = false;
    }

    private void Start()
    {
        pos1.position = new Vector3(pos1.position.x, transform.position.y, pos1.position.z);
    }

    // Opens the Door
    public void OpenDoor(Collider other, Player tempPlayer)
    {
        if (tempPlayer != null)
        {
            // If the door is not locked open the door without the minigame
            if (!isDoorLocked && isDoorClosed)
            {
                // If X is pressed
                if (tempPlayer.X_Input)
                {
                    // Open the door
                    StartCoroutine("Door");
                    isDoorClosed = false;
                }
            }

            // If the door is locked play the minigame
            if (isDoorLocked && isDoorClosed)
            {
                // If X if pressed and you're not in the minigame
                if (tempPlayer.X_Input && tempPlayer.stopPlayerMovement == false)
                {
                    // Start the Mini Game and stop the player movement
                    tempPlayer.miniGame.StartGame();
                    tempPlayer.stopPlayerMovement = true;
                }
             
                // Checks if you won
                if (tempPlayer.miniGame.didYouWin())
                {
                    // Open the door and make the player move again
                    StartCoroutine("Door");
                    isDoorClosed = false;
                    tempPlayer.stopPlayerMovement = false;
                    tempPlayer.miniGame.win = false;
                    tempPlayer.miniGame.lose = false;
                }

                // Checks if you lost
                if (tempPlayer.miniGame.didYouLose())
                {
                    // Make the player move again
                    tempPlayer.stopPlayerMovement = false;
                    tempPlayer.miniGame.win = false;
                    tempPlayer.miniGame.lose = false;
                }
            }
        }
    }

    // Opens the door
    IEnumerator Door()
    {
        while (Vector3.Distance(transform.position, pos1.position) > 0f)
        {
            transform.position = Vector3.Lerp(transform.position, pos1.position, doorSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
