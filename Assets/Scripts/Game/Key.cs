using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Key : NetworkBehaviour
{
    Player tempPlayer;
    public AudioSource tada;

    public Transform spawnLocations;

    Vector3[] waypoints;
    int index;

    // Use this for initialization
    void Start()
    {
        CmdStart();
    }
	
    [Command]
    void CmdStart()
    {
        RpcStart();
    }
    [ClientRpc]
    void RpcStart()
    {
        // Get the waypoints positions for where the key will spawn
        waypoints = new Vector3[spawnLocations.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = spawnLocations.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, waypoints[i].y, waypoints[i].z);
        }
        index = Random.Range(0, waypoints.Length - 1);

        transform.position = waypoints[index];
    }

	// Update is called once per frame
	void Update () {
        //transform.position = waypoints[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tempPlayer = other.gameObject.GetComponent<Player>();

            if (tempPlayer != null)
            {
                GameManager.SetKey();
                tempPlayer.KeyPickUp();
                tada.Play();
                gameObject.SetActive(false);
            }
        }
    }
}
