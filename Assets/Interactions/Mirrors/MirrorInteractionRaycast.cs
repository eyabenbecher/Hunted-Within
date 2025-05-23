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
        if (hasInteracted) return;

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit[] hits = Physics.SphereCastAll(ray, sphereRadius, rayDistance);

        InteractableMirror nearestMirror = null;
        RaycastHit nearestHit = new RaycastHit();
        float closestDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            InteractableMirror mirror = hit.collider.GetComponent<InteractableMirror>();
            if (mirror != null)
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestMirror = mirror;
                    nearestHit = hit;
                }
            }
        }

        if (nearestMirror != null)
        {
            if (currentInteractable == null)
            {
                uiPrompt?.SetActive(true);
                currentInteractable = nearestMirror;
            }

            if (Input.GetKeyDown(interactKey))
            {
                playerAnimator?.SetTrigger("push");
                mirrorAnimator?.SetTrigger("mirror push");

                if (audioSource != null && mirrorOpenSound != null)
                    audioSource.PlayOneShot(mirrorOpenSound);

                uiPrompt?.SetActive(false);
                currentInteractable = null;
                hasInteracted = true; 
            }

            Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);
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
