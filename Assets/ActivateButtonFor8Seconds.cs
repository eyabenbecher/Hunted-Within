using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnableButtonAfter8Seconds : MonoBehaviour
{
    public Button targetButton;

    void Start()
    {
        if (targetButton != null)
        {
            targetButton.gameObject.SetActive(false); // Set inactive at start
            StartCoroutine(EnableAfterDelay());
        }
        else
        {
            Debug.LogWarning("No button assigned to EnableButtonAfter8Seconds script.");
        }
    }

    IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(8f);
        targetButton.gameObject.SetActive(true); // Enable after 8 seconds
    }
}
