using UnityEngine;
using System.Collections.Generic;

public class CustomerMovement4 : MonoBehaviour
{
    public Transform buyPoint;
    public Transform exitPoint;

    public float speed = 2f;
    public float waitTime = 3f;

    public float maxWaitTime = 15f;
    private float waitTimer = 0f;

    public GameObject orderText;

    public ScoreManager scoreManager;

    public bool faceLeft = false;

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
    private float timer = 0f;

    private List<string> currentOrders =
        new List<string>();

    private List<string> receivedOrders =
        new List<string>();


    void Update()
    {
        if (state == 0)
        {
            MoveTo(buyPoint);

            if (Vector3.Distance(
                transform.position,
                buyPoint.position
            ) < 0.05f)
            {
                state = 1;

                timer = 0f;
                waitTimer = 0f;

                GenerateRandomOrder();
                ShowOrder();

                Debug.Log(
                    "CUSTOMER 4 SAMPAI BUY POINT!"
                );
            }
        }
        else if (state == 1)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= maxWaitTime)
            {
                Debug.Log(
                    "CUSTOMER 4 KEHABISAN WAKTU MENUNGGU!"
                );

                state = 2;

                if (orderText != null)
                {
                    orderText.SetActive(false);
                }
            }
        }

        else if (state == 2)
        {
            MoveTo(exitPoint);

            if (Vector3.Distance(
                transform.position,
                exitPoint.position
            ) < 0.05f)
            {
                Debug.Log(
                    "CUSTOMER 4 SUDAH KELUAR!"
                );

                gameObject.SetActive(false);
            }
        }
    }

    void GenerateRandomOrder()
    {
        currentOrders.Clear();
        receivedOrders.Clear();

        currentOrders.Add("Coffee");

        int pastryAmount =
            Random.Range(1, 4);

        List<string> availablePastry =
            new List<string>(pastryOrders);

        for (int i = 0; i < pastryAmount; i++)
        {
            int randomIndex =
                Random.Range(
                    0,
                    availablePastry.Count
                );

            currentOrders.Add(
                availablePastry[randomIndex]
            );

            availablePastry.RemoveAt(
                randomIndex
            );
        }
    }

    void ShowOrder()
    {
        if (orderText == null)
            return;

        orderText.SetActive(true);

        TMPro.TMP_Text text =
            orderText.GetComponent<TMPro.TMP_Text>();

        if (text != null)
        {
            string orderDisplay =
                "Order:\n";

            foreach (string order in currentOrders)
            {
                orderDisplay +=
                    "• " + order + "\n";
            }

            text.text = orderDisplay;
        }
    }

    public void ReceiveCoffee()
    {
        Debug.Log(
            "CUSTOMER 4 RECEIVE COFFEE DIPANGGIL | STATE: "
            + state
        );

        if (state != 1)
        {
            Debug.Log(
                "CUSTOMER 4 TIDAK BISA TERIMA COFFEE | STATE: "
                + state
            );

            return;
        }

        ReceiveItem("Coffee");
    }

    public void ReceivePastry(
        string pastryName
    )
    {
        if (state != 1)
            return;

        ReceiveItem(pastryName);
    }

    void ReceiveItem(string itemName)
    {
        if (!currentOrders.Contains(itemName))
        {
            Debug.Log(
                "CUSTOMER 4 TIDAK MEMESAN: "
                + itemName
            );

            return;
        }

        if (receivedOrders.Contains(itemName))
        {
            Debug.Log(
                "CUSTOMER 4 SUDAH MENERIMA: "
                + itemName
            );

            return;
        }

        receivedOrders.Add(itemName);

        Debug.Log(
            "CUSTOMER 4 MENERIMA: "
            + itemName
        );

        UpdateOrderDisplay();

        CheckOrderComplete();
    }

    void UpdateOrderDisplay()
    {
        if (orderText == null)
            return;

        TMPro.TMP_Text text =
            orderText.GetComponent<TMPro.TMP_Text>();

        if (text == null)
            return;

        string orderDisplay =
            "Order:\n";

        foreach (string order in currentOrders)
        {
            if (receivedOrders.Contains(order))
            {
                orderDisplay +=
                    "✓ " + order + "\n";
            }
            else
            {
                orderDisplay +=
                    "• " + order + "\n";
            }
        }

        text.text = orderDisplay;
    }

    void CheckOrderComplete()
    {
        if (receivedOrders.Count >= currentOrders.Count)
        {
            state = 2;

            if (orderText != null)
            {
                orderText.SetActive(false);
            }

            if (scoreManager != null)
            {
                scoreManager.AddScore(100);
            }

            Debug.Log(
                "SEMUA PESANAN CUSTOMER 4 "
                + "SUDAH DITERIMA! "
                + "CUSTOMER MENUJU EXIT!"
            );
        }
    }

    public bool IsWaitingForOrder()
    {
        return state == 1;
    }

    void MoveTo(Transform target)
    {
        if (target == null)
            return;

        bool faceLeft =
            target.position.x <
            transform.position.x;

        transform.position =
            Vector3.MoveTowards(
                transform.position,
                target.position,
                speed * Time.deltaTime
            );

        transform.localScale =
            new Vector3(
                faceLeft ? -1f : 1f,
                1f,
                1f
            );

        if (orderText != null)
        {
            orderText.transform.localScale =
                new Vector3(
                    faceLeft ? -1f : 1f,
                    1f,
                    1f
                );
        }
    }

    void OnEnable()
    {
        state = 0;

        timer = 0f;

        waitTimer = 0f;

        currentOrders.Clear();
        receivedOrders.Clear();

        if (orderText != null)
            orderText.SetActive(false);
    }
}