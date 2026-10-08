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
        // Tidak sedang membuat coffee
        if (!isBrewing)
            return;

        brewTimer += Time.deltaTime;

        // Coffee selesai dibuat
        if (brewTimer >= brewTime)
        {
            isBrewing = false;
            brewTimer = 0f;
            coffeeReady = true;

            // Stop suara mesin
            if (brewingAudio != null)
            {
                brewingAudio.Stop();
            }

            // Isi mug
            if (coffeeMug != null)
            {
                coffeeMug.SetCoffeeReady();
            }

            ShowStatus("Coffee Ready!");

            Debug.Log(
                "COFFEE SUDAH READY!"
            );

            Debug.Log(
                "STATUS: Brewing = "
                + isBrewing
                + " | Ready = "
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

        // =========================
        // CEK SEDANG BREWING
        // =========================

        if (isBrewing)
        {
            Debug.Log(
                "COFFEE MASIH SEDANG DIBUAT!"
            );

            return;
        }

        // =========================
        // CEK COFFEE MASIH READY
        // =========================

        if (coffeeReady)
        {
            Debug.Log(
                "COFFEE MASIH READY!"
            );

            return;
        }

        // =========================
        // MULAI SIKLUS BARU
        // =========================

        isBrewing = true;
        coffeeReady = false;
        brewTimer = 0f;

        // Pastikan mug kosong
        if (coffeeMug != null)
        {
            coffeeMug.SetCoffeeEmpty();
        }

        // =========================
        // SUARA MESIN
        // =========================

        if (brewingAudio != null)
        {
            brewingAudio.Stop();
            brewingAudio.Play();
        }

        ShowStatus("Making Coffee...");

        Debug.Log(
            "COFFEE SEDANG DIBUAT..."
        );

        Debug.Log(
            "STATUS BARU: Brewing = "
            + isBrewing
            + " | Ready = "
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

        // =========================
        // CEK COFFEE READY
        // =========================

        if (!coffeeReady)
        {
            Debug.Log(
                "COFFEE BELUM READY!"
            );

            return false;
        }

        // =========================
        // RESET TOTAL SIKLUS
        // =========================

        coffeeReady = false;
        isBrewing = false;
        brewTimer = 0f;

        // Stop suara kalau masih berjalan
        if (brewingAudio != null)
        {
            brewingAudio.Stop();
        }

        // Kosongkan mug
        if (coffeeMug != null)
        {
            coffeeMug.SetCoffeeEmpty();
        }

        HideStatus();

        Debug.Log(
            "COFFEE DIAMBIL PLAYER!"
        );

        Debug.Log(
            "SIKLUS COFFEE DI-RESET!"
        );

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
        {
            text.text = message;
        }
    }

    void HideStatus()
    {
        if (coffeeStatusText != null)
        {
            coffeeStatusText.SetActive(false);
        }
    }
}