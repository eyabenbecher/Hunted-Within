using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public GameObject videoPlayerObject; // The GameObject that includes the VideoPlayer (and canvas if any)

    private VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayerObject != null)
        {
            videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();

            if (videoPlayer != null)
            {
                videoPlayerObject.SetActive(true); // Show video player
                videoPlayer.Play(); // Start the video

                videoPlayer.loopPointReached += OnVideoEnd; // Subscribe to event when video ends
            }
            else
            {
                Debug.LogError("No VideoPlayer component found on the assigned GameObject.");
            }
        }
        else
        {
            Debug.LogError("VideoPlayerObject is not assigned.");
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoPlayerObject.SetActive(false); // Hide the video player after video ends
    }
}
