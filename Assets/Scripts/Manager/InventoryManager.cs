using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager instance;

    void Awake()
    {
        if (instance == null)
        {   //assign manager to current object
            instance = this;
        }
        else if (instance != this)
        {
            //if not equal to instance destroy that object
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        // Inventory
        InventoryWithMemoryManager.LoadDLL();
    }

    public void GotKey(int keyID)
    {
        InventoryWithMemoryManager.gotItem(keyID);  // Got the key
    }

    public bool HasKey(int keyID)
    {
        return InventoryWithMemoryManager.doesHaveItem(keyID); // Check if you have a key
    }
}
