using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

    public Renderer rend;
    public Camera camera;

    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {
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
            }
        }
        else
        {
            foreach (Material mat in rend.materials)
            {
                mat.SetColor("_OutlineColor", Color.black);
            }
        }
    }
}
