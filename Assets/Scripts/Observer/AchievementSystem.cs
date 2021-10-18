using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
	// Use this for initialization
	private void Start ()
    {
        PlayerPrefs.DeleteAll();

        PointofInterest.OnPointofInterestEntered += PointofInterest_OnPointofInterestEntered;
	}

    public void PointofInterest_OnPointofInterestEntered(string poiName)
    {
        string achivement = "achievement-" + poiName;

        if (PlayerPrefs.GetInt(achivement) == 1)
            return;

        PlayerPrefs.SetInt(achivement, 1);
        Debug.Log("Unlocked" + poiName);
    }
}
