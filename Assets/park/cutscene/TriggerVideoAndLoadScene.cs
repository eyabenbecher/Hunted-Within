using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class TriggerVideoAndLoadScene : MonoBehaviour
{
    public GameObject videoPlayerObject; // Object that has the VideoPlayer component (e.g., canvas with video)
    private VideoPlayer videoPlayer;
    private bool hasTriggered = false;

    void Start()
    {
        if (videoPlayerObject != null)
        {
            videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();
            videoPlayerObject.SetActive(false); // Hide at start
        }
        else
        {
            Debug.LogError("Video Player Object is not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player")) // Make sure Lia has the tag "Player"
        {
            hasTriggered = true;

            if (videoPlayer != null)
            {
                videoPlayerObject.SetActive(true);
                videoPlayer.Play();
                videoPlayer.loopPointReached += OnVideoEnd;
            }
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
