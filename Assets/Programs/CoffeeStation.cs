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
    public CoffeeItem[] coffeeMugs;

    [Header("Brewing Sound")]
    public AudioSource brewingAudio;

    private float brewTimer = 0f;

    void Awake()
    {
        ResolveCoffeeMug();
    }


    void ResolveCoffeeMug()
    {
        string suffix = "";

        if (gameObject.name == "CoffeeMachineLeft")
            suffix = "Left";
        else if (gameObject.name == "CoffeeMachineMid")
            suffix = "Mid";
        else if (gameObject.name == "CoffeeMachineRight")
            suffix = "Right";

        if (suffix == "")
        {
            Debug.LogError("Nama mesin kopi tidak cocok: " + gameObject.name);
            return;
        }

        CoffeeItem[] mugs =
            FindObjectsByType<CoffeeItem>(
                FindObjectsSortMode.None
            );

        foreach (CoffeeItem mug in mugs)
        {
            if (mug != null && mug.gameObject.name == "Mug" + suffix)
            {
                coffeeMugs = new CoffeeItem[] { mug };
                Debug.Log(gameObject.name + " terhubung ke " + mug.name);
                return;
            }
        }

        Debug.LogError("Mug" + suffix + " tidak ditemukan!");
    }



    void Update()
    {
        if (!isBrewing)
            return;

        brewTimer += Time.deltaTime;

        Debug.Log(gameObject.name + " Timer: " + brewTimer);

        if (brewTimer >= brewTime)
        {
            isBrewing = false;
            brewTimer = 0f;
            coffeeReady = true;

            if (brewingAudio != null)
                brewingAudio.Stop();

            if (coffeeMugs != null)
            {
                foreach (CoffeeItem mug in coffeeMugs)
                {
                    if (mug != null)
                        mug.SetCoffeeReady();
                }
            }

            ShowStatus("Coffee Ready!");
            Debug.Log(gameObject.name + " SELESAI BREWING!");
        }
    }

    public void MakeCoffee()
    {
        ResolveCoffeeMug();

        Debug.Log(gameObject.name + " tombol kopi ditekan!");

        if (isBrewing || coffeeReady)
        {
            Debug.Log("Kopi sedang dibuat atau sudah siap!");
            return;
        }

        isBrewing = true;
        coffeeReady = false;
        brewTimer = 0f;

        if (coffeeMugs != null)
        {
            foreach (CoffeeItem mug in coffeeMugs)
            {
                if (mug != null)
                {
                    mug.SetCoffeeEmpty();
                }
            }
        }

        if (brewingAudio != null)
        {
            brewingAudio.Stop();
            brewingAudio.Play();
        }

        ShowStatus("Making Coffee...");

        Debug.Log(gameObject.name + " mulai membuat kopi!");
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

        if (coffeeMugs != null)
        {
            foreach (CoffeeItem mug in coffeeMugs)
            {
                if (mug != null)
                    mug.SetCoffeeEmpty();
            }
        }

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