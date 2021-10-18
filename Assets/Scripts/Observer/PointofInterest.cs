using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointofInterest : MonoBehaviour
{
    public static event Action<string> OnPointofInterestEntered;
    
    [SerializeField]
    private string _poiName;

    public string PoiName { get { return _poiName; } }

    private void OnTriggerEnter(Collider other)
    {
        if (OnPointofInterestEntered != null)
            OnPointofInterestEntered(this._poiName);
    }
}
