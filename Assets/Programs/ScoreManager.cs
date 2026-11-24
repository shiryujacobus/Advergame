
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int totalScore = 0;
    public int ordersAccepted = 0;
    public int totalItemsSold = 0;

    public int pointPerItem = 25;

    public void AddScore(int totalItems)
    {
        int earnedScore = totalItems * pointPerItem;

        totalScore += earnedScore;
        ordersAccepted++;
        totalItemsSold += totalItems;

        Debug.Log("SCORE DIDAPAT: +" + earnedScore);
        Debug.Log("TOTAL SCORE: " + totalScore);
        Debug.Log("ORDERS ACCEPTED: " + ordersAccepted);
        Debug.Log("TOTAL ITEMS SOLD: " + totalItemsSold);
    }

    public int GetScore()
    {
        return totalScore;
    }

    public int GetOrdersAccepted()
    {
        return ordersAccepted;
    }

    public int GetTotalItemsSold()
    {
        return totalItemsSold;
    }
}
