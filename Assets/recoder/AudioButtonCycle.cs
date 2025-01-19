using UnityEngine;
using UnityEngine.UI;

public class AudioButtonCycle : MonoBehaviour
{
    public AudioClip[] audioClips;  // Array of audio clips
    public Button playButton;      // Button to start playing the first audio
    public Button nextButton;      // Button to play the next audio
    public Button replayButton;    // Button to replay the current audio
    private AudioSource audioSource; // AudioSource component
    private int currentAudioIndex = 0; // Index of the currently playing audio

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Assign the Play Button functionality
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayFirstAudio);
        }
        else
        {
            Debug.LogError("Play Button not assigned in the Inspector!");
        }

        // Assign the Next Button functionality
        if (nextButton != null)
        {
            nextButton.onClick.AddListener(PlayNextAudio);
        }
        else
        {
            Debug.LogError("Next Button not assigned in the Inspector!");
        }

        // Assign the Replay Button functionality
        if (replayButton != null)
        {
            replayButton.onClick.AddListener(ReplayAudio);
        }
        else
        {
            Debug.LogError("Replay Button not assigned in the Inspector!");
        }
    }

    // Play the first audio clip
    void PlayFirstAudio()
    {
        if (audioClips.Length > 0)
        {
            currentAudioIndex = 0; // Reset to the first audio clip
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No audio clips assigned in the Inspector!");
        }
    }

    // Play the next audio clip in the array
    void PlayNextAudio()
    {
        if (audioClips.Length > 0)
        {
            currentAudioIndex = (currentAudioIndex + 1) % audioClips.Length; // Cycle through the clips
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No audio clips assigned in the Inspector!");
        }
    }

    // Replay the currently playing or last played audio
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
}
