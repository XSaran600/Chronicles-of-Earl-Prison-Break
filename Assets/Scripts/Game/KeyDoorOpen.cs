using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorOpen : MonoBehaviour {

    public Transform pos1;

    public float doorSpeed = 2f;

    public Warden wardenScript;

    // Opens the Door
    public void OpenDoor(Collider other, Player tempPlayer)
    {
        if (tempPlayer != null)
        {
            if (GameManager.GotKey())
            {
                if (tempPlayer.X_Input)
                {
                    // Open the door
                    StartCoroutine("Door");
                }
            }
            else
            {
                Debug.Log("Don't have key");
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