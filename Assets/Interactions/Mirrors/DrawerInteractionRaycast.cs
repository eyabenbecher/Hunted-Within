using UnityEngine;

public class DrawerInteractionRaycast : MonoBehaviour
{
    public GameObject uiPrompt; // Drag your UI element here
    public Animator drawerAnimator; // Drag the drawer's animator here
    public float rayDistance = 3f; // Maximum distance for interaction
    public KeyCode interactKey = KeyCode.E; // Key to trigger interaction

    private InteractableDrawer currentInteractable = null;

    private void Start()
    {
        if (uiPrompt != null)
        {
            uiPrompt.SetActive(false); // Ensure the UI is initially hidden
        }
        else
        {
            Debug.LogError("UI Prompt is not assigned in the Inspector!");
        }

        if (drawerAnimator == null)
        {
            Debug.LogError("Drawer Animator is not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        // Perform a raycast from the camera
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // Visualize the ray in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.green);

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}"); // Log what the ray hit

            InteractableDrawer interactable = hit.collider.GetComponent<InteractableDrawer>();

            if (interactable != null)
            {
                // If a new interactable is detected, activate the UI
                if (currentInteractable == null)
                {
                    Debug.Log("Interactable drawer detected.");
                    uiPrompt.SetActive(true);
                    currentInteractable = interactable;
                }

                // Trigger interaction when pressing the interact key
                if (Input.GetKeyDown(interactKey))
                {
                    Debug.Log("Interaction triggered.");
                    drawerAnimator.SetTrigger("drawerOpen");

                    // Hide UI during the interaction
                    uiPrompt.SetActive(false);
                    currentInteractable = null; // Reset after interaction
                }
            }
            else
            {
                // Raycast hit but not an interactable drawer
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
            Debug.Log("Clearing interaction.");
            uiPrompt.SetActive(false);
            currentInteractable = null;
        }
    }
}
