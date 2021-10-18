using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    Vector3 temp;

    // Use this for initialization
    void Start () {
        temp = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.W))
        {
            temp.x += 1f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            temp.x -= 1f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            temp.z += 1f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            temp.z -= 1f;
        }
        transform.position = temp;
    }
}
