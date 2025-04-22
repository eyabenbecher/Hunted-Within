using UnityEngine;

public class SimpleRideRandomController : MonoBehaviour
{
    public Light rideLight;
    public Animator rideAnimator;
    public string animationName = "RideAnimation";

    public AudioSource lightSound;
    public AudioSource rideSound;

    public float animationDelay = 2f;       // Time between light on and animation
    public float animationDuration = 8f;    // Animation length
    public float minWaitTime = 10f;         // Min time between activations
    public float maxWaitTime = 20f;         // Max time between activations
    public float firstDelay = 10f;          // Initial delay at game start

    void Start()
    {
        // Make sure everything starts OFF
        if (rideLight != null) rideLight.enabled = false;
        if (rideSound != null) rideSound.Stop();
        if (lightSound != null) lightSound.Stop();

        StartCoroutine(RandomRideRoutine());
    }

    System.Collections.IEnumerator RandomRideRoutine()
    {
        // Wait before first activation
        yield return new WaitForSeconds(firstDelay);

        while (true)
        {
            // Light ON + sound
            if (rideLight != null) rideLight.enabled = true;
            if (lightSound != null) lightSound.Play();

            // Wait before animation
            yield return new WaitForSeconds(animationDelay);

            // Start animation
            if (rideAnimator != null)
                rideAnimator.Play(animationName, 0, 0f);

            if (rideSound != null) rideSound.Play();

            // Wait while animation plays
            yield return new WaitForSeconds(animationDuration);

            // Turn light OFF and stop ride sound
            if (rideLight != null) rideLight.enabled = false;
            if (rideSound != null) rideSound.Stop();

            // Wait random time before next cycle
            float randomWait = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(randomWait);
        }
    }
}
