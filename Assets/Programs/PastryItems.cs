using UnityEngine;

public class PastryItems : MonoBehaviour
{
    public string pastryName;

    private void OnMouseDown()
    {
        if (FoodBagSystem.Instance == null)
            return;

        FoodBagSystem.Instance.AddFood(pastryName);
    }
}