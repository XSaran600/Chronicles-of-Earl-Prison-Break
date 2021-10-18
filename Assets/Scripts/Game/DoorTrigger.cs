using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Saran Krishnaraja

public class DoorTrigger : MonoBehaviour {

    DoorOpen doorScript;
    public GameObject doorObject;

    Player tempPlayer;

    // Use this for initialization
    void Start () {
        doorScript = doorObject.GetComponent<DoorOpen>();
	}

    private void OnTriggerStay(Collider other)
    {
        tempPlayer = other.gameObject.GetComponent<Player>();

        if(tempPlayer != null)
            doorScript.OpenDoor(other, tempPlayer);
    }

}
