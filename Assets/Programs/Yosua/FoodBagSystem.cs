
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

    [Header("Score System")]
    public ScoreManager scoreManager;

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
        if (scoreManager == null)
            scoreManager = FindFirstObjectByType<ScoreManager>();

        if (bagPanel != null)
            bagPanel.SetActive(false);

        if (deliverButton != null)
            deliverButton.onClick.AddListener(DeliverBag);

        UpdateBagUI();
    }

    public void SelectCustomer(ICustomerOrder customer)
    {
        if (customer == null)
            return;

        if (customer.CanAcceptOrder())
        {
            customer.AcceptOrder();
        }
        else if (!customer.IsWaitingForOrder())
        {
            return;
        }

        selectedCustomer = customer;
        bagItems.Clear();

        if (bagPanel != null)
            bagPanel.SetActive(true);

        UpdateBagUI();

        Debug.Log("Customer berhasil dipilih!");
    }

    public void AddFood(string itemName)
    {
        if (selectedCustomer == null)
            return;

        if (!selectedCustomer.IsWaitingForOrder())
            return;

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
            return;

        if (bagItems.Count == 0)
            return;

        if (!selectedCustomer.DeliverBag())
        {
            Debug.Log("Pesanan belum lengkap!");
            return;
        }

        CompleteDelivery();
    }

    public void TryDeliverBagToCustomer(ICustomerOrder targetCustomer)
    {
        if (targetCustomer == null)
            return;

        if (selectedCustomer != targetCustomer)
        {
            Debug.Log("Customer tidak sesuai!");
            return;
        }

        if (bagItems.Count == 0)
            return;

        if (!targetCustomer.DeliverBag())
        {
            Debug.Log("Pesanan belum lengkap!");
            return;
        }

        CompleteDelivery();

        Debug.Log("Kantong berhasil diberikan!");
    }

    // DIPANGGIL KETIKA WAKTU CUSTOMER HABIS
    public void CancelBagForCustomer(ICustomerOrder customer)
    {
        if (customer == null)
            return;

        // Pastikan hanya kantong milik customer tersebut yang dihapus
        if (selectedCustomer != customer)
            return;

        Debug.Log("WAKTU HABIS! KANTONG DIHAPUS.");

        ResetBag();
    }

    // RESET SELURUH DATA KANTONG
    public void ResetBag()
    {
        bagItems.Clear();
        selectedCustomer = null;

        UpdateBagUI();

        if (bagPanel != null)
            bagPanel.SetActive(false);

        Debug.Log("Kantong kembali ke kondisi awal!");
    }

    private void CompleteDelivery()
    {
        int totalItems = 0;

        foreach (var item in bagItems)
        {
            totalItems += item.Value;
        }

        if (scoreManager != null)
        {
            scoreManager.AddScore(totalItems);
            Debug.Log("Score bertambah: " + totalItems);
        }

        ResetBag();
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
        {
            text += item.Key + " x" + item.Value + "\n";
        }

        bagText.text = text;
    }
}
