using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalBoxTrigger : MonoBehaviour {

    [SerializeField]
    ElectricalBox electricalBoxScript;

    Player tempPlayer;

    private void OnTriggerStay(Collider other)
    {
        tempPlayer = other.gameObject.GetComponent<Player>();

        if (tempPlayer != null)
        {
            electricalBoxScript.TurnPowerOff(other, tempPlayer);
            electricalBoxScript.PopUp();
        }
    }
}
