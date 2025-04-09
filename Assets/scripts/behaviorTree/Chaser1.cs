using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class Chaser1 : ActionNode
{
    private GameObject player;
    private GameObject guard;
    private Animator guardAnimator; 
    public guardProp guardProp;

    public float vitesse_poursuite;
    public float vitesseRot_poursuite;
    public float precision_poursuite;

    protected override void OnStart()
    {
        
        player = GameObject.Find("Player");
        guard = context.gameObject;

        
        guardAnimator = guard.GetComponent<Animator>();

        guardProp = guard.GetComponent<guardProp>();
        vitesse_poursuite = 3f;
        vitesseRot_poursuite = 5f;
        precision_poursuite = 0.5f;
    }

    protected override void OnStop()
    {
        
        guardAnimator.SetBool("isRunning", false);
    }

    protected override State OnUpdate()
    {
        Poursuivre(player.transform);

        // Calculate the distance between the guard and the player
        float distanceToPlayer = Vector3.Distance(guard.transform.position, player.transform.position);

        // Debug log the distance
        Debug.Log("Distance between guard and player: " + distanceToPlayer);

       
        if (distanceToPlayer < 1.0f)
        {
            Debug.Log("Caught player");
           

            return State.Running;
        }
        else
        {
            blackboard.moveToPosition = player.transform.position;
            Debug.Log("Chasing player");
            guardAnimator.SetBool("isIdle", false);
            guardAnimator.SetBool("isRunning", true);

            return State.Running;
        }
    }


    public void Poursuivre(Transform Player)
    {
        guard.GetComponent<UnityEngine.AI.NavMeshAgent>().isStopped = false; 
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
