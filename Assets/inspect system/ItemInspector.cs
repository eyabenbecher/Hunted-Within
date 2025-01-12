using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ItemInspector : MonoBehaviour
{
    public Camera inspectionCamera; // Reference to the inspection camera
    public Transform inspectionPoint; // Position near the camera for inspecting items
    public PostProcessVolume postProcessVolume; // Reference to the Post-Processing Volume
    public LayerMask interactableLayer; // Layer for interactable objects

    private DepthOfField depthOfField; // Depth of Field effect
    private Transform originalParent; // Store the original parent of the item
    private Vector3 originalPosition; // Store the original position of the item
    private Quaternion originalRotation; // Store the original rotation of the item
    private bool isInspecting = false;

    private MaterialSwitcher materialSwitcher; // Reference to the MaterialSwitcher script

    private float minFOV = 20f; // Minimum field of view for zoom
    private float maxFOV = 60f; // Maximum field of view for zoom
    public float zoomSpeed = 10f; // Speed of zooming

    void Start()
    {
        // Get the MaterialSwitcher script on the same object
        materialSwitcher = GetComponent<MaterialSwitcher>();

        // Fetch Depth of Field from Post-Processing Volume
        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out DepthOfField dof))
        {
            depthOfField = dof;
        }
    }

    void Update()
    {
        // Handle object inspection
        if (Input.GetMouseButtonDown(1) && !isInspecting)
        {
            TryStartInspection();
        }

        // Handle zoom and rotation during inspection
        if (isInspecting)
        {
            RotateItem();
            HandleZoom();

            // Exit inspection when Escape is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndInspection();
            }
        }
    }

    private void TryStartInspection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
        {
            if (hit.transform == transform)
            {
                StartInspection();
            }
        }
    }

    private void StartInspection()
    {
        isInspecting = true;

        // Save the item's original position and parent
        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Move the item to the inspection point
        transform.position = inspectionPoint.position;
        transform.rotation = Quaternion.identity;
        transform.SetParent(inspectionCamera.transform);

        // Enable the inspection camera
        inspectionCamera.gameObject.SetActive(true);

        // Disable the MaterialSwitcher script
        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = false;
        }

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Enable Depth of Field for background blur
        if (depthOfField != null)
        {
            depthOfField.active = true;
            depthOfField.focusDistance.value = 0.5f; // Adjust as needed
        }
    }

    private void RotateItem()
    {
        float rotationSpeed = 100f;
        float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, -xRotation, Space.World); // Rotate around Y-axis
        transform.Rotate(Vector3.right, yRotation, Space.World); // Rotate around X-axis
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            // Adjust the field of view of the camera
            inspectionCamera.fieldOfView = Mathf.Clamp(
                inspectionCamera.fieldOfView - scrollInput * zoomSpeed,
                minFOV,
                maxFOV
            );
        }
    }

    private void EndInspection()
    {
        isInspecting = false;

        // Restore the item's original position and parent
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        // Disable the inspection camera
        inspectionCamera.gameObject.SetActive(false);

        // Re-enable the MaterialSwitcher script
        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = true;
        }

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable Depth of Field for background blur
        if (depthOfField != null)
        {
            depthOfField.active = false;
        }

        // Reset the camera's field of view
        inspectionCamera.fieldOfView = maxFOV;
    }
}
