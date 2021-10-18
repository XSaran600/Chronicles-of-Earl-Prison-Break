using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoorTrigger : MonoBehaviour {

    KeyDoorOpen doorScript;
    public GameObject doorObject;

    Player tempPlayer;

    // Use this for initialization
    void Start()
    {
        doorScript = doorObject.GetComponent<KeyDoorOpen>();
    }

    private void OnTriggerStay(Collider other)
    {
        tempPlayer = other.gameObject.GetComponent<Player>();

        if (tempPlayer != null)
            doorScript.OpenDoor(other, tempPlayer);
    }
}
