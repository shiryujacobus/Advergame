using UnityEngine;

public class LeaderboardReset : MonoBehaviour
{
    [ContextMenu("Reset Leaderboard")]
    private void ResetLeaderboard()
    {
        LeaderboardManager.ResetLeaderboard();
    }
}