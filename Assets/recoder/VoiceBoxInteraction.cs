using UnityEngine;

public class VoiceBoxInteraction : MonoBehaviour
{
    public GameObject canvas;           // The Canvas to be shown/hidden
    public KeyCode escapeKey = KeyCode.Escape; // The key to close the canvas (Esc by default)

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

    // Update is called once per frame
    void Update()
    {
        // Check if the escape key is pressed to hide the canvas
        if (Input.GetKeyDown(escapeKey))
        {
            if (canvas != null && canvas.activeSelf)
            {
                canvas.SetActive(false);  // Hide the canvas
            }
        }
    }
}
