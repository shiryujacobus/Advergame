
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private CoffeeStation left;
    private CoffeeStation mid;
    private CoffeeStation right;

    private void Awake()
    {
        CoffeeStation[] stations =
            FindObjectsByType<CoffeeStation>(FindObjectsSortMode.None);

        foreach (CoffeeStation station in stations)
        {
            switch (station.gameObject.name)
            {
                case "CoffeeMachineLeft":
                    left = station;
                    break;

                case "CoffeeMachineMid":
                    mid = station;
                    break;

                case "CoffeeMachineRight":
                    right = station;
                    break;
            }
        }

        Debug.Log(
            $"Coffee machines found: Left={left != null}, " +
            $"Mid={mid != null}, Right={right != null}"
        );
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StartCoffee(left);

        if (Input.GetKeyDown(KeyCode.W))
            StartCoffee(mid);

        if (Input.GetKeyDown(KeyCode.E))
            StartCoffee(right);
    }

    private void StartCoffee(CoffeeStation station)
    {
        if (station == null)
        {
            Debug.LogError("Mesin kopi tidak ditemukan! Periksa nama GameObject.");
            return;
        }

        station.MakeCoffee();
    }

    public bool TryTakeCoffee(CoffeeItem mug)
    {
        if (mug == null || !mug.coffeeReady)
            return false;

        CoffeeStation station = FindStationForMug(mug);

        if (station == null || !station.coffeeReady)
            return false;

        if (FoodBagSystem.Instance == null)
        {
            Debug.LogError("FoodBagSystem belum ada di scene!");
            return false;
        }

        FoodBagSystem.Instance.AddFood("Coffee");
        return station.TakeCoffee();
    }

    private CoffeeStation FindStationForMug(CoffeeItem mug)
    {
        CoffeeStation[] stations =
            FindObjectsByType<CoffeeStation>(FindObjectsSortMode.None);

        foreach (CoffeeStation station in stations)
        {
            if (station.coffeeMugs == null)
                continue;

            foreach (CoffeeItem assignedMug in station.coffeeMugs)
            {
                if (assignedMug == mug)
                    return station;
            }
        }

        return null;
    }
}
