using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomerMovement2 : MonoBehaviour
{
    [Header("Movement")]
    public Transform buyPoint;
    public Transform exitPoint;
    public float speed = 2f;
    public float waitTime = 3f;
    public float maxWaitTime = 15f;

    public RectTransform orderPanelRect;
    public RectTransform orderItemsRect;
    public float iconSize = 65f;
    public float iconSpacing = 6f;
    public float horizontalPadding = 10f;
    public float verticalPadding = 10f;

    private float waitTimer = 0f;
    private float nextSlotCheck = 0f;

    private bool hasQueueSlot = false;
    private Transform assignedBuyPoint;
    private Transform assignedWaitPoint;

    private float initialSpawnX;
    private bool spawnFromRight = false;
    private bool spawnSideDetected = false;

    [Header("Order UI")]
    public GameObject orderPanel;
    public Image coffeeIcon;
    public Image donutVanillaIcon;
    public Image donutChocolateIcon;
    public Image donutStrawberryIcon;
    public Image croissantIcon;
    public Image bakpaoIcon;
    public Image rotiIcon;

    public ScoreManager scoreManager;

    [Header("Sprite Direction")]
    public bool faceLeft = false;
    public bool spriteFacesRight = true;

    [Header("Pastry Orders")]
    public string[] pastryOrders =
    {
        "Donut Vanilla",
        "Donut Chocolate",
        "Donut Strawberry",
        "Croissant",
        "Bakpao",
        "Roti"
    };

    private int state = 0;

    private List<string> currentOrders = new List<string>();
    private List<string> receivedOrders = new List<string>();

    void Update()
    {
        CustomerQueueManager queue = CustomerQueueManager.Instance;

        if (queue == null)
            return;

        if (!spawnSideDetected)
        {
            spawnFromRight = queue.IsSpawnFromRight(initialSpawnX);
            spawnSideDetected = true;
        }

        // STATE 0: Bergerak ke Buy Point atau menunggu slot.
        if (state == 0)
        {
            if (!hasQueueSlot)
            {
                if (Time.time >= nextSlotCheck)
                {
                    nextSlotCheck = Time.time + 0.5f;

                    hasQueueSlot = queue.RequestEntry(
                        this,
                        spawnFromRight,
                        out assignedBuyPoint
                    );

                    if (hasQueueSlot)
                    {
                        buyPoint = assignedBuyPoint;
                        assignedWaitPoint = null;
                    }
                }

                if (!hasQueueSlot)
                {
                    assignedWaitPoint = queue.GetWaitPoint(this);

                    if (assignedWaitPoint != null)
                        MoveTo(assignedWaitPoint);

                    return;
                }
            }

            if (buyPoint == null)
                return;

            MoveTo(buyPoint);

            if (Vector3.Distance(transform.position, buyPoint.position) < 0.05f)
            {
                state = 1;
                waitTimer = 0f;

                GenerateRandomOrder();
                ShowOrder();

                Debug.Log("CUSTOMER 2 SAMPAI BUY POINT!");
            }
        }

        // STATE 1: Menunggu pesanan.
        else if (state == 1)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= maxWaitTime)
            {
                Debug.Log("CUSTOMER 2 KEHABISAN WAKTU!");

                state = 2;

                if (orderPanel != null)
                    orderPanel.SetActive(false);
            }
        }

        // STATE 2: Keluar setelah pesanan selesai atau waktu habis.
        else if (state == 2)
        {
            if (exitPoint == null)
                return;

            MoveTo(exitPoint);

            if (Vector3.Distance(transform.position, exitPoint.position) < 0.05f)
            {
                hasQueueSlot = false;
                assignedBuyPoint = null;
                assignedWaitPoint = null;

                if (orderPanel != null)
                    orderPanel.SetActive(false);

                queue.RequeueCustomer(this, transform.position, 0f);

                state = 0;
                waitTimer = 0f;
                nextSlotCheck = Time.time + queue.spawnDelay;
            }

        }
    }

    void GenerateRandomOrder()
    {
        currentOrders.Clear();
        receivedOrders.Clear();

        currentOrders.Add("Coffee");

        if (pastryOrders == null || pastryOrders.Length == 0)
            return;

        int maxPastries = Mathf.Min(3, pastryOrders.Length);
        int pastryAmount = Random.Range(1, maxPastries + 1);

        List<string> availablePastry = new List<string>(pastryOrders);

        for (int i = 0; i < pastryAmount; i++)
        {
            if (availablePastry.Count == 0)
                break;

            int randomIndex = Random.Range(0, availablePastry.Count);

            currentOrders.Add(availablePastry[randomIndex]);
            availablePastry.RemoveAt(randomIndex);
        }
    }

    void ShowOrder()
    {
        if (orderPanel == null)
            return;

        orderPanel.SetActive(true);

        SetIcon(coffeeIcon, false);
        SetIcon(donutVanillaIcon, false);
        SetIcon(donutChocolateIcon, false);
        SetIcon(donutStrawberryIcon, false);
        SetIcon(croissantIcon, false);
        SetIcon(bakpaoIcon, false);
        SetIcon(rotiIcon, false);

        foreach (string order in currentOrders)
        {
            SetIcon(GetIcon(order), true);
        }
    }

    Image GetIcon(string itemName)
    {
        switch (itemName)
        {
            case "Coffee":
                return coffeeIcon;

            case "Donut Vanilla":
                return donutVanillaIcon;

            case "Donut Chocolate":
                return donutChocolateIcon;

            case "Donut Strawberry":
                return donutStrawberryIcon;

            case "Croissant":
                return croissantIcon;

            case "Bakpao":
                return bakpaoIcon;

            case "Roti":
                return rotiIcon;

            default:
                return null;
        }
    }

    void SetIcon(Image icon, bool visible)
    {
        if (icon != null)
            icon.gameObject.SetActive(visible);
    }

    public void ReceiveCoffee()
    {
        if (state != 1)
            return;

        ReceiveItem("Coffee");
    }

    public void ReceivePastry(string pastryName)
    {
        if (state != 1)
            return;

        ReceiveItem(pastryName);
    }

    void ReceiveItem(string itemName)
    {
        if (!currentOrders.Contains(itemName))
        {
            Debug.Log("CUSTOMER 2 TIDAK MEMESAN: " + itemName);
            return;
        }

        if (receivedOrders.Contains(itemName))
        {
            Debug.Log("CUSTOMER 2 SUDAH MENERIMA: " + itemName);
            return;
        }

        receivedOrders.Add(itemName);

        UpdateOrderDisplay();
        CheckOrderComplete();
    }

    void UpdateOrderDisplay()
    {
        foreach (string order in currentOrders)
        {
            Image icon = GetIcon(order);

            if (icon != null)
            {
                icon.gameObject.SetActive(
                    !receivedOrders.Contains(order)
                );
            }
        }
    }

    void CheckOrderComplete()
    {
        if (receivedOrders.Count < currentOrders.Count)
            return;

        state = 2;

        if (orderPanel != null)
            orderPanel.SetActive(false);

        if (scoreManager != null)
            scoreManager.AddScore(100);

        Debug.Log("PESANAN CUSTOMER 2 SELESAI!");
    }

    public bool IsWaitingForOrder()
    {
        return state == 1;
    }

    void MoveTo(Transform target)
    {
        if (target == null)
            return;

        bool movingLeft = target.position.x < transform.position.x;

        bool flip = spriteFacesRight ? movingLeft : !movingLeft;

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        transform.localScale = new Vector3(
            flip ? -1f : 1f,
            1f,
            1f
        );

        if (orderPanel != null)
        {
            orderPanel.transform.localScale = new Vector3(
                flip ? -1f : 1f,
                1f,
                1f
            );
        }
    }

    void OnEnable()
    {
        initialSpawnX = transform.position.x;
        spawnSideDetected = false;

        state = 0;
        waitTimer = 0f;
        nextSlotCheck = 0f;
        hasQueueSlot = false;

        assignedBuyPoint = null;
        assignedWaitPoint = null;

        currentOrders.Clear();
        receivedOrders.Clear();

        if (orderPanel != null)
            orderPanel.SetActive(false);
    }

    void OnDisable()
    {
        CustomerQueueManager queue = CustomerQueueManager.Instance;

        if (queue != null)
            queue.RemoveCustomer(this);
    }

    void ResizeOrderPanel()
    {
        if (orderPanelRect == null || orderItemsRect == null)
            return;

        int count = 0;

        foreach (string order in currentOrders)
        {
            Image icon = GetIcon(order);

            if (icon != null && icon.gameObject.activeSelf)
                count++;
        }

        float width = count > 0
            ? (count * iconSize) + ((count - 1) * iconSpacing) + horizontalPadding * 2
            : horizontalPadding * 2;

        float height = iconSize + verticalPadding * 2;

        orderPanelRect.sizeDelta = new Vector2(width, height);
    }
}