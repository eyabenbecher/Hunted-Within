using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
    public LayerMask kickLayers;         // Objects on this layer will trigger the Kick animation
    public LayerMask pushWithHandsLayers; // Objects on this layer will trigger PushWithHands

    public bool canPush = true;
    public float strength = 1.1f;

    public Animator animator; // Assign in Inspector

    private Rigidbody lastHitBody;
    private Vector3 lastPushDir;
    private string lastAnimationTrigger;

    private void Update()
    {
        if (canPush && lastHitBody != null && Input.GetKeyDown(KeyCode.E))
        {
            // Apply the force
            lastHitBody.AddForce(lastPushDir * strength, ForceMode.Impulse);

            // Play the correct animation
            if (!string.IsNullOrEmpty(lastAnimationTrigger))
            {
                animator.SetTrigger(lastAnimationTrigger);
            }

            // Clear after pushing
            lastHitBody = null;
            lastAnimationTrigger = null;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!canPush) return;

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic) return;

        int objectLayer = body.gameObject.layer;
        int objectLayerMask = 1 << objectLayer;

        // Determine animation trigger based on object's layer
        if ((objectLayerMask & kickLayers) != 0)
        {
            lastAnimationTrigger = "Kick";
        }
        else if ((objectLayerMask & pushWithHandsLayers) != 0)
        {
            lastAnimationTrigger = "PushWithHands";
        }
        else
        {
            return; // Not in any interactable layer
        }

        if (hit.moveDirection.y < -0.3f) return;

        lastHitBody = body;
        lastPushDir = new Vector3(hit.moveDirection.x, 0f, hit.moveDirection.z);
    }
}
