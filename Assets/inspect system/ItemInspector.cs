using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class ItemInspector : MonoBehaviour
{
    public Camera inspectionCamera;
    public Transform inspectionPoint;
    public PostProcessVolume postProcessVolume;
    public LayerMask interactableLayer;
    public Button quitInspectionButton; // UI Button to quit inspection

    private DepthOfField depthOfField;
    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private bool isInspecting = false;

    private MaterialSwitcher materialSwitcher;

    private float minFOV = 20f;
    private float maxFOV = 60f;
    public float zoomSpeed = 10f;

    void Start()
    {
        materialSwitcher = GetComponent<MaterialSwitcher>();

        if (postProcessVolume != null && postProcessVolume.profile.TryGetSettings(out DepthOfField dof))
        {
            depthOfField = dof;
        }

        // Ensure the quit button is hidden at start
        if (quitInspectionButton != null)
        {
            quitInspectionButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isInspecting)
        {
            TryStartInspection();
        }

        if (isInspecting)
        {
            RotateItem();
            HandleZoom();
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

        originalParent = transform.parent;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        transform.position = inspectionPoint.position;
        transform.rotation = Quaternion.identity;
        transform.SetParent(inspectionCamera.transform);

        inspectionCamera.gameObject.SetActive(true);

        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = false;
        }

        // Cursor remains unlocked and visible during inspection

        if (depthOfField != null)
        {
            depthOfField.active = true;
            depthOfField.focusDistance.value = 0.5f;
        }

        if (quitInspectionButton != null)
        {
            quitInspectionButton.gameObject.SetActive(true);
            quitInspectionButton.onClick.AddListener(EndInspection);
        }
    }

    private void RotateItem()
    {
        float rotationSpeed = 100f;
        float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, -xRotation, Space.World);
        transform.Rotate(Vector3.right, yRotation, Space.World);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
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

        transform.SetParent(originalParent);
        transform.position = originalPosition;
        transform.rotation = originalRotation;

        inspectionCamera.gameObject.SetActive(false);

        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = true;
        }

        // Cursor lock and visibility control removed

        if (depthOfField != null)
        {
            depthOfField.active = false;
        }

        inspectionCamera.fieldOfView = maxFOV;

        if (quitInspectionButton != null)
        {
            quitInspectionButton.onClick.RemoveListener(EndInspection);
            quitInspectionButton.gameObject.SetActive(false);
        }
    }
}
