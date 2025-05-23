using UnityEngine;
using UnityEngine.UI;

public class PlayerProximityUI : MonoBehaviour
{
    public float detectionRadius = 3f;
    public string interactableTag = "Player";
    public GameObject promptCanvas;

    private Transform currentTarget;
    private bool canvasDisabledByClick = false;  // Flag to prevent showing canvas again after click hides it

    void Start()
    {
        if (promptCanvas != null)
            promptCanvas.SetActive(false);
    }

    void Update()
    {
        if (!canvasDisabledByClick)
        {
            CheckForInteractables();
        }

        // If this object is tagged "Drawer" and player presses E, hide canvas and disable it from showing again
        if (CompareTag("Drawer") && Input.GetKeyDown(KeyCode.E))
        {
            HideCanvas();
            canvasDisabledByClick = true;
        }

        if (currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);

            if (distance > detectionRadius)
            {
                HideCanvas();
                currentTarget = null;
                return;
            }

            // Hide canvas if left or right click on any item NOT tagged "Drawer"
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform != null && !hit.transform.CompareTag("Drawer"))
                    {
                        HideCanvas();
                        currentTarget = null;
                        canvasDisabledByClick = true;  // Prevent showing again
                        return;
                    }
                }
            }
        }
    }

    void CheckForInteractables()
    {
        if (currentTarget != null) return;

        GameObject[] interactables = GameObject.FindGameObjectsWithTag(interactableTag);
        float closestDistance = detectionRadius;
        Transform closest = null;

        foreach (GameObject obj in interactables)
        {
            float distance = Vector3.Distance(transform.position, obj.transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closest = obj.transform;
            }
        }

        if (closest != null)
        {
            currentTarget = closest;
            ShowCanvas();
        }
    }

    void ShowCanvas()
    {
        if (promptCanvas != null)
            promptCanvas.SetActive(true);
    }

    void HideCanvas()
    {
        if (promptCanvas != null)
            promptCanvas.SetActive(false);
    }
}
