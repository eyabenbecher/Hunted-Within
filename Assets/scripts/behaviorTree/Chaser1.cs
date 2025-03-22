using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class Chaser1 : ActionNode
{
    private GameObject player;
    private GameObject guard;
    private Animator guardAnimator; // Animator reference
    public guardProp guardProp;

    public float vitesse_poursuite;
    public float vitesseRot_poursuite;
    public float precision_poursuite;

    protected override void OnStart()
    {
        //Debug.Log("Chasing player...");
        player = GameObject.Find("Player");
        guard = context.gameObject;

        // Get the Animator component attached to the guard
        guardAnimator = guard.GetComponent<Animator>();

        guardProp = guard.GetComponent<guardProp>();
        vitesse_poursuite = 5f;
        vitesseRot_poursuite = 5f;
        precision_poursuite = 0.5f;
    }

    protected override void OnStop()
    {
        // Stop any animations if needed when the action stops
        guardAnimator.SetBool("isRunning", false);
    }

    protected override State OnUpdate()
    {
        Poursuivre(player.transform);

        // Calculate the distance between the guard and the player
        float distanceToPlayer = Vector3.Distance(guard.transform.position, player.transform.position);

        // Debug log the distance
        Debug.Log("Distance between guard and player: " + distanceToPlayer);

        // If the guard is within range of the player, stop chasing
        if (distanceToPlayer < 1.0f)
        {
            Debug.Log("Caught player");

            return State.Running;
        }
        else
        {
            blackboard.moveToPosition = player.transform.position;
            Debug.Log("Chasing player");
            guardAnimator.SetBool("isRunning", true);
            return State.Running;
        }
    }


    public void Poursuivre(Transform Player)
    {
        guard.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false; // Ensure the agent is not stopped
        guard.GetComponent<UnityEngine.AI.NavMeshAgent>().ResetPath();
        Vector3 direction = Player.position - guard.transform.position;
        guard.transform.rotation = Quaternion.Slerp(guard.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * vitesseRot_poursuite);

        // If the guard is far enough from the player, move and play running animation
        if (direction.magnitude > precision_poursuite)
        {
            guard.transform.Translate(0, 0, Time.deltaTime * vitesse_poursuite);

           
           


        }
       
    }
}
