using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueCanvasManager : MonoBehaviour
{
    public GameObject[] dialogueCanvases;     // All dialogue steps
    public GameObject[] griefCanvases;        // One grief canvas per question
    public GameObject winUI;                  // ✅ Win screen UI

    // Audio
    public AudioSource audioSource;
    public AudioClip[] questionClips;         // One per dialogue
    public AudioClip[] answerClipsTrue;       // True answers
    public AudioClip[] answerClipsFalse;      // False answers
    public AudioClip[] griefClips;            // One per grief screen
    public AudioClip winClip;                 // ✅ Win UI voice line

    private int currentCanvasIndex = 0;

    void Start()
    {
        foreach (GameObject canvas in dialogueCanvases)
            canvas.SetActive(false);

        foreach (GameObject grief in griefCanvases)
            grief.SetActive(false);

        if (winUI != null)
            winUI.SetActive(false);
    }

    public void StartConversation()
    {
        StartCoroutine(ShowNextCanvasAfterDelay(7f)); // Delay before first question
    }

    IEnumerator ShowNextCanvasAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (currentCanvasIndex < dialogueCanvases.Length)
        {
            dialogueCanvases[currentCanvasIndex].SetActive(true);

            // Play question voice line
            if (audioSource != null && questionClips.Length > currentCanvasIndex && questionClips[currentCanvasIndex] != null)
            {
                audioSource.clip = questionClips[currentCanvasIndex];
                audioSource.Play();
            }
        }
    }

    public void ChooseAnswer(bool isCorrect)
    {
        dialogueCanvases[currentCanvasIndex].SetActive(false);

        // Play answer voice
        if (audioSource != null)
        {
            AudioClip answerClip = null;

            if (isCorrect && answerClipsTrue.Length > currentCanvasIndex)
                answerClip = answerClipsTrue[currentCanvasIndex];
            else if (!isCorrect && answerClipsFalse.Length > currentCanvasIndex)
                answerClip = answerClipsFalse[currentCanvasIndex];

            if (answerClip != null)
            {
                audioSource.clip = answerClip;
                audioSource.Play();
            }
        }

        if (isCorrect)
        {
            currentCanvasIndex++;

            if (currentCanvasIndex < dialogueCanvases.Length)
            {
                StartCoroutine(ShowNextCanvasAfterDelay(7f));
            }
            else
            {
                // ✅ All questions answered correctly: Show Win UI after 7s
                StartCoroutine(ShowWinUIAfterDelay(7f));
            }
        }
        else
        {
            StartCoroutine(ShowGriefCanvasAfterDelay(7f, currentCanvasIndex));
        }
    }

    IEnumerator ShowGriefCanvasAfterDelay(float delay, int griefIndex)
    {
        yield return new WaitForSeconds(delay);

        if (griefIndex < griefCanvases.Length && griefCanvases[griefIndex] != null)
        {
            griefCanvases[griefIndex].SetActive(true);

            // Play grief voice line
            if (audioSource != null && griefClips.Length > griefIndex && griefClips[griefIndex] != null)
            {
                audioSource.clip = griefClips[griefIndex];
                audioSource.Play();
            }
        }
    }

    IEnumerator ShowWinUIAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (winUI != null)
        {
            winUI.SetActive(true);

            // ✅ Play win voice line
            if (audioSource != null && winClip != null)
            {
                audioSource.clip = winClip;
                audioSource.Play();
            }
        }
    }

    public void RetryScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
