using UnityEngine;

public class VoiceBoxInteraction : MonoBehaviour
{
    public GameObject canvas;           
    public KeyCode escapeKey = KeyCode.E; 

    void Start()
    {
        // Ensure the canvas is initially hidden
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas not assigned in the Inspector!");
        }
    }

    // This method is called when the voice box GameObject is clicked
    void OnMouseDown()
    {
        // Show the canvas only if it's not already active
        if (canvas != null && !canvas.activeSelf)
        {
            canvas.SetActive(true);
        }
    }


}
