using UnityEngine;

public class animwall : MonoBehaviour
{
    public Animator[] wallAnimators;   // Assign multiple Animators in the Inspector
    public string[] animationTriggers; // Assign different animation trigger names
    public AudioSource soundEffect;    // Assign the AudioSource component

    private void Start()
    {
        // Optional: Ensure sound does not play on awake
        if (soundEffect != null)
        {
            soundEffect.Stop();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the Player entering the trigger
        if (other.CompareTag("Player"))
        {
            // Play the sound only on collision
            if (soundEffect != null)
            {
                soundEffect.Play();
            }

            // Loop through and trigger animations for all wallAnimators
            for (int i = 0; i < wallAnimators.Length; i++)
            {
                if (wallAnimators[i] != null)
                {
                    wallAnimators[i].gameObject.SetActive(true);  // Activate the wall (make it visible)
                    wallAnimators[i].enabled = true;  // Enable the Animator
                    wallAnimators[i].SetTrigger(animationTriggers[i]);  // Trigger the animation
                }
            }
        }
    }
}
