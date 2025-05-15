using System.Collections;
using UnityEngine;

public class PianoKey : MonoBehaviour
{
    public AudioClip note;
    private AudioSource audioSource;
    private float rotationAmount = 3f;
    private float rotationDuration = 0.1f;
    public ClickSequenceManager sequenceManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void OnMouseDown()
    {
        if (sequenceManager != null)
            sequenceManager.RegisterClick(gameObject);

        StartCoroutine(RotateKey());
    }

    IEnumerator RotateKey()
    {
        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float rotX = Mathf.Lerp(0, rotationAmount, elapsed / rotationDuration);
            transform.localEulerAngles = new Vector3(rotX, transform.localEulerAngles.y, transform.localEulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            float rotX = Mathf.Lerp(rotationAmount, 0, elapsed / rotationDuration);
            transform.localEulerAngles = new Vector3(rotX, transform.localEulerAngles.y, transform.localEulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);

        if (note != null)
            audioSource.PlayOneShot(note);
    }
}
