using UnityEngine;
using TheKiwiCoder;
using UnityEngine.AI;

[System.Serializable]
public class Patrolll : ActionNode
{
    private Vector3 lastKnownLocation; // The last known location of the player
    private float patrolTimer = 1.0f; // Timer for patrolling
    public float patrolInterval = 3.0f; // Interval between patrolling
    public float patrolDistance; // Maximum distance to patrol from last known location
    private GameObject guard;
    public guardProp guardProp;

    private Animator animator; // Reference to the guard's Animator component
    private float pauseTimer = 0f; // Timer for pausing between patrols
    private bool isPatrolling = false; // Flag to track whether the guard is currently patrolling
    private NavMeshAgent navMeshAgent; // Reference to the guard's NavMeshAgent

    protected override void OnStart()
    {
        // Initialize variables
        lastKnownLocation = context.gameObject.transform.position;
        guard = context.gameObject;
        guardProp = guard.GetComponent<guardProp>();
        patrolDistance = guardProp.distance_patrouille;

        // Get the Animator and NavMeshAgent components
        animator = guard.GetComponent<Animator>();
        navMeshAgent = guard.GetComponent<NavMeshAgent>();

        // Ensure the guard starts with the idle animation
        animator.SetBool("isWalking", false);
        animator.SetBool("isIdle", true);
    }

    protected override void OnStop()
    {
        // Stop patrolling and ensure idle animation is active when stopping the behavior
        animator.SetBool("isWalking", false);
       
        navMeshAgent.isStopped = true; // Stop the NavMeshAgent from moving
    }

    protected override State OnUpdate()
    {
        patrolTimer += Time.deltaTime;

        // If the guard is not patrolling and it's time to start patrolling
        if (patrolTimer >= patrolInterval && !isPatrolling)
        {
            patrolTimer = 0.0f; // Reset patrol timer
            MoveToNextLocation();
        }

        // Handle patrolling and pause
        if (isPatrolling)
        {
            // Check if the guard has reached the destination
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance <= 0.1f)
            {
                // Play idle animation for 15 seconds before patrolling again
                animator.SetBool("isWalking", false); // Stop walking animation
                animator.SetBool("isIdle", true); // Play idle animation

                pauseTimer += Time.deltaTime;

                if (pauseTimer >= 5f) // If the guard has been idle for 5 seconds
                {
                    pauseTimer = 0f; // Reset pause timer
                    MoveToNextLocation(); // Move to the next location after pause
                }
            }
        }

        return State.Running;
    }

    private void MoveToNextLocation()
    {
        
        animator.SetBool("isWalking", true);
        animator.SetBool("isIdle", false); // Stop idle animation

        // Generate a random point within patrolDistance from the last known location
        Vector3 randomPoint = lastKnownLocation + Random.insideUnitSphere * patrolDistance;
        randomPoint.y = 0; // Ensure the point stays on the same level

      
        navMeshAgent.SetDestination(randomPoint);

        isPatrolling = true; // Indicate that the guard is patrolling
    }
}
