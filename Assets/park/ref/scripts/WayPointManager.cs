using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WayPointManager : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    public Transform player;

    [Header("Behavior Settings")]
    public float lookAtPlayerRange = 15f;
    public float proceedToNextWaypointRange = 5f;
    public float audioTriggerRange = 3f; // Distance to start playing audio when close

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip audioToFirstWaypoint;
    public AudioClip audioToNextWaypoint;
    public AudioClip audioToThirdWaypoint;

    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool isWaitingAtWaypoint = false;
    private bool hasPlayedApproachAudio = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (wayPoints.Count > 0)
        {
            GoToWaypoint(0);
        }
        else
        {
            Debug.LogWarning("No waypoints set.");
            agent.isStopped = true;
        }
    }

    void Update()
    {
        if (currentWaypointIndex >= wayPoints.Count)
            return;

        float distanceToWaypoint = Vector3.Distance(transform.position, wayPoints[currentWaypointIndex].position);

        if (!hasPlayedApproachAudio && distanceToWaypoint <= audioTriggerRange)
        {
            PlayApproachAudio(currentWaypointIndex);
            hasPlayedApproachAudio = true;
        }

        if (isWaitingAtWaypoint)
        {
            HandleWaitingBehavior();
        }
        else if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaitingAtWaypoint = true;
        }
    }

    void HandleWaitingBehavior()
    {
        float playerDistance = Vector3.Distance(player.position, transform.position);

        if (playerDistance <= lookAtPlayerRange)
        {
            Vector3 lookDirection = player.position - transform.position;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }

        if (playerDistance <= proceedToNextWaypointRange)
        {
            ProceedToNextWaypoint();
        }
    }

    void ProceedToNextWaypoint()
    {
        currentWaypointIndex++;
        isWaitingAtWaypoint = false;
        hasPlayedApproachAudio = false;

        if (currentWaypointIndex < wayPoints.Count)
        {
            GoToWaypoint(currentWaypointIndex);
        }
        else
        {
            // Reached the final waypoint — stop the agent
            agent.isStopped = true;
            Debug.Log("Final waypoint reached. Agent stopped.");
        }
    }

    void GoToWaypoint(int index)
    {
        if (index < wayPoints.Count)
        {
            agent.SetDestination(wayPoints[index].position);
        }
    }

    void PlayApproachAudio(int index)
    {
        if (audioSource == null) return;

        if (index == 0 && audioToFirstWaypoint != null)
        {
            audioSource.PlayOneShot(audioToFirstWaypoint);
        }
        else if (index == 2 && audioToThirdWaypoint != null)
        {
            audioSource.PlayOneShot(audioToThirdWaypoint);
        }
        else if (audioToNextWaypoint != null)
        {
            audioSource.PlayOneShot(audioToNextWaypoint);
        }
    }
}
