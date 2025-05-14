using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndSceneLoader : MonoBehaviour
{
    public VideoPlayer videoPlayer;     
    public string sceneToLoad;          

    void Start()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponent<VideoPlayer>();
        }

        if (videoPlayer != null)
        {
            // Subscribe to the loopPointReached event
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogError("VideoPlayer not assigned or found!");
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        
        SceneManager.LoadScene(sceneToLoad);
    }
}
