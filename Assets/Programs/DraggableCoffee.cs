
using UnityEngine;

public class DraggableCoffee : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;
    private Vector3 offset;

    public CoffeeStation coffeeStation;

    void Start()
    {
        mainCamera = Camera.main;

        if (coffeeStation == null)
        {
            coffeeStation = FindFirstObjectByType<CoffeeStation>();
        }

        if (coffeeStation == null)
        {
            Debug.LogError("CoffeeStation tidak ditemukan!");
        }
        else
        {
            Debug.Log("CoffeeStation berhasil terhubung!");
        }
    }

    public void StartDragging()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
        {
            Debug.LogError("Main Camera tidak ditemukan!");
            return;
        }

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = transform.position.z;

        offset = transform.position - mousePosition;
        isDragging = true;

        Debug.Log("COFFEE MULAI DI-DRAG!");
    }

    void OnMouseDown()
    {
        StartDragging();
    }

    void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = transform.position.z;
        transform.position = mousePosition + offset;
    }

void OnMouseUp()
    {
        if (!isDragging)
            return;

        isDragging = false;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            0.5f
        );

        foreach (Collider2D hit in hits)
        {
            CustomerMovement1 c1 =
                hit.GetComponentInParent<CustomerMovement1>();
            if (c1 != null && c1.IsWaitingForOrder())
            {
                DeliverCoffee(c1.ReceiveCoffee, "CUSTOMER 1");
                return;
            }

            CustomerMovement2 c2 =
                hit.GetComponentInParent<CustomerMovement2>();
            if (c2 != null && c2.IsWaitingForOrder())
            {
                DeliverCoffee(c2.ReceiveCoffee, "CUSTOMER 2");
                return;
            }

            CustomerMovement3 c3 =
                hit.GetComponentInParent<CustomerMovement3>();
            if (c3 != null && c3.IsWaitingForOrder())
            {
                DeliverCoffee(c3.ReceiveCoffee, "CUSTOMER 3");
                return;
            }

            CustomerMovement4 c4 =
                hit.GetComponentInParent<CustomerMovement4>();
            if (c4 != null && c4.IsWaitingForOrder())
            {
                DeliverCoffee(c4.ReceiveCoffee, "CUSTOMER 4");
                return;
            }

            CustomerMovement5 c5 =
                hit.GetComponentInParent<CustomerMovement5>();
            if (c5 != null && c5.IsWaitingForOrder())
            {
                DeliverCoffee(c5.ReceiveCoffee, "CUSTOMER 5");
                return;
            }

            CustomerMovement6 c6 =
                hit.GetComponentInParent<CustomerMovement6>();
            if (c6 != null && c6.IsWaitingForOrder())
            {
                DeliverCoffee(c6.ReceiveCoffee, "CUSTOMER 6");
                return;
            }
        }

        Debug.LogWarning("Kopi tidak mengenai customer.");
        // Jangan hapus gelas jika gagal mendeteksi customer.
    }

    void DeliverCoffee(
        System.Action receiveCoffee,
        string customerName)
    {
        if (coffeeStation == null)
            coffeeStation = FindFirstObjectByType<CoffeeStation>();

        if (coffeeStation == null)
        {
            Debug.LogError("CoffeeStation tidak ditemukan!");
            return;
        }

        if (!coffeeStation.coffeeReady)
        {
            Debug.LogWarning("Kopi belum ready!");
            return;
        }

        // Customer menerima kopi terlebih dahulu.
        receiveCoffee.Invoke();

        // Konsumsi kopi dan reset mesin.
        bool success = coffeeStation.TakeCoffee();

        if (!success)
        {
            Debug.LogWarning("Gagal mengambil kopi dari mesin.");
            return;
        }

        Debug.Log("COFFEE DIBERIKAN KE " + customerName + "!");
        Destroy(gameObject);
    }

    void ResetCoffeeStation()
    {
        if (coffeeStation == null)
            coffeeStation = FindFirstObjectByType<CoffeeStation>();

        if (coffeeStation == null)
        {
            Debug.LogError("CoffeeStation tidak ditemukan!");
            return;
        }

        coffeeStation.coffeeReady = false;
        coffeeStation.isBrewing = false;
        coffeeStation.MakeCoffee();

        Debug.Log("Mesin kopi di-reset dan brewing dimulai kembali.");
    }
}