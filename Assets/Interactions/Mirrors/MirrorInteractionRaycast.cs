using UnityEngine;

public class MirrorInteractionRaycast : MonoBehaviour
{
    public GameObject uiPrompt; // Drag your UI element here
    public Animator playerAnimator; // Drag the player's animator here
    public Animator mirrorAnimator; // Drag the mirror's animator here
    public AudioSource audioSource; // Drag your AudioSource component here
    public AudioClip mirrorOpenSound; // Drag your sound clip here
    public float rayDistance = 3f; // Maximum distance for interaction
    public KeyCode interactKey = KeyCode.E;

    private InteractableMirror currentInteractable = null;

    private void Start()
    {
        uiPrompt.SetActive(false); // Ensure the UI is initially hidden
    }

    private void Update()
    {
        // Perform a raycast from the camera
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            InteractableMirror interactable = hit.collider.GetComponent<InteractableMirror>();

            if (interactable != null)
            {
                // If a new interactable is detected, activate the UI
                if (currentInteractable == null)
                {
                    uiPrompt.SetActive(true);
                    currentInteractable = interactable;
                }

                // Trigger interaction when pressing the interact key
                if (Input.GetKeyDown(interactKey))
                {
                    // Trigger player and mirror animations
                    playerAnimator.SetTrigger("push");
                    mirrorAnimator.SetTrigger("mirror push");

                    // Play the sound when the mirror opens
                    if (audioSource != null && mirrorOpenSound != null)
                    {
                        audioSource.PlayOneShot(mirrorOpenSound);
                    }

                    // Hide UI during the interaction
                    uiPrompt.SetActive(false);
                    currentInteractable = null; // Reset after interaction
                }
            }
            else
            {
                // Raycast hit but not an interactable mirror
                ClearInteraction();
            }
        }
        else
        {
            // Raycast hit nothing
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        // Hide UI and reset current interactable
        if (currentInteractable != null)
        {
            uiPrompt.SetActive(false);
            currentInteractable = null;
        }
    }
}
