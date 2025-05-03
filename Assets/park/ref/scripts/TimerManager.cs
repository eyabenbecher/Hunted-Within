using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerManager : MonoBehaviour
{
    public float totalTime = 180f; // 3 minutes
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI loseMessageText;

    private float timeLeft;
    private bool timerRunning = true;

    void Start()
    {
        timeLeft = totalTime;

        if (loseMessageText != null)
            loseMessageText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!timerRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            timerRunning = false;
            HandleTimeOut();
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60f);
        int seconds = Mathf.FloorToInt(timeLeft % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void HandleTimeOut()
    {
        if (loseMessageText != null)
        {
            loseMessageText.text = "You Lost!";
            loseMessageText.gameObject.SetActive(true);
        }

        Invoke(nameof(RestartScene), 2f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
