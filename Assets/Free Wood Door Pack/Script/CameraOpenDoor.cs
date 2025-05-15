using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float DistanceOpen = 3;
        public GameObject text;

        public InventoryObject playerInventory; // Reference to the player's inventory
        public ItemObject keyItem;              // The key item required to open "Locked" doors

        private Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
            {
                var door = hit.transform.GetComponent<DoorScript.Door>();
                if (door != null)
                {
                    text.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Check if the door is tagged as "Locked"
                        if (hit.transform.CompareTag("Locked"))
                        {
                            if (HasKey())
                            {
                                animator.SetTrigger("OpenDoor");
                                door.OpenDoor();

                                // 🗝️ Remove one key from inventory
                                RemoveKey();

                                // ✅ Remove the "Locked" tag after opening
                                hit.transform.tag = "Untagged";
                            }
                            else
                            {
                                Debug.Log("This door is locked. You need a key.");
                                // Optional: Show UI feedback or play a sound
                            }
                        }
                        else
                        {
                            // Normal door (no key needed)
                            animator.SetTrigger("OpenDoor");
                            door.OpenDoor();
                        }
                    }
                }
                else
                {
                    text.SetActive(false);
                }
            }
            else
            {
                text.SetActive(false);
            }
        }

        private bool HasKey()
        {
            foreach (var slot in playerInventory.container)
            {
                if (slot.item == keyItem && slot.amount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void RemoveKey()
        {
            foreach (var slot in playerInventory.container)
            {
                if (slot.item == keyItem && slot.amount > 0)
                {
                    slot.amount--; // Remove one key
                    break;
                }
            }
        }
    }
}
