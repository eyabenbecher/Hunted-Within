using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class ItemInspector : MonoBehaviour
{
    public Camera inspectionCamera; // Reference to the inspection camera
    public Transform inspectionPoint; // Position near the camera for inspecting items
    public PostProcessVolume postProcessVolume; // Reference to the Post-Processing Volume

    private DepthOfField depthOfField; // Depth of Field effect
    private Transform originalParent; // Store the original parent of the item
    private Vector3 originalPosition; // Store the original position of the item
    private Quaternion originalRotation; // Store the original rotation of the item
    private bool isInspecting = false;

    private MaterialSwitcher materialSwitcher; // Reference to the MaterialSwitcher script

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
        if (isInspecting)
        {
            RotateItem();

            // Exit inspection when Escape is pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndInspection();
            }
        }
    }

    private void OnMouseOver()
    {
        // Trigger inspection with right-click
        if (Input.GetMouseButtonDown(1) && !isInspecting)
        {
            StartInspection();
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
    }
}
