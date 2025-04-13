using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Transform[] monsters;
    public Slider anxietyBar;
    public Slider staminaBar;

    public float maxAnxietyDistance = 10f;
    public float caughtDistance = 1f;
    public float decaySpeed = 0.3f;
    public AudioSource heartbeatAudio;
    public float maxHeartbeatVolume = 1f;
    public float maxHeartbeatPitch = 1.5f;

    public float staminaDecreaseSpeed = 0.3f;
    public float staminaRecoverSpeed = 0.2f;
    public float runningSpeedThreshold = 5f;

    public LayerMask obstacleMask;

    private float currentAnxiety = 0f;
    private float currentStamina = 1f; // Starts full

    private CharacterController controller; // Or use Rigidbody if that's how your player moves

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (monsters == null || monsters.Length == 0 || anxietyBar == null || heartbeatAudio == null || staminaBar == null)
            return;

        Vector3 playerPos = transform.position + Vector3.up * 1.5f;
        float visibleClosestDistance = Mathf.Infinity;

        foreach (Transform monster in monsters)
        {
            Vector3 monsterEye = monster.position + Vector3.up * 1.5f;
            Vector3 dirToPlayer = (playerPos - monsterEye).normalized;
            float distanceToPlayer = Vector3.Distance(monsterEye, playerPos);

            if (!Physics.Raycast(monsterEye, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                if (distanceToPlayer < visibleClosestDistance)
                {
                    visibleClosestDistance = distanceToPlayer;
                }
            }
        }

        if (visibleClosestDistance != Mathf.Infinity)
        {
            float targetAnxiety = Mathf.Clamp01(1 - ((visibleClosestDistance - caughtDistance) / (maxAnxietyDistance - caughtDistance)));
            currentAnxiety = Mathf.MoveTowards(currentAnxiety, targetAnxiety, Time.deltaTime * 3f);
        }
        else
        {
            currentAnxiety = Mathf.MoveTowards(currentAnxiety, 0f, Time.deltaTime * decaySpeed);
        }

        anxietyBar.value = currentAnxiety;
        heartbeatAudio.volume = Mathf.Lerp(0f, maxHeartbeatVolume, currentAnxiety);
        heartbeatAudio.pitch = Mathf.Lerp(1f, maxHeartbeatPitch, currentAnxiety);

        // --- Stamina Management ---
        float speed = controller != null ? controller.velocity.magnitude : 0f;

        if (speed > runningSpeedThreshold)
        {
            currentStamina = Mathf.MoveTowards(currentStamina, 0f, Time.deltaTime * staminaDecreaseSpeed);
        }
        else
        {
            currentStamina = Mathf.MoveTowards(currentStamina, 1f, Time.deltaTime * staminaRecoverSpeed);
        }

        staminaBar.value = currentStamina;
    }
}
