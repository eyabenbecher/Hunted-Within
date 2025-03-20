using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

[System.Serializable]
public class CanSee : ActionNode
{
    public GameObject player;
    public GameObject guard;
    public guardProp guardProp;

    public float fovDistance;
    public float fovAngle;

    protected override void OnStart()
    {
        Debug.Log("CanSee: OnStart() called.");

        // Find the player GameObject
        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("CanSee: Player object not found! Make sure a GameObject named 'Player' exists in the scene.");
        }
        else
        {
            Debug.Log("CanSee: Player object found.");
        }

        // Assign guard from behavior tree context
        guard = context?.gameObject;
        if (guard == null)
        {
            Debug.LogError("CanSee: Context or guard is null! Check if this node is correctly assigned in the behavior tree.");
            return;
        }
        Debug.Log("CanSee: Guard object found: " + guard.name);

        // Get guard properties component
        guardProp = guard.GetComponent<guardProp>();
        if (guardProp == null)
        {
            Debug.LogError("CanSee: guardProp component not found on guard! Ensure the guard has a 'guardProp' component attached.");
            return;
        }
        Debug.Log("CanSee: guardProp component found.");

        // Assign Field of View values
        fovDistance = guardProp.fovDistance;
        fovAngle = guardProp.fovAngle;
        Debug.Log($"CanSee: fovDistance = {fovDistance}, fovAngle = {fovAngle}");
    }

    protected override void OnStop()
    {
        Debug.Log("CanSee: OnStop() called.");
    }

    protected override State OnUpdate()
    {
        Debug.Log("CanSee: OnUpdate() called.");

        DrawFOV();

        // Check if player or guard is null before proceeding
        if (player == null)
        {
            Debug.LogError("CanSee: Player is null in OnUpdate! Ensure it's assigned correctly.");
            return State.Failure;
        }
        if (guard == null)
        {
            Debug.LogError("CanSee: Guard is null in OnUpdate! This should not happen if context is set.");
            return State.Failure;
        }

        Debug.Log($"CanSee: Checking visibility of {player.name} from {guard.name}");

        if (ICanSee(player.transform, guard.transform))
        {
            Debug.Log("CanSee: Player is visible!");
            return State.Success;
        }
        else
        {
            Debug.Log("CanSee: Player is NOT visible.");
            return State.Failure;
        }
    }

    private bool ICanSee(Transform player, Transform objectTransform)
    {
        if (player == null || objectTransform == null)
        {
            Debug.LogError("CanSee: ICanSee() received a null transform! Ensure both player and guard are assigned.");
            return false;
        }

        Vector3 direction = player.position - objectTransform.position;
        float angle = Vector3.Angle(direction, objectTransform.forward);

      

        if (angle < fovAngle / 2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(objectTransform.position, direction, out hit, fovDistance))
            {
                Debug.Log($"CanSee: Raycast hit {hit.collider.gameObject.name}");

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("CanSee: Player is within FOV and not obstructed.");
                    return true;
                }
                else
                {
                    Debug.Log("CanSee: Player is obstructed by " + hit.collider.gameObject.name);
                }
            }
        }
        else
        {
            Debug.Log("CanSee: Player is outside of FOV angle.");
        }

        return false;
    }

    void DrawFOV()
    {
        if (context == null)
        {
            Debug.LogError("CanSee: Context is null in DrawFOV()! Ensure the node is correctly set up in the behavior tree.");
            return;
        }

        Vector3 origin = context.transform.position;
        float halfFOV = fovAngle / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * context.transform.forward;
        Vector3 rightRayDirection = rightRayRotation * context.transform.forward;

        Debug.DrawRay(origin, leftRayDirection * fovDistance, Color.green);
        Debug.DrawRay(origin, rightRayDirection * fovDistance, Color.green);

        Debug.DrawLine(origin, origin + leftRayDirection * fovDistance, Color.green);
        Debug.DrawLine(origin, origin + rightRayDirection * fovDistance, Color.green);

        float currentAngle = -halfFOV;
        float step = fovAngle / 10.0f;
        for (int i = 0; i < 10; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
            Vector3 direction = rotation * context.transform.forward;
            Debug.DrawRay(origin, direction * fovDistance, Color.green);
            currentAngle += step;
        }
    }
}
