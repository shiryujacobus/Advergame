using UnityEngine;
using TMPro;

public class CoffeeStation : MonoBehaviour
{
    public bool coffeeReady = false;
    public bool isBrewing = false;

    public float brewTime = 3f;

    public GameObject coffeeStatusText;

    [Header("Coffee Mug")]
    public CoffeeItem coffeeMug;

    [Header("Brewing Sound")]
    public AudioSource brewingAudio;

    private float brewTimer = 0f;

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
            else
                Debug.LogWarning(
                    "Coffee Mug belum di-assign di CoffeeStation!"
                );

            ShowStatus("Coffee Ready!");

            Debug.Log("COFFEE SUDAH READY!");
            Debug.Log(
                "STATUS SETELAH BREWING | Brewing: "
                + isBrewing
                + " | Ready: "
                + coffeeReady
            );
        }
    }

    public void MakeCoffee()
    {
        Debug.Log(
            "MAKE COFFEE DIPANGGIL | Brewing: "
            + isBrewing
            + " | Ready: "
            + coffeeReady
        );

        if (isBrewing)
        {
            Debug.Log("COFFEE MASIH SEDANG DIBUAT!");
            return;
        }

        if (coffeeReady)
        {
            Debug.Log("COFFEE MASIH READY!");
            return;
        }

        isBrewing = true;
        coffeeReady = false;
        brewTimer = 0f;

        if (coffeeMug != null)
        {
            coffeeMug.SetCoffeeEmpty();
        }
        else
        {
            Debug.LogWarning(
                "Coffee Mug belum di-assign di CoffeeStation!"
            );
        }

        if (brewingAudio != null)
        {
            brewingAudio.Stop();
            brewingAudio.Play();
        }

        ShowStatus("Making Coffee...");

        Debug.Log("COFFEE SEDANG DIBUAT...");
        Debug.Log(
            "STATUS BARU | Brewing: "
            + isBrewing
            + " | Ready: "
            + coffeeReady
        );
    }

    public bool TakeCoffee()
    {
        Debug.Log(
            "TAKE COFFEE DIPANGGIL | Brewing: "
            + isBrewing
            + " | Ready: "
            + coffeeReady
        );

        if (!coffeeReady)
        {
            Debug.LogWarning(
                "COFFEE BELUM READY! TakeCoffee tidak mereset status."
            );
            return false;
        }

        Debug.Log(
            "STATUS DIUBAH KE KOSONG OLEH TakeCoffee()"
        );

        coffeeReady = false;
        isBrewing = false;
        brewTimer = 0f;

        if (brewingAudio != null)
            brewingAudio.Stop();

        if (coffeeMug != null)
        {
            coffeeMug.SetCoffeeEmpty();
        }

        HideStatus();

        Debug.Log("COFFEE DIAMBIL PLAYER!");
        Debug.Log("SIKLUS COFFEE DI-RESET!");

        Debug.Log(
            "STATUS SETELAH DIAMBIL | Brewing: "
            + isBrewing
            + " | Ready: "
            + coffeeReady
        );

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

    void OnMouseDown()
    {
        Debug.Log("MESIN KOPI DIKLIK!");
        MakeCoffee();
    }
}