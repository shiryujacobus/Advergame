
using UnityEngine;

public class ShelfFoodClick : MonoBehaviour
{
    [Header("Nama item harus sama dengan nama di pesanan")]
    public string itemName = "Coffee";

    private void OnMouseDown()
    {
        if (FoodBagSystem.Instance == null)
            return;

        FoodBagSystem.Instance.AddFood(itemName);
    }
}
