using UnityEngine;

public class MirrorInteractionRaycast : MonoBehaviour
{
    public GameObject uiPrompt;
    public Animator playerAnimator;
    public Animator mirrorAnimator;
    public AudioSource audioSource;
    public AudioClip mirrorOpenSound;
    public float rayDistance = 10f;
    public float sphereRadius = 1f;
    public KeyCode interactKey = KeyCode.E;

    private InteractableMirror currentInteractable = null;
    private bool hasInteracted = false;

    private void Start()
    {
        if (uiPrompt != null) uiPrompt.SetActive(false);
    }

    private void Update()
    {
        if (hasInteracted) return; // Prevent repeated interaction

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.SphereCast(ray, sphereRadius, out hit, rayDistance))
        {
            InteractableMirror interactable = hit.collider.GetComponent<InteractableMirror>();

            if (interactable != null)
            {
                if (currentInteractable == null)
                {
                    uiPrompt?.SetActive(true);
                    currentInteractable = interactable;
                }

                if (Input.GetKeyDown(interactKey))
                {
                    playerAnimator?.SetTrigger("push");
                    mirrorAnimator?.SetTrigger("mirror push");

                    if (audioSource != null && mirrorOpenSound != null)
                        audioSource.PlayOneShot(mirrorOpenSound);

                    uiPrompt?.SetActive(false);
                    currentInteractable = null;
                    hasInteracted = true; // Block further interactions
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
            uiPrompt?.SetActive(false);
            currentInteractable = null;
        }
    }
}
