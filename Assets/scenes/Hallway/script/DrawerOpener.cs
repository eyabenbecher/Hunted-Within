using UnityEngine;

public class DrawerRotator : MonoBehaviour
{
    public enum InteractionMode { Rotate, Translate }
    public InteractionMode mode = InteractionMode.Rotate;

    [Header("Setup")]
    public Transform affectedPart;
    public Transform Player;
    public Vector3 rayOriginOffset = new Vector3(0f, 0.5f, 0f);
    public Vector3 rayDirectionRotation = Vector3.zero;
    public float interactionDistance = 2f;

    [Header("Rotation Settings")]
    public Vector3 openRotationEuler = new Vector3(0f, 90f, 0f);
    public float rotationSpeed = 100f; // degrees per second

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
        if (affectedPart == null)
        {
            Debug.LogError("AffectedPart is not assigned!");
            enabled = false;
            return;
        }

        if (mode == InteractionMode.Rotate)
        {
            closedRotation = affectedPart.localRotation;
            targetRotation = closedRotation;
        }
        else
        {
            closedPosition = affectedPart.localPosition;
            targetPosition = closedPosition;
        }
    }

    void Update()
    {
        if (Player == null)
        {
            Debug.LogWarning("Player not assigned!");
            return;
        }

        Vector3 rayOrigin = transform.position + transform.TransformDirection(rayOriginOffset);
        Vector3 baseDirection = Quaternion.Euler(rayDirectionRotation) * transform.forward;

        Vector3[] directions = new Vector3[]
        {
            baseDirection,
            Quaternion.Euler(0, 15, 0) * baseDirection,
            Quaternion.Euler(0, -15, 0) * baseDirection,
            Quaternion.Euler(10, 0, 0) * baseDirection,
            Quaternion.Euler(-10, 0, 0) * baseDirection,
        };

        foreach (var direction in directions)
        {
            Debug.DrawRay(rayOrigin, direction * interactionDistance, Color.red);

            if (Physics.Raycast(rayOrigin, direction, out RaycastHit hit, interactionDistance))
            {
                // Check if ray hits the player
                if (hit.collider.transform == Player)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        isOpen = !isOpen; // Toggle state

                        if (mode == InteractionMode.Rotate)
                        {
                            targetRotation = isOpen ? Quaternion.Euler(openRotationEuler) : closedRotation;
                        }
                        else
                        {
                            targetPosition = isOpen ? closedPosition + openPositionOffset : closedPosition;
                        }
                        break; // Exit after toggling
                    }
                }
            }
        }

        // Smoothly animate rotation or position
        if (mode == InteractionMode.Rotate)
        {
            affectedPart.localRotation = Quaternion.RotateTowards(affectedPart.localRotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(affectedPart.localRotation, targetRotation) < 0.1f)
                affectedPart.localRotation = targetRotation; // Snap to target
        }
        else
        {
            affectedPart.localPosition = Vector3.MoveTowards(affectedPart.localPosition, targetPosition, translationSpeed * Time.deltaTime);

            if (Vector3.Distance(affectedPart.localPosition, targetPosition) < 0.01f)
                affectedPart.localPosition = targetPosition; // Snap to target
        }
    }

    void OnDrawGizmos()
    {
        if (Player == null || affectedPart == null) return;

        Vector3 rayOrigin = transform.position + transform.TransformDirection(rayOriginOffset);
        Vector3 baseDirection = Quaternion.Euler(rayDirectionRotation) * transform.forward;

        Vector3[] directions = new Vector3[]
        {
            baseDirection,
            Quaternion.Euler(0, 15, 0) * baseDirection,
            Quaternion.Euler(0, -15, 0) * baseDirection,
            Quaternion.Euler(10, 0, 0) * baseDirection,
            Quaternion.Euler(-10, 0, 0) * baseDirection,
        };

        Gizmos.color = Color.green;
        foreach (var dir in directions)
            Gizmos.DrawRay(rayOrigin, dir * interactionDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(rayOrigin, 0.025f);
    }
}
