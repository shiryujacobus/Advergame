
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FoodBagSystem : MonoBehaviour
{
    public static FoodBagSystem Instance;

    [Header("Bag UI")]
    public GameObject bagPanel;
    public TMP_Text bagText;
    public Button deliverButton;

    private ICustomerOrder selectedCustomer;

    private readonly Dictionary<string, int> bagItems =
        new Dictionary<string, int>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (bagPanel != null)
            bagPanel.SetActive(false);

        if (deliverButton != null)
            deliverButton.onClick.AddListener(DeliverBag);

        UpdateBagUI();
    }

    public void SelectCustomer(ICustomerOrder customer)
    {

        Debug.Log("CUSTOMER DIPILIH: " + customer);
        if (customer == null || !customer.IsWaitingForOrder())
            return;

        selectedCustomer = customer;
        bagItems.Clear();

        if (bagPanel != null)
            bagPanel.SetActive(true);

        UpdateBagUI();

        Debug.Log("Pesanan customer berhasil dipilih!");
    }

    public void AddFood(string itemName)
    {
        if (selectedCustomer == null)
        {
            Debug.Log("Klik customer terlebih dahulu!");
            return;
        }

        if (!selectedCustomer.IsWaitingForOrder())
        {
            Debug.Log("Customer tidak sedang menunggu pesanan.");
            return;
        }

        if (!selectedCustomer.AddItemToBag(itemName))
            return;

        if (!bagItems.ContainsKey(itemName))
            bagItems[itemName] = 0;

        bagItems[itemName]++;

        UpdateBagUI();
    }

    public void DeliverBag()
    {
        if (selectedCustomer == null)
        {
            Debug.Log("Belum memilih customer!");
            return;
        }

        if (bagItems.Count == 0)
        {
            Debug.Log("Kantong masih kosong!");
            return;
        }

        if (!selectedCustomer.DeliverBag())
        {
            Debug.Log("Pesanan belum lengkap!");
            return;
        }

        bagItems.Clear();
        selectedCustomer = null;

        UpdateBagUI();

        if (bagPanel != null)
            bagPanel.SetActive(false);
    }

    private void UpdateBagUI()
    {
        if (bagText == null)
            return;

        if (bagItems.Count == 0)
        {
            bagText.text = "Kantong kosong";
            return;
        }

        string text = "ISI KANTONG\n";

        foreach (var item in bagItems)
            text += item.Key + " x" + item.Value + "\n";

        bagText.text = text;
    }
    public void TryDeliverBagToCustomer(ICustomerOrder targetCustomer)
    {

        Debug.Log("CUSTOMER TUJUAN: " + targetCustomer);
        Debug.Log("CUSTOMER TERPILIH: " + selectedCustomer);

        if (targetCustomer == null)
        {
            Debug.Log("Customer tidak ditemukan!");
            return;
        }

        if (selectedCustomer != targetCustomer)
        {
            Debug.Log("Pilih customer yang sesuai dengan kantong!");
            return;
        }

        if (bagItems.Count == 0)
        {
            Debug.Log("Kantong masih kosong!");
            return;
        }

        if (!targetCustomer.DeliverBag())
        {
            Debug.Log("Pesanan belum lengkap!");
            return;
        }

        bagItems.Clear();
        selectedCustomer = null;

        UpdateBagUI();

        if (bagPanel != null)
            bagPanel.SetActive(false);

        Debug.Log("Kantong berhasil diberikan!");
    }
}
