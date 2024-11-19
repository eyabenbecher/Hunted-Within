using UnityEngine;

public class MaterialSwitcher : MonoBehaviour
{
    public Material normalMaterial;   // Default material
    public Material outlineMaterial; // Outline material
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material = normalMaterial; // Start with the normal material
        }
    }

    void OnMouseEnter()
    {
        // Check if the objectRenderer still exists before accessing it
        if (objectRenderer != null)
        {
            objectRenderer.material = outlineMaterial; // Apply outline material
        }
    }

    void OnMouseExit()
    {
        // Check if the objectRenderer still exists before accessing it
        if (objectRenderer != null)
        {
            objectRenderer.material = normalMaterial; // Revert to normal material
        }
    }

    void Update()
    {
        // If the object is destroyed externally, reset the reference
        if (objectRenderer != null && objectRenderer.gameObject == null)
        {
            objectRenderer = null;
        }
    }

    void OnDestroy()
    {
        // Ensure we clear references to avoid errors when the object is destroyed
        objectRenderer = null;
    }
}
