using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AudioButtonCycle : MonoBehaviour
{
    public AudioClip[] audioClips;
    public Button playButton;
    public Button nextButton;
    public Button replayButton;
    public Button quitButton;
    public GameObject canvasToHide;
    public GameObject canvasAfterLastAudio;

    private AudioSource audioSource;
    private int currentAudioIndex = 0;
    private bool quitPressed = false;

    public static bool hasPlayedAudio = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (playButton != null)
            playButton.onClick.AddListener(PlayFirstAudio);

        if (nextButton != null)
            nextButton.onClick.AddListener(PlayNextAudio);

        if (replayButton != null)
            replayButton.onClick.AddListener(ReplayAudio);

        if (quitButton != null)
            quitButton.onClick.AddListener(QuitAudioAndHideCanvas);

        if (canvasAfterLastAudio != null)
            canvasAfterLastAudio.SetActive(false);
    }

    void PlayFirstAudio()
    {
        quitPressed = false;

        if (audioClips.Length > 0)
        {
            currentAudioIndex = 0;
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();
            hasPlayedAudio = true;

            if (audioClips.Length == 1)
                StartCoroutine(ShowCanvasAfterAudio());
        }
    }

    void PlayNextAudio()
    {
        if (audioClips.Length == 0)
            return;

        currentAudioIndex++;

        if (currentAudioIndex < audioClips.Length)
        {
            audioSource.clip = audioClips[currentAudioIndex];
            audioSource.Play();
            hasPlayedAudio = true;

            if (currentAudioIndex == audioClips.Length - 1)
                StartCoroutine(ShowCanvasAfterAudio());
        }
    }

    void ReplayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
            audioSource.Play();
            hasPlayedAudio = true;
        }
    }

    void QuitAudioAndHideCanvas()
    {
        quitPressed = true;
        StopAllCoroutines();

        if (audioSource.isPlaying)
            audioSource.Stop();

        if (canvasToHide != null)
            canvasToHide.SetActive(false);
    }

    IEnumerator ShowCanvasAfterAudio()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);

        if (quitPressed) yield break;

        if (canvasToHide != null)
            canvasToHide.SetActive(false);

        if (canvasAfterLastAudio != null)
        {
            canvasAfterLastAudio.SetActive(true);
            yield return new WaitForSeconds(10f);
            Destroy(canvasAfterLastAudio);
        }
    }
}
