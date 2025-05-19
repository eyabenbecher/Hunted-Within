using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueSystem : MonoBehaviour
{
    [Header("References")]
    public Camera inspectionCamera;
    public Transform inspectionPoint;
    public Button reInspectButton;
    public Button exitInspectionButton; 

    [Header("Settings")]
    public string playerTag = "Player";
    public float triggerRange = 5f;
    public float rotationSpeed = 10f;
    public float zoomSpeed = 10f;
    public float minFOV = 20f;
    public float maxFOV = 60f;
    public float initialFOV = 25f;
    public float inspectionDelay = 3f;

    [Header("Inspectable Objects")]
    public List<GameObject> inspectableObjects = new List<GameObject>();

    private Transform originalParent;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private bool isInspecting = false;
    private GameObject currentItem;
    private MaterialSwitcher materialSwitcher;
    private bool inspectionTriggered = false;
    private GameObject lastInspectedItem;

    void Start()
    {
        reInspectButton.gameObject.SetActive(false);
        exitInspectionButton.gameObject.SetActive(false); 
        inspectionCamera.fieldOfView = initialFOV;
    }

    void Update()
    {
        if (!inspectionTriggered)
        {
            GameObject player = GameObject.FindGameObjectWithTag(playerTag);
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                if (distance <= triggerRange && inspectableObjects.Count > 0)
                {
                    Debug.Log("Player entered range. Starting delay.");
                    StartCoroutine(StartInspectionWithDelay());
                    inspectionTriggered = true;
                }
            }
        }

        if (isInspecting)
        {
            RotateItem();
            HandleZoom();
        }
    }

    private IEnumerator StartInspectionWithDelay()
    {
        float elapsedTime = 0f;
        while (elapsedTime < inspectionDelay)
        {
            elapsedTime += Time.deltaTime;
            Debug.Log($"Delay time: {elapsedTime} seconds");
            yield return null;
        }

        Debug.Log("3 seconds passed. Starting inspection.");
        StartRandomInspection();
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
        currentItem.transform.SetParent(inspectionCamera.transform);

        Vector3 startRotation = currentItem.transform.localEulerAngles;
        currentItem.transform.localEulerAngles = new Vector3(90f, 0f, startRotation.z);

        materialSwitcher = currentItem.GetComponent<MaterialSwitcher>();
        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = false;
        }

        inspectionCamera.gameObject.SetActive(true);
      

        inspectionCamera.fieldOfView = initialFOV;

        lastInspectedItem = currentItem;
        reInspectButton.gameObject.SetActive(false);
        exitInspectionButton.gameObject.SetActive(true); 
    }

    private void RotateItem()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        if (currentItem != null)
        {
            Vector3 currentEuler = currentItem.transform.localEulerAngles;
            float zRotation = currentEuler.z > 180 ? currentEuler.z - 360 : currentEuler.z;
            zRotation -= mouseX;
            zRotation = Mathf.Clamp(zRotation, -20f, 20f);
            currentItem.transform.localEulerAngles = new Vector3(currentEuler.x, currentEuler.y, zRotation);
        }
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

    public void EndInspection()
    {
        if (currentItem == null) return;

        isInspecting = false;
        inspectionTriggered = false;

        currentItem.transform.SetParent(originalParent);
        currentItem.transform.position = originalPosition;
        currentItem.transform.rotation = originalRotation;

        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = true;
        }

        inspectionCamera.gameObject.SetActive(false);
      

        inspectionCamera.fieldOfView = initialFOV;
        currentItem = null;

        reInspectButton.gameObject.SetActive(true);
        exitInspectionButton.gameObject.SetActive(false);
    }

    public void ReInspectLastItem()
    {
        if (lastInspectedItem == null || isInspecting) return;

        currentItem = lastInspectedItem;
        isInspecting = true;
        inspectionTriggered = true;
        reInspectButton.gameObject.SetActive(false);
        exitInspectionButton.gameObject.SetActive(true); 

        originalParent = currentItem.transform.parent;
        originalPosition = currentItem.transform.position;
        originalRotation = currentItem.transform.rotation;

        float fixedDistance = 4f;
        Vector3 positionInFrontOfCamera = inspectionCamera.transform.position + inspectionCamera.transform.forward * fixedDistance;

        currentItem.transform.position = positionInFrontOfCamera;
        currentItem.transform.SetParent(inspectionCamera.transform);

        Vector3 startRotation = currentItem.transform.localEulerAngles;
        currentItem.transform.localEulerAngles = new Vector3(90f, 0f, startRotation.z);

        materialSwitcher = currentItem.GetComponent<MaterialSwitcher>();
        if (materialSwitcher != null)
        {
            materialSwitcher.enabled = false;
        }

        inspectionCamera.gameObject.SetActive(true);
       
        inspectionCamera.fieldOfView = initialFOV;
    }
}
