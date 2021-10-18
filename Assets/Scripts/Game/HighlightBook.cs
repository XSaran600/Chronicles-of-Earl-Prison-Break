using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightBook : MonoBehaviour {

    public Renderer rend;
    public Camera camera;
    public GameObject wardenBook;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            wardenBook.SetActive(!wardenBook.activeSelf);
        }

        /*
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Transform objectHit = hit.transform;

            //Debug.Log(objectHit.name);
            if (objectHit.name == name)
            {
                foreach (Material mat in rend.materials)
                {
                    mat.SetColor("_OutlineColor", Color.red);
                }

                // Turn on the warden book UI
                if (Input.GetMouseButtonDown(1))
                {
                    Debug.Log("turn on UI");
                    wardenBook.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.black);
            }
        }

        // Turn off the warden book UI
        if (Input.GetMouseButtonDown(1))
        {
            if (wardenBook.activeSelf)
            {
                Debug.Log("turn off UI");
                wardenBook.SetActive(false);
            }
        }
        */
    }
}
