using UnityEngine;

public class VanishOnApproach : MonoBehaviour
{
    public Transform player;
    public GameObject disappearingObject;
    public GameObject objectToCalculateDistance;
    public float vanishDistance = 10f;
    public float reappearDistance = 10f;
    public AudioSource audioSource;
    public AudioClip vanishSound;

    private bool isCurrentlyVisible = true;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        if (disappearingObject == null)
            disappearingObject = gameObject;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
       
        float distance = Vector3.Distance(player.position, objectToCalculateDistance.transform.position);
        if (isCurrentlyVisible && distance <= vanishDistance)
        {
            if (audioSource != null && vanishSound != null)
                audioSource.PlayOneShot(vanishSound);

            disappearingObject.SetActive(false);
            isCurrentlyVisible = false;
        }
        else if (!isCurrentlyVisible && distance >= reappearDistance)
        {
            disappearingObject.SetActive(true);
            isCurrentlyVisible = true;
        }
    }
}