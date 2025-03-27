using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class Search : ActionNode
{
    private GameObject guard;
    private NavMeshAgent agent;
    private GameObject player;
    private Animator animator; 

    protected override void OnStart()
    {
        guard = context.gameObject;
        agent = guard.GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        animator = guard.GetComponent<Animator>(); 

        Debug.Log("Searching for player...");

        
       
        
            animator.SetBool("isRunning", true);
        
    }

    protected override void OnStop()
    {
        
     
            animator.SetBool("isRunning", false);
        
    }

    protected override State OnUpdate()
    {
        if (Vector3.Distance(guard.transform.position, blackboard.moveToPosition) < 0.5f)
        {
            animator.SetBool("isRunning", true);
            return State.Running;
        }
        else
        {
            agent.SetDestination(blackboard.moveToPosition);
            return State.Failure;
        }
    }
}
