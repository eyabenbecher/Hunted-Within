using UnityEngine;

public class ObjectRaycastHandler : MonoBehaviour
{
    [SerializeField] private GameObject canvasUI; // The canvas UI to show/hide
    [SerializeField] private GameObject targetObject; // The object to interact with

    void Start()
    {
        if (canvasUI != null)
        {
            canvasUI.SetActive(false); // Ensure the canvas starts as inactive
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Right-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"Raycast hit: {hit.collider.name}"); // Log hit object name

                // Check if the hit object is the target object
                if (hit.collider.gameObject == targetObject)
                {
                    Debug.Log("Target object clicked!");
                    if (canvasUI != null)
                    {
                        canvasUI.SetActive(true); // Show the canvas
                    }
                }
            }
        }

        // Hide the canvas when Escape is pressed
        if (Input.GetKeyDown(KeyCode.Escape) && canvasUI.activeSelf)
        {
            canvasUI.SetActive(false); // Hide the canvas
        }
    }
}
