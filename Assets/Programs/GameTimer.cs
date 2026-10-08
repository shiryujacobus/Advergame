using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60f;

    public TMP_Text timerText;
    public GameObject timeUpPanel;

    public ScoreManager scoreManager;
    public TMP_Text scoreText;

    private bool timerRunning = true;

    void Start()
    {
        if (timeUpPanel != null)
            timeUpPanel.SetActive(false);

        UpdateTimerDisplay();
    }

    void Update()
    {
        if (!timerRunning)
            return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            timerRunning = false;

            UpdateTimerDisplay();
            TimeUp();

            return;
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes =
            Mathf.FloorToInt(timeRemaining / 60f);

        int seconds =
            Mathf.FloorToInt(timeRemaining % 60f);

        if (timerText != null)
        {
            timerText.text =
                minutes.ToString("00") + ":" +
                seconds.ToString("00");
        }
    }

    void TimeUp()
    {
        Debug.Log("TIME'S UP!");

        if (scoreText != null && scoreManager != null)
        {
            scoreText.text =
                "FINAL SCORE: " +
                scoreManager.GetScore();
        }

        if (timeUpPanel != null)
            timeUpPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("CodeScene");
    }
}