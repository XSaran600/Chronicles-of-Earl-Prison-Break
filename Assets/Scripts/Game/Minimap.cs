using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gabrielle Hollaender -> 100623554 =^.^=
public class Minimap : MonoBehaviour {

    public Transform player; // public reference to player object

    Quaternion temp;

    private void Start()
    {
        transform.Rotate(new Vector3(0, 0, 0));
        temp = transform.rotation;
    }

    private void LateUpdate() //important for cameras, updates after void Update() in order to receive movement of player
    {
        Vector3 miniMapPos = player.position;
        miniMapPos.y = transform.position.y;

        //transform.LookAt(player);

        transform.rotation = temp;

        transform.position = miniMapPos;
    }
}
