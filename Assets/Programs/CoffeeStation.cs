using UnityEngine;
using TMPro;

public class CoffeeStation : MonoBehaviour
{
    [Header("Coffee Status")]
    public bool coffeeReady = false;
    public bool isBrewing = false;
    public float brewTime = 3f;

    [Header("Coffee UI")]
    public GameObject coffeeStatusText;

    [Header("Coffee Mug")]
    public CoffeeItem coffeeMug;

    [Header("Brewing Sound")]
    public AudioSource brewingAudio;

    private float brewTimer = 0f;

    void Awake()
    {
        ResolveCoffeeMug();
    }

    void ResolveCoffeeMug()
    {
        // Cari mug berdasarkan nama pasangan mesin.
        string stationName = gameObject.name;

        if (stationName.StartsWith("CoffeeMachine"))
        {
            string suffix =
                stationName.Substring("CoffeeMachine".Length);

            string expectedMugName = "Mug" + suffix;

            CoffeeItem[] mugs =
                FindObjectsByType<CoffeeItem>(
                    FindObjectsSortMode.None
                );

            foreach (CoffeeItem mug in mugs)
            {
                if (mug != null &&
                    mug.gameObject.name == expectedMugName)
                {
                    coffeeMug = mug;
                    break;
                }
            }
        }

        if (coffeeMug == null)
        {
            Debug.LogError(
                gameObject.name +
                ": Mug tidak ditemukan! Periksa nama MugLeft, MugMid, atau MugRight."
            );
        }
        else
        {
            Debug.Log(
                gameObject.name +
                " terhubung ke " + coffeeMug.gameObject.name
            );
        }
    }

    void Update()
    {
        if (!isBrewing)
            return;

        brewTimer += Time.deltaTime;

        if (brewTimer >= brewTime)
        {
            isBrewing = false;
            brewTimer = 0f;
            coffeeReady = true;

            if (brewingAudio != null)
                brewingAudio.Stop();

            if (coffeeMug != null)
                coffeeMug.SetCoffeeReady();

            ShowStatus("Coffee Ready!");

            Debug.Log(
                gameObject.name +
                " SELESAI BREWING | Mug: " +
                (coffeeMug != null
                    ? coffeeMug.gameObject.name
                    : "NULL")
            );
        }
    }

    public void MakeCoffee()
    {
        ResolveCoffeeMug();

        Debug.Log(
            gameObject.name +
            " DIKLIK | Brewing: " + isBrewing +
            " | Ready: " + coffeeReady +
            " | Mug: " +
            (coffeeMug != null
                ? coffeeMug.gameObject.name
                : "NULL")
        );

        if (isBrewing || coffeeReady)
            return;

        isBrewing = true;
        coffeeReady = false;
        brewTimer = 0f;

        if (coffeeMug != null)
            coffeeMug.SetCoffeeEmpty();

        if (brewingAudio != null)
        {
            brewingAudio.Stop();
            brewingAudio.Play();
        }

        ShowStatus("Making Coffee...");
    }

    public bool TakeCoffee()
    {
        if (!coffeeReady)
            return false;

        coffeeReady = false;
        isBrewing = false;
        brewTimer = 0f;

        if (brewingAudio != null)
            brewingAudio.Stop();

        if (coffeeMug != null)
            coffeeMug.SetCoffeeEmpty();

        HideStatus();

        Debug.Log(gameObject.name + " KOPI DIAMBIL!");

        return true;
    }

    void ShowStatus(string message)
    {
        if (coffeeStatusText == null)
            return;

        coffeeStatusText.SetActive(true);

        TMP_Text text =
            coffeeStatusText.GetComponent<TMP_Text>();

        if (text != null)
            text.text = message;
    }

    void HideStatus()
    {
        if (coffeeStatusText != null)
            coffeeStatusText.SetActive(false);
    }
}