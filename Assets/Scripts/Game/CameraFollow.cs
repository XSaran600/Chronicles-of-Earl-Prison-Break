using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Saran Krishnaraja

public class CameraFollow : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;// = new Vector3 (0, 10, -10);

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        Vector3 newPos = player.transform.position + offset;

        transform.position = newPos;

        transform.LookAt(player.transform);
    }
}
