using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLeaderboard : MonoBehaviour
{
    public void OpenLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
