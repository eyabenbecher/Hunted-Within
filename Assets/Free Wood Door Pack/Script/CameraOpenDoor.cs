using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CameraDoorScript
{
    public class CameraOpenDoor : MonoBehaviour
    {
        public float DistanceOpen = 3;
        public GameObject text;

        private Animator animator; // Character's Animator

        void Start()
        {
            // Get the Animator component from this GameObject
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, DistanceOpen))
            {
                if (hit.transform.GetComponent<DoorScript.Door>())
                {
                    text.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        // Trigger the "OpenDoor" animation
                        animator.SetTrigger("OpenDoor");

                        // Open the door
                        hit.transform.GetComponent<DoorScript.Door>().OpenDoor();
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
    }
}
