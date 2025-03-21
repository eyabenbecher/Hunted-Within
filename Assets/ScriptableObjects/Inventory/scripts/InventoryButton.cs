//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InventoryButton : MonoBehaviour
//{
//    public InventoryObject inventory;
//    public GameObject inventoryUI;
//    public bool seen;

//    private void Start()
//    {
//        // Ensure the inventory UI is initially set to inactive
//        inventoryUI.SetActive(false);
//        seen = false;
//    }

//    public void ShowAndHideInventory()
//    {
//        Debug.Log("Button clicked");  // Debugging line to verify button is triggering the function

//        if (!seen)
//        {
//            // Show the inventory UI and load the inventory
//            inventoryUI.SetActive(true);
//            seen = true;
//            inventory.Load();
//            Debug.Log("Inventory shown");
//        }
//        else
//        {
//            // Hide the inventory UI
//            inventoryUI.SetActive(false);
//            seen = false;
//            Debug.Log("Inventory hidden");
//        }
//    }
//}
