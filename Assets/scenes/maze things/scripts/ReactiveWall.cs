using UnityEngine;

public class ReactiveWall : MonoBehaviour
{
    [Header("Raycast Settings")]
    public Vector2 wallSize = new Vector2(2f, 2f);     // Width x Height of the wall
    public int raysPerAxis = 5;                        // Number of rays across width & height
    public float rayDistance = 10f;                    // How far each ray reaches
    public LayerMask playerLayer;                      // Layer to detect the player

    [Header("Movement Settings")]
    public float moveDistance = 3f;                    // How far the wall moves
    public float moveSpeed = 1f;                       // How fast it moves

    private bool isMoving = false;
    private bool alreadyTriggered = false;
    private Vector3 targetPosition;

    void Update()
    {
        if (!isMoving && !alreadyTriggered && DetectPlayer())
        {
            MoveInRandomDirection();
            alreadyTriggered = true;
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                isMoving = false;
            }
        }
    }

    bool DetectPlayer()
    {
        Vector3 origin = transform.position;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        for (int x = 0; x < raysPerAxis; x++)
        {
            for (int y = 0; y < raysPerAxis; y++)
            {
                float offsetX = ((float)x / (raysPerAxis - 1)) * wallSize.x - (wallSize.x / 2f); // centered X
                float offsetY = ((float)y / (raysPerAxis - 1)) * wallSize.y; // only upward Y
                Vector3 rayStart = origin + right * offsetX + up * offsetY;

                RaycastHit hitF, hitB;
                bool forwardHit = Physics.Raycast(rayStart, transform.forward, out hitF, rayDistance, playerLayer);
                bool backwardHit = Physics.Raycast(rayStart, -transform.forward, out hitB, rayDistance, playerLayer);

                if ((forwardHit && hitF.collider.CompareTag("Player")) ||
                    (backwardHit && hitB.collider.CompareTag("Player")))
                {
                    return true;
                }
            }
        }

        return false;
    }

    void MoveInRandomDirection()
    {
        Vector3[] directions = new Vector3[]
        {
        Vector3.up,
        Vector3.down,
        Vector3.forward,
        Vector3.back
        };

        Vector3 chosenDir = directions[Random.Range(0, directions.Length)];
        targetPosition = transform.position + chosenDir * moveDistance;
        isMoving = true;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 origin = transform.position;
        Vector3 right = transform.right;
        Vector3 up = transform.up;

        for (int x = 0; x < raysPerAxis; x++)
        {
            for (int y = 0; y < raysPerAxis; y++)
            {
                float offsetX = ((float)x / (raysPerAxis - 1)) * wallSize.x - (wallSize.x / 2f);
                float offsetY = ((float)y / (raysPerAxis - 1)) * wallSize.y;
                Vector3 rayStart = origin + right * offsetX + up * offsetY;

                Gizmos.DrawRay(rayStart, transform.forward * rayDistance);
                Gizmos.DrawRay(rayStart, -transform.forward * rayDistance);
            }
        }
    }
}

