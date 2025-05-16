using System.Collections;
using UnityEngine;

public class DialogueCanvasManager : MonoBehaviour
{
    public Canvas[] dialogueCanvases; // Assign all canvases in inspector

    private int currentCanvasIndex = -1;

    void Start()
    {
        // Disable all canvases initially
        foreach(var canvas in dialogueCanvases)
            canvas.gameObject.SetActive(false);
    }

    public void StartDialogueAfterDelay(float delaySeconds)
    {
        StartCoroutine(StartFirstCanvasAfterDelay(delaySeconds));
    }

    private IEnumerator StartFirstCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowCanvas(0);  // Show first canvas after delay
    }

    void ShowCanvas(int index)
    {
        if(currentCanvasIndex >= 0)
            dialogueCanvases[currentCanvasIndex].gameObject.SetActive(false);

        dialogueCanvases[index].gameObject.SetActive(true);
        currentCanvasIndex = index;
    }

    // Call this method from UI buttons OnClick()
    public void OnChoiceSelected(int canvasIndexToShow)
    {
        ShowCanvas(canvasIndexToShow);
    }
}
