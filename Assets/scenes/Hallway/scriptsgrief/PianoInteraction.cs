using UnityEngine;
using UnityEngine.UI;

public class PianoInteraction : MonoBehaviour
{
    public GameObject playPianoButton;
    public GameObject exitPianoButton;
    public Camera pianoCamera;         
    public Transform player;           
    public float activationDistance = 3f;

    private bool isPlayerInRange = false;

    void Start()
    {
        playPianoButton.SetActive(false);
        pianoCamera.gameObject.SetActive(false);

        playPianoButton.GetComponent<Button>().onClick.AddListener(ActivatePianoCamera);
        exitPianoButton.GetComponent<Button>().onClick.AddListener(DiactivatePianoCamera);
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= activationDistance)
        {
            if (!isPlayerInRange)
            {
                playPianoButton.SetActive(true);
                isPlayerInRange = true;
            }
        }
        else
        {
            if (isPlayerInRange)
            {
                playPianoButton.SetActive(false);
                isPlayerInRange = false;
            }
        }
    }

    void ActivatePianoCamera()
    {
        pianoCamera.gameObject.SetActive(true);

        playPianoButton.SetActive(false); 

        exitPianoButton.SetActive(true );
    }
    void DiactivatePianoCamera()
    {
        pianoCamera.gameObject.SetActive(false);

        exitPianoButton.SetActive(false);
    }
}
