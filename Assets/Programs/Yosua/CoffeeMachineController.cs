
using UnityEngine;
using TMPro;

public class CoffeeMachineController : MonoBehaviour
{
    [System.Serializable]
    public class CoffeeMachine
    {
        public string machineName;
        public KeyCode key;
        public CoffeeItem mug;
        public GameObject statusObject;
        public AudioSource brewingAudio;

        [HideInInspector] public bool isBrewing;
        [HideInInspector] public bool coffeeReady;
        [HideInInspector] public float timer;
    }

    [Header("Coffee Machines")]
    public CoffeeMachine[] machines;

    [Header("Settings")]
    public float brewTime = 3f;

    private void Start()
    {
        foreach (CoffeeMachine machine in machines)
        {
            SetStatus(machine, "");

            if (machine.mug != null)
                machine.mug.SetCoffeeEmpty();
        }
    }

    private void Update()
    {
        foreach (CoffeeMachine machine in machines)
        {
            if (Input.GetKeyDown(machine.key))
                StartBrewing(machine);

            if (machine.isBrewing)
            {
                machine.timer += Time.deltaTime;

                if (machine.timer >= brewTime)
                    FinishBrewing(machine);
            }

            if (Input.GetMouseButtonDown(0))
                TryClickMug(machine);
        }
    }

    private void StartBrewing(CoffeeMachine machine)
    {
        if (machine.isBrewing || machine.coffeeReady)
            return;

        machine.isBrewing = true;
        machine.coffeeReady = false;
        machine.timer = 0f;

        if (machine.mug != null)
            machine.mug.SetCoffeeEmpty();

        if (machine.brewingAudio != null)
        {
            machine.brewingAudio.Stop();
            machine.brewingAudio.Play();
        }

        SetStatus(machine, "Making Coffee...");
        Debug.Log(machine.machineName + ": mulai membuat kopi.");
    }

    private void FinishBrewing(CoffeeMachine machine)
    {
        machine.isBrewing = false;
        machine.coffeeReady = true;
        machine.timer = 0f;

        if (machine.brewingAudio != null)
            machine.brewingAudio.Stop();

        if (machine.mug != null)
            machine.mug.SetCoffeeReady();

        SetStatus(machine, "Coffee Ready!");
        Debug.Log(machine.machineName + ": kopi selesai!");
    }


    private void TryClickMug(CoffeeMachine machine)
    {
        if (machine.mug == null)
            return;

        Camera cam = Camera.main;
        if (cam == null)
            return;

        Vector3 mousePosition =
            cam.ScreenToWorldPoint(Input.mousePosition);

        Collider2D[] hits = Physics2D.OverlapPointAll(
            new Vector2(mousePosition.x, mousePosition.y)
        );

        foreach (Collider2D hit in hits)
        {
            if (hit == null)
                continue;

            bool clickedMug =
                hit.gameObject == machine.mug.gameObject ||
                hit.transform.IsChildOf(machine.mug.transform);

            if (!clickedMug)
                continue;

            if (!machine.coffeeReady || !machine.mug.coffeeReady)
            {
                Debug.Log(machine.machineName + ": kopi belum siap.");
                return;
            }

            if (FoodBagSystem.Instance == null)
            {
                Debug.LogError("FoodBagSystem tidak ditemukan!");
                return;
            }

            FoodBagSystem.Instance.AddFood("Coffee");

            machine.coffeeReady = false;
            machine.isBrewing = false;
            machine.timer = 0f;

            machine.mug.SetCoffeeEmpty();
            SetStatus(machine, "");

            Debug.Log(machine.machineName + ": kopi berhasil diserahkan.");
            return;
        }
    }


    private void SetStatus(CoffeeMachine machine, string message)
    {
        if (machine.statusObject == null)
            return;

        machine.statusObject.SetActive(!string.IsNullOrEmpty(message));

        TMP_Text text =
            machine.statusObject.GetComponent<TMP_Text>();

        if (text != null)
            text.text = message;
    }
}
