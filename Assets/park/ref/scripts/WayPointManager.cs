using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointManager : MonoBehaviour
{
    public List<Transform> wayPoints = new List<Transform>();
    public float moveSpeed = 3f;
    public Transform player;

    [Header("Behavior Settings")]
    public float waypointProximity = 0.5f;
    public float lookAtPlayerRange = 15f;
    public float proceedToNextWaypointRange = 5f;

    [Header("Obstacle Avoidance")]
    public LayerMask obstacleMask;
    public float detectionRadius = 1f;
    public float avoidanceForce = 3f;

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioClip audioToFirstWaypoint;
    public AudioClip audioToNextWaypoint;
    public AudioClip audioToThirdWaypoint;

    private int currentWaypointIndex = 0;
    private bool isWaitingAtWaypoint = false;
    private Vector3 moveDirection;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        MoveToFirstWaypoint();
    }

    void Update()
    {
        if (currentWaypointIndex >= wayPoints.Count) return;

        if (isWaitingAtWaypoint)
        {
            HandleWaitingBehavior();
        }
        else
        {
            MoveToCurrentWaypoint();
        }
    }

    void MoveToFirstWaypoint()
    {
        currentWaypointIndex = 0;
        isWaitingAtWaypoint = false;

        if (audioSource && audioToFirstWaypoint)
            audioSource.PlayOneShot(audioToFirstWaypoint);
    }

    void MoveToCurrentWaypoint()
    {
        Vector3 targetPos = wayPoints[currentWaypointIndex].position;
        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance <= waypointProximity)
        {
            StartWaiting();
            return;
        }

        Vector3 desiredDirection = (targetPos - transform.position).normalized;
        Vector3 avoidance = CalculateAvoidance(desiredDirection);
        moveDirection = (desiredDirection + avoidance).normalized;

        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void StartWaiting()
    {
        isWaitingAtWaypoint = true;
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

        if (audioSource)
        {
            if (currentWaypointIndex == 2 && audioToThirdWaypoint != null)
            {
                audioSource.PlayOneShot(audioToThirdWaypoint);
            }
            else if (audioToNextWaypoint != null)
            {
                audioSource.PlayOneShot(audioToNextWaypoint);
            }
        }
    }

    Vector3 CalculateAvoidance(Vector3 desiredDirection)
    {
        Vector3 avoidance = Vector3.zero;

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, desiredDirection, out hit, detectionRadius, obstacleMask))
        {
            Vector3 hitNormal = hit.normal;
            Vector3 avoidanceDir = new Vector3(hitNormal.z, 0, -hitNormal.x);

            if (Vector3.Dot(avoidanceDir, desiredDirection) < 0)
            {
                avoidanceDir = -avoidanceDir;
            }

            float forceMultiplier = 1f - (hit.distance / detectionRadius);
            avoidance = avoidanceDir * avoidanceForce * forceMultiplier;
        }

        return avoidance;
    }
}
