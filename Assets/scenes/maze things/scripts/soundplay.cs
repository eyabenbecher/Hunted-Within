using UnityEngine;

public class BoxCollisionSound : MonoBehaviour
{
    public AudioSource collisionSound; // Assign this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Checks if Lia collides
        {
            if (collisionSound != null && !collisionSound.isPlaying)
            {
                collisionSound.Play();
            }
        }
    }
}
