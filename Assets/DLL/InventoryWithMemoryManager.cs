using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Runtime.InteropServices;

namespace Inventory
{
    public class InventoryWithMemoryManager : MonoBehaviour
    {

        [DllImport("InventoryWithMemoryManagementDLL")]
        public static extern void LoadDLL();

        [DllImport("InventoryWithMemoryManagementDLL")]
        public static extern int addItem();

        [DllImport("InventoryWithMemoryManagementDLL")]
        public static extern void deleteAllItems();

        [DllImport("InventoryWithMemoryManagementDLL")]
        public static extern void gotItem(int id);

        [DllImport("InventoryWithMemoryManagementDLL")]
        public static extern void tookItem(int id);

        [DllImport("InventoryWithMemoryManagementDLL")]
        public static extern bool doesHaveItem(int id);

        void loadDLL()
        {
            LoadDLL();
        }
    }
}