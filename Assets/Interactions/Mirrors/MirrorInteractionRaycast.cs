using UnityEngine;

public class MirrorInteractionRaycast : MonoBehaviour
{
    public GameObject uiPrompt;
    public Animator playerAnimator;
    public Animator mirrorAnimator;
    public AudioSource audioSource;
    public AudioClip mirrorOpenSound;
    public float rayDistance = 10f;
    public float sphereRadius = 1f; // Widen the detection cone
    public KeyCode interactKey = KeyCode.E;

    private InteractableMirror currentInteractable = null;

    private void Start()
    {
        uiPrompt.SetActive(false);
    }

    private void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Draw the ray for debugging
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        // Use SphereCast instead of Raycast
        if (Physics.SphereCast(ray, sphereRadius, out hit, rayDistance))
        {
            InteractableMirror interactable = hit.collider.GetComponent<InteractableMirror>();

            if (interactable != null)
            {
                if (currentInteractable == null)
                {
                    uiPrompt.SetActive(true);
                    currentInteractable = interactable;
                }

                if (Input.GetKeyDown(interactKey))
                {
                    playerAnimator.SetTrigger("push");
                    mirrorAnimator.SetTrigger("mirror push");

                    if (audioSource != null && mirrorOpenSound != null)
                    {
                        audioSource.PlayOneShot(mirrorOpenSound);
                    }

                    uiPrompt.SetActive(false);
                    currentInteractable = null;
                }
            }
            else
            {
                ClearInteraction();
            }
        }
        else
        {
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        if (currentInteractable != null)
        {
            uiPrompt.SetActive(false);
            currentInteractable = null;
        }
    }
}
