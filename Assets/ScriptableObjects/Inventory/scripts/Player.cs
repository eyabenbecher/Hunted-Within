using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    public Camera playerCamera;
    public AudioClip pickupSound;        // Reference to the pickup sound clip

    private AudioSource audioSource;     // Private reference to the AudioSource

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the attached AudioSource
    }

    void Update()
    {
        // Detect mouse click
        if (Input.GetMouseButtonDown(0))
        {
            TryPickupItem();
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            Debug.Log("saved");
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("enter");
            inventory.Load();
        }
    }

    private void TryPickupItem()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var item = hit.collider.GetComponent<Item>();
            if (item)
            {
                inventory.AddItem(item.item, 1);
                Destroy(hit.collider.gameObject);

                // Play the pickup sound
                if (pickupSound) // Ensure the clip is assigned
                {
                    audioSource.PlayOneShot(pickupSound);
                }

                Debug.Log("Picked up " + item.item.name);
            }
        }
    }

    //// private void OnApplicationQuit()
    // {
    //     inventory.container.Clear();
    // }
}
