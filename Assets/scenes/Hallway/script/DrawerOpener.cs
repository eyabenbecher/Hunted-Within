using UnityEngine;

public class DrawerRotator : MonoBehaviour
{
    public enum InteractionMode { Rotate, Translate }
    public InteractionMode mode = InteractionMode.Rotate;

    [Header("Setup")]
    public Transform affectedPart;           // The drawer or door to animate
    public Transform player;                 // The player GameObject
    public Vector3 rayOriginOffset = new Vector3(0f, 0.5f, 0f); // Editable in Inspector
    public float interactionDistance = 2f;

    [Header("Rotation Settings")]
    public Vector3 openRotationEuler = new Vector3(0f, 90f, 0f);
    public float rotationSpeed = 2f;

    [Header("Translation Settings")]
    public Vector3 openPositionOffset = new Vector3(0f, 0f, 0.3f);
    public float translationSpeed = 2f;

    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private Vector3 closedPosition;
    private Vector3 targetPosition;

    private bool isOpen = false;

    void Start()
    {
        if (mode == InteractionMode.Rotate)
        {
            closedRotation = affectedPart.localRotation;
            targetRotation = closedRotation;
        }
        else if (mode == InteractionMode.Translate)
        {
            closedPosition = affectedPart.localPosition;
            targetPosition = closedPosition;
        }
    }

    void Update()
    {
        if (player == null || affectedPart == null) return;

        Vector3 rayOrigin = transform.position + transform.TransformDirection(rayOriginOffset);
        Vector3 toPlayer = (player.position + Vector3.up * 0.5f) - rayOrigin;
        float distance = toPlayer.magnitude;
        Vector3 direction = toPlayer.normalized;

        // Main raycast logic
        bool isInFront = Vector3.Dot(transform.forward, direction) > 0.5f;

        if (!isOpen && distance < interactionDistance && isInFront && Input.GetKeyDown(KeyCode.E))
        {
            isOpen = true;
            Debug.Log("Interaction triggered");

            if (mode == InteractionMode.Rotate)
                targetRotation = Quaternion.Euler(openRotationEuler);
            else if (mode == InteractionMode.Translate)
                targetPosition = closedPosition + openPositionOffset;
        }

        // Smooth movement
        if (isOpen)
        {
            if (mode == InteractionMode.Rotate)
                affectedPart.localRotation = Quaternion.Lerp(affectedPart.localRotation, targetRotation, Time.deltaTime * rotationSpeed);
            else if (mode == InteractionMode.Translate)
                affectedPart.localPosition = Vector3.Lerp(affectedPart.localPosition, targetPosition, Time.deltaTime * translationSpeed);
        }
    }

    // ?? Visible in Scene View, not just Play Mode
    void OnDrawGizmos()
    {
        if (player == null) return;

        Vector3 rayOrigin = transform.position + transform.TransformDirection(rayOriginOffset);
        Vector3 toPlayer = (player.position + Vector3.up * 0.5f) - rayOrigin;
        Vector3 direction = toPlayer.normalized;

        float length = interactionDistance;

        // Main ray
        Gizmos.color = Color.green;
        Gizmos.DrawRay(rayOrigin, direction * length);

        // Spread rays
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(rayOrigin, Quaternion.Euler(0, 15, 0) * direction * length);
        Gizmos.DrawRay(rayOrigin, Quaternion.Euler(0, -15, 0) * direction * length);
        Gizmos.DrawRay(rayOrigin, Quaternion.Euler(10, 0, 0) * direction * length);
        Gizmos.DrawRay(rayOrigin, Quaternion.Euler(-10, 0, 0) * direction * length);

        // Ray origin indicator
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(rayOrigin, 0.025f);
    }
}
