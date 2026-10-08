using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int totalScore = 0;

    public void AddScore(int amount)
    {
        totalScore += amount;

        Debug.Log("SCORE SEKARANG: " + totalScore);
    }

    public int GetScore()
    {
        return totalScore;
    }
}