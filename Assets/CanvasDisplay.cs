using UnityEngine;

public class CanvasDisplay : MonoBehaviour
{
    public GameObject canvasToDisplay; 
    public float displayTime = 10f;

    void Start()
    {
        if (canvasToDisplay != null)
        {
            canvasToDisplay.SetActive(true);
            Invoke("DestroyCanvas", displayTime);
        }
        else
        {
            Debug.LogWarning("Canvas not assigned to CanvasDisplay script.");
        }
    }

    void DestroyCanvas()
    {
        Destroy(canvasToDisplay);
    }
}
