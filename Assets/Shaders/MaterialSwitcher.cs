using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material normalMaterial;   // Default material
    public Material outlineMaterial; // Outline material
    public GameObject uiElement;     // Reference to the UI element in the scene
    private Renderer objectRenderer;

    private Camera mainCamera;       // Reference to the main camera
    private bool isHovered;          // To track hover state

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();

        // Start with the normal material
        if (objectRenderer != null)
        {
            objectRenderer.material = normalMaterial;
        }

        // Ensure the UI is initially hidden
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }

        // Cache the main camera reference
        mainCamera = Camera.main;
    }

    void Update()
    {
        // If the GameObject is destroyed or disabled, hide the UI
        if (gameObject == null || !gameObject.activeInHierarchy)
        {
            if (uiElement != null && uiElement.activeSelf)
            {
                uiElement.SetActive(false);
            }
            return;
        }

        // Make the UI face the camera
        if (uiElement != null && mainCamera != null)
        {
            uiElement.transform.LookAt(mainCamera.transform);
            uiElement.transform.Rotate(0f, 180f, 0f); // Flip the UI if it faces the wrong way
        }
    }

    void OnMouseEnter()
    {
        // Switch to outline material
        if (objectRenderer != null)
        {
            objectRenderer.material = outlineMaterial;
        }

        // Show the UI
        if (uiElement != null)
        {
            uiElement.SetActive(true);
        }

        isHovered = true; // Mark that the object is being hovered over
    }

    void OnMouseExit()
    {
        // Switch back to normal material
        if (objectRenderer != null)
        {
            objectRenderer.material = normalMaterial;
        }

        // Hide the UI
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }

        isHovered = false; // Mark that the object is no longer being hovered over
    }

    void OnDestroy()
    {
        // Hide the UI if the object is destroyed
        if (uiElement != null)
        {
            uiElement.SetActive(false);
        }
    }
}
