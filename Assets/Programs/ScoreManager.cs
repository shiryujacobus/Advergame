using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int totalScore = 0;
    public int ordersAccepted = 0;

    public void AddScore(int amount)
    {
        totalScore += amount;
        ordersAccepted++;

        Debug.Log("SCORE SEKARANG: " + totalScore);
        Debug.Log("ORDERS ACCEPTED: " + ordersAccepted);
    }

    public int GetScore()
    {
        return totalScore;
    }

    public int GetOrdersAccepted()
    {
        return ordersAccepted;
    }
}
