using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depressionPass : MonoBehaviour
{
    public MeshRenderer targetMesh;
    public GameObject canvas1ToActivate;
    public GameObject canvas2ToActivate;
    public GameObject doorToDeactivate;
    public AudioSource audioSource;
    public AudioClip completionSound;

    private bool triggered = false;

    void Start()
    {
        if (canvas1ToActivate != null)
            canvas1ToActivate.SetActive(false);

        if (canvas2ToActivate != null)
            canvas2ToActivate.SetActive(false);
    }

    void Update()
    {
        if (!triggered && targetMesh != null && targetMesh.enabled)
        {
            TriggerEvent();
            triggered = true;
        }
    }

    void TriggerEvent()
    {
        if (canvas1ToActivate != null)
            canvas1ToActivate.SetActive(true);

        if (canvas2ToActivate != null)
        {
            canvas2ToActivate.SetActive(true);
            StartCoroutine(DestroyCanvas2AfterDelay());
        }

        if (doorToDeactivate != null)
            doorToDeactivate.SetActive(false);

        if (audioSource != null && completionSound != null)
            audioSource.PlayOneShot(completionSound);
    }

    IEnumerator DestroyCanvas2AfterDelay()
    {
        yield return new WaitForSeconds(6f);
        Destroy(canvas2ToActivate);
    }
}
