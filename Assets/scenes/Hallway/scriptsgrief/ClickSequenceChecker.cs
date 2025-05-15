using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSequenceManager : MonoBehaviour
{
    public List<GameObject> correctOrder;
    public GameObject successCanvas;
    public GameObject doorToDestroy;
    public GameObject cameraToDisable;
    public GameObject exitbutton;
    public AudioClip doorDestroySound; // ?? Assign the sound clip here

    private List<GameObject> clickedObjects = new List<GameObject>();
    private bool sequenceComplete = false;
    private AudioSource audioSource;

    void Start()
    {
        // Setup audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void RegisterClick(GameObject key)
    {
        if (sequenceComplete) return;

        clickedObjects.Add(key);

        if (!IsCorrectSoFar())
        {
            clickedObjects.Clear(); // reset if wrong
            Debug.Log("Wrong sequence. Resetting...");
            return;
        }

        if (clickedObjects.Count == correctOrder.Count)
        {
            sequenceComplete = true;
            StartCoroutine(HandleSuccess());
        }
    }

    private bool IsCorrectSoFar()
    {
        for (int i = 0; i < clickedObjects.Count; i++)
        {
            if (clickedObjects[i] != correctOrder[i])
                return false;
        }
        return true;
    }

    private IEnumerator HandleSuccess()
    {
        successCanvas.SetActive(true);
        exitbutton.SetActive(false);

        if (cameraToDisable != null)
            cameraToDisable.SetActive(false);


        if (doorDestroySound != null)
            audioSource.PlayOneShot(doorDestroySound);

        Destroy(doorToDestroy);

        yield return new WaitForSeconds(6f);
        successCanvas.SetActive(false);
    }
}
