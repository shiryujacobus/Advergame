using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [Header("Timer")]
    public float timeRemaining = 60f;
    public TMP_Text timerText;
    public GameObject timeUpPanel;

    [Header("Score")]
    public ScoreManager scoreManager;
    public TMP_Text scoreText;
    public TMP_Text ordersAcceptedText;

    [Header("Gameplay BGM")]
    public AudioSource gameplayBGM;

    [Header("Countdown SFX")]
    public AudioSource tickAudioSource;
    public AudioSource bellAudioSource;
    public AudioClip tickSFX;
    public AudioClip completeBellSFX;

    [Header("Leaderboard")]
    private string playerName = "Player";
    private bool scoreSaved = false;

    private int lastPlayedSecond = -1;
    private bool timerRunning = true;

    void Start()
    {
        // Ambil nama pemain yang sudah disimpan.
        playerName = PlayerPrefs.GetString("PlayerName", "Player");

        Debug.Log("Nama pemain untuk leaderboard: " + playerName);

        // Pastikan panel Time's Up tersembunyi saat game dimulai.
        if (timeUpPanel != null)
            timeUpPanel.SetActive(false);

        // Pastikan waktu berjalan normal.
        Time.timeScale = 1f;

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
            PlayCountdownSFX();
            TimeUp();
            return;
        }

        UpdateTimerDisplay();
        PlayCountdownSFX();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);

        if (timerText != null)
        {
            timerText.text =
                minutes.ToString("00") + ":" +
                seconds.ToString("00");
        }
    }

    void PlayCountdownSFX()
    {
        int currentSecond = Mathf.CeilToInt(timeRemaining);

        if (currentSecond <= 10 &&
            currentSecond >= 1 &&
            currentSecond != lastPlayedSecond)
        {
            if (tickAudioSource != null && tickSFX != null)
            {
                tickAudioSource.Stop();
                tickAudioSource.PlayOneShot(tickSFX);
            }

            lastPlayedSecond = currentSecond;
        }
    }

    void TimeUp()
    {
        Debug.Log("TIME'S UP!");

        if (!scoreSaved && scoreManager != null)
        {
            LeaderboardManager.SaveScore(
                playerName,
                scoreManager.GetScore()
            );

            scoreSaved = true;
        }

        // Hentikan BGM gameplay.
        if (gameplayBGM != null)
            gameplayBGM.Stop();

        // Hentikan suara countdown.
        if (tickAudioSource != null)
            tickAudioSource.Stop();

        // Putar suara bel ketika waktu habis.
        if (bellAudioSource != null && completeBellSFX != null)
            bellAudioSource.PlayOneShot(completeBellSFX);

        // Tampilkan skor akhir.
        if (scoreManager != null)
        {
            if (scoreText != null)
            {
                scoreText.text =
                    "FINAL SCORE: " + scoreManager.GetScore();
            }

            if (ordersAcceptedText != null)
            {
                ordersAcceptedText.text =
                    "ORDERS ACCEPTED: " +
                    scoreManager.GetOrdersAccepted();
            }
        }

        // Tampilkan panel Time's Up.
        if (timeUpPanel != null)
            timeUpPanel.SetActive(true);

        // Jeda gameplay.
        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        // Pulihkan waktu sebelum memuat ulang scene.
        Time.timeScale = 1f;

        SceneManager.LoadScene("CafeGame");
    }

    public void GoHome()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("CodeScene");
    }
}