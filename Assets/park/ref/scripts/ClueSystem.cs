using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSystem : MonoBehaviour
{
    [Header("References")]
    public Camera inspectionCamera;
    public Transform inspectionPoint;
    public Button reInspectButton;

    [Header("Settings")]
    public string playerTag = "Player";
    public float triggerRange = 5f;
    public float zoomSpeed = 10f;
    public float minFOV = 20f;
    public float maxFOV = 60f;

    [Header("Inspectable Objects")]
    public List<GameObject> inspectableObjects = new List<GameObject>();

    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isInspecting = false;
    private GameObject currentItem;
    private MaterialSwitcher materialSwitcher;
    private bool hasTriggered = false;
    private GameObject lastInspectedItem;

    void Start()
    {
        reInspectButton.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckPlayerProximity();

        if (isInspecting)
        {
            RotateItem();
            HandleZoom();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndInspection();
            }
        }
    }

    private void CheckPlayerProximity()
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= triggerRange && !isInspecting && !hasTriggered && inspectableObjects.Count > 0)
            {
                StartRandomInspection();
                hasTriggered = true;
            }
            else if (distance > triggerRange)
            {
                hasTriggered = false;
            }
        }
    }

    private void StartRandomInspection()
    {
        int randomIndex = Random.Range(0, inspectableObjects.Count);
        currentItem = inspectableObjects[randomIndex];
        inspectableObjects.RemoveAt(randomIndex);

        isInspecting = true;

        originalParent = currentItem.transform.parent;
        originalPosition = currentItem.transform.position;
        originalRotation = currentItem.transform.rotation;

        currentItem.transform.position = inspectionPoint.position;
        currentItem.transform.rotation = Quaternion.identity;
        currentItem.transform.SetParent(inspectionCamera.transform);

        materialSwitcher = currentItem.GetComponent<MaterialSwitcher>();
        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = false;
        }

        inspectionCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lastInspectedItem = currentItem;
        reInspectButton.gameObject.SetActive(false);
    }

    private void RotateItem()
    {
        float rotationSpeed = 100f;
        float xRotation = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float yRotation = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        currentItem.transform.Rotate(Vector3.up, -xRotation, Space.World);
        currentItem.transform.Rotate(Vector3.right, yRotation, Space.World);
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
        if (currentItem == null) return;

        isInspecting = false;

        currentItem.transform.SetParent(originalParent);
        currentItem.transform.position = originalPosition;
        currentItem.transform.rotation = originalRotation;

        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = true;
        }

        inspectionCamera.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        inspectionCamera.fieldOfView = maxFOV;
        currentItem = null;

        reInspectButton.gameObject.SetActive(true);
    }

    public void ReInspectLastItem()
    {
        if (lastInspectedItem == null || isInspecting)
        {
            return;
        }

        currentItem = lastInspectedItem;
        isInspecting = true;
        reInspectButton.gameObject.SetActive(false);

        originalParent = currentItem.transform.parent;
        originalPosition = currentItem.transform.position;
        originalRotation = currentItem.transform.rotation;

        currentItem.transform.position = inspectionPoint.position;
        currentItem.transform.rotation = Quaternion.identity;
        currentItem.transform.SetParent(inspectionCamera.transform);

        materialSwitcher = currentItem.GetComponent<MaterialSwitcher>();
        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = false;
        }

        inspectionCamera.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
