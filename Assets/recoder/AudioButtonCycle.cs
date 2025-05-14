using UnityEngine;
using UnityEngine.UI;

public class AudioButtonCycle : MonoBehaviour
{
    public AudioClip[] audioClips;      // Array of audio clips
    public Button playButton;           // Button to start playing the first audio
    public Button nextButton;           // Button to play the next audio
    public Button replayButton;         // Button to replay the current audio
    public Button quitButton;           // Button to quit audio and hide canvas
    public GameObject canvasToHide;     // Canvas or panel to deactivate

    private AudioSource audioSource;    // AudioSource component
    private int currentAudioIndex = 0;  // Index of the currently playing audio

    public static bool hasPlayedAudio = false;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Assign the Play Button functionality
        if (playButton != null)
            playButton.onClick.AddListener(PlayFirstAudio);
        else
            Debug.LogError("Play Button not assigned in the Inspector!");

        // Assign the Next Button functionality
        if (nextButton != null)
            nextButton.onClick.AddListener(PlayNextAudio);
        else
            Debug.LogError("Next Button not assigned in the Inspector!");

        // Assign the Replay Button functionality
        if (replayButton != null)
            replayButton.onClick.AddListener(ReplayAudio);
        else
            Debug.LogError("Replay Button not assigned in the Inspector!");

        // Assign the Quit Button functionality
        if (quitButton != null)
            quitButton.onClick.AddListener(QuitAudioAndHideCanvas);
        else
            Debug.LogError("Quit Button not assigned in the Inspector!");
    }

    void PlayFirstAudio()
    {
        if (audioClips.Length > 0)
        {
            currentAudioIndex = 0;
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();
            hasPlayedAudio = true;
        }
        else
        {
            Debug.LogError("No audio clips assigned in the Inspector!");
        }
    }

    void PlayNextAudio()
    {
        if (audioClips.Length > 0)
        {
            currentAudioIndex = (currentAudioIndex + 1) % audioClips.Length;
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No audio clips assigned in the Inspector!");
        }
    }

    void ReplayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No audio clip assigned to replay!");
        }
    }

    // Stops the audio and hides the canvas
    void QuitAudioAndHideCanvas()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas to hide not assigned in the Inspector!");
        }
    }
}
