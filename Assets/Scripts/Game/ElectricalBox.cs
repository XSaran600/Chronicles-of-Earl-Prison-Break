using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// Author: Saran Krishnaraja

public class ElectricalBox : NetworkBehaviour
{

    //public Warden wardenScript;

    [SerializeField]
    GameObject popUpText;

    // Checks if you already used it
    [SyncVar (hook = "OnChangeUsed")] public bool alreadyUsed = false;

    // If the UI should be turned on or off
    bool popUp = false;

    public AudioSource genRattle;

    public GameObject GenUI;

    // Function called in ElectricalBoxTrigger
    public void PopUp()
    {
        if(alreadyUsed == false)
            popUp = true;
    }

    public void TurnPowerOff(Collider other, Player tempPlayer)
    {
        // If not already used
        if (alreadyUsed == false)
        {
            // If player presses X
            if (tempPlayer.X_Input)
            {
                Debug.Log("Pressed Gen");

                genRattle.Play();

                // Turn the power off and set already used to true
                //wardenScript.UpdatePower(false);

                //wardenScript.Gen(); // Update the generators done

                //GenUI.SetActive(true);
                popUp = false;      // Turn off UI
                alreadyUsed = true; // Make sure the electrical box is off
            }

            //Debug.Log("Close to Gen");

        }

    }

    void OnChangeUsed(bool temp)
    {
        GenUI.SetActive(temp);
    }

    private void Update()
    {
        if(popUp == true)  
        {
            if (!popUpText.activeSelf)  // If the UI is off turn it on
                popUpText.SetActive(popUp);
        }
        else
        {
            if (popUpText.activeSelf)   // If the UI is on turn it off
                popUpText.SetActive(popUp);
        }

        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    GameManager.SetGenerator();
        //}

        if(alreadyUsed == true)
        {
            Debug.Log("sdfsadfasd");
            GenUI.SetActive(true);
        }
    }
}
