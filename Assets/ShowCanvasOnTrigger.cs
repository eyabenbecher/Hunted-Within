using UnityEngine;

public class ShowCanvasOnTrigger : MonoBehaviour
{
    public GameObject canvasToShow;
    public float showDuration = 6f;

    private bool hasBeenTriggered = false;

    void Start()
    {
        if (canvasToShow != null)
            canvasToShow.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasBeenTriggered && other.CompareTag("Player"))
        {
            hasBeenTriggered = true;

            if (canvasToShow != null)
            {
                canvasToShow.SetActive(true);
                StartCoroutine(HideAndDestroyCanvasAfterDelay());
            }
        }
    }

    private System.Collections.IEnumerator HideAndDestroyCanvasAfterDelay()
    {
        yield return new WaitForSeconds(showDuration);

        if (canvasToShow != null)
        {
            Destroy(canvasToShow);
        }
    }
}
