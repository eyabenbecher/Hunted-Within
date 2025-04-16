using UnityEngine;

public class HorrorFlickerLight : MonoBehaviour
{
    public Light flickerLight; // Assign your spotlight here
    public float minInterval = 0.05f; // Minimum time between flickers
    public float maxInterval = 0.5f;  // Maximum time between flickers

    private void Start()
    {
        if (flickerLight == null)
            flickerLight = GetComponent<Light>();

        StartCoroutine(Flicker());
    }

    System.Collections.IEnumerator Flicker()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            flickerLight.enabled = !flickerLight.enabled;
            yield return new WaitForSeconds(waitTime);
        }
    }
}
