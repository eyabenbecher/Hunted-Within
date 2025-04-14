using StarterAssets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    public float staminaDecreaseSpeed = 0.1f;
    public float staminaRecoverSpeed = 0.01f;
    public float runningSpeedThreshold = 5f;

    public LayerMask obstacleMask;

    private float currentAnxiety = 0f;
    private float currentStamina = 1f;

    private CharacterController controller;
    private ThirdPersonController movementController;

    public float minSprintSpeed = 0f;
    private float baseSprintSpeed;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        movementController = GetComponent<ThirdPersonController>();

        if (movementController != null)
        {
            baseSprintSpeed = movementController.SprintSpeed;
        }
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
            currentAnxiety = Mathf.MoveTowards(currentAnxiety, targetAnxiety, Time.deltaTime * 1f);
        }
        else
        {
            currentAnxiety = Mathf.MoveTowards(currentAnxiety, 0f, Time.deltaTime * decaySpeed);
        }

        anxietyBar.value = currentAnxiety;
        heartbeatAudio.volume = Mathf.Lerp(0f, maxHeartbeatVolume, currentAnxiety);
        heartbeatAudio.pitch = Mathf.Lerp(1f, maxHeartbeatPitch, currentAnxiety);

        float speed = controller != null ? controller.velocity.magnitude : 0f;

        bool isMoving = movementController != null && movementController.GetComponent<StarterAssetsInputs>().move != Vector2.zero;
        bool isTryingToSprint = movementController != null && movementController.GetComponent<StarterAssetsInputs>().sprint;
        bool isSprinting = isMoving && isTryingToSprint && speed > movementController.MoveSpeed + 0.1f;

        if (isSprinting)
        {
            currentStamina = Mathf.MoveTowards(currentStamina, 0f, Time.deltaTime * staminaDecreaseSpeed);
        }
        else if (speed < 0.1f)
        {
            currentStamina = Mathf.MoveTowards(currentStamina, 1f, Time.deltaTime * staminaRecoverSpeed);
        }

        staminaBar.value = currentStamina;

        if (movementController != null)
        {
            float staminaFactor = Mathf.Lerp(minSprintSpeed, baseSprintSpeed, currentStamina);
            movementController.SprintSpeed = staminaFactor;
        }

        if (currentAnxiety >= 1f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
