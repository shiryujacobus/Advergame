using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject coffeeStatusText;

    [Header("Coffee Drag")]
    public GameObject draggableCoffeePrefab;

    [Header("Coffee Stations")]
    public CoffeeStation coffeeStationLeft;
    public CoffeeStation coffeeStationMid;
    public CoffeeStation coffeeStationRight;

    private GameObject draggedCoffee;
    private bool isDraggingCoffee = false;
    private Vector3 dragOffset;

    // Menyimpan mug dan mesin asal kopi yang sedang di-drag.
    private CoffeeItem draggedSourceMug;
    private CoffeeStation draggedFromStation;

    void Update()
    {
        // KONTROL MESIN KOPI DENGAN KEYBOARD
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (coffeeStationLeft != null)
                coffeeStationLeft.MakeCoffee();
            else
                Debug.LogWarning("CoffeeMachineLeft belum di-assign!");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (coffeeStationMid != null)
                coffeeStationMid.MakeCoffee();
            else
                Debug.LogWarning("CoffeeMachineMid belum di-assign!");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (coffeeStationRight != null)
                coffeeStationRight.MakeCoffee();
            else
                Debug.LogWarning("CoffeeMachineRight belum di-assign!");
        }

        if (isDraggingCoffee && draggedCoffee != null)
        {
            Vector3 mousePosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePosition.z = draggedCoffee.transform.position.z;
            draggedCoffee.transform.position = mousePosition + dragOffset;

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("COFFEE SELESAI DI-DRAG");
                CheckCoffeeCustomer();
                return;
            }

            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (Camera.main == null)
            {
                Debug.LogError("Main Camera tidak ditemukan!");
                return;
            }

            Vector2 mousePosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] hits =
                Physics2D.OverlapPointAll(mousePosition);

            // PRIORITAS 1: CEK MUG KOPI
            foreach (Collider2D colliderHit in hits)
            {
                CoffeeItem coffeeItem =
                    colliderHit.GetComponent<CoffeeItem>();

                if (coffeeItem == null)
                    continue;

                if (!coffeeItem.coffeeReady)
                {
                    Debug.Log(
                        "COFFEE BELUM READY! MUG TIDAK BISA DIAMBIL."
                    );
                    continue;
                }

                Debug.Log("MUG SIAP DIAMBIL!");

                StartCoffeeDrag(coffeeItem, mousePosition);
                return;
            }

            // PRIORITAS 2: CEK MESIN KOPI
            foreach (Collider2D colliderHit in hits)
            {
                CoffeeStation clickedStation =
                    colliderHit.GetComponentInParent<CoffeeStation>();

                if (clickedStation == null)
                    continue;

                Debug.Log("========== MESIN KOPI DIKLIK ==========");
                Debug.Log("Coffee Ready: " + clickedStation.coffeeReady);
                Debug.Log("Is Brewing: " + clickedStation.isBrewing);

                clickedStation.MakeCoffee();

                Debug.Log("========== SELESAI MAKE COFFEE ==========");
                return;
            }

            if (hits.Length == 0)
                Debug.Log("KLIK TIDAK MENGENAI OBJEK");
        }
    }

    // MENCARI MESIN BERDASARKAN MUG ASAL
    CoffeeStation FindStationForMug(CoffeeItem sourceMug)
    {
        if (sourceMug == null)
            return null;

        CoffeeStation[] stations =
            FindObjectsByType<CoffeeStation>(FindObjectsSortMode.None);

        foreach (CoffeeStation station in stations)
        {
            if (station != null && station.coffeeMug == sourceMug)
                return station;
        }

        return null;
    }

    // MULAI DRAG COFFEE
    void StartCoffeeDrag(
        CoffeeItem sourceMug,
        Vector2 mousePosition
    )
    {
        if (draggedCoffee != null)
            return;

        if (sourceMug == null)
        {
            Debug.LogError("Coffee Item sumber tidak ditemukan!");
            return;
        }

        if (draggableCoffeePrefab == null)
        {
            Debug.LogError(
                "Draggable Coffee Prefab BELUM DIISI!"
            );
            return;
        }

        // Temukan mesin yang terhubung dengan mug yang diklik.
        CoffeeStation sourceStation = FindStationForMug(sourceMug);

        if (sourceStation == null)
        {
            Debug.LogError(
                "Mesin untuk mug " + sourceMug.name +
                " tidak ditemukan. Periksa field Coffee Mug di CoffeeStation."
            );
            return;
        }

        draggedSourceMug = sourceMug;
        draggedFromStation = sourceStation;

        draggedCoffee = Instantiate(
            draggableCoffeePrefab,
            (Vector3)mousePosition,
            Quaternion.identity
        );

        dragOffset =
            draggedCoffee.transform.position -
            (Vector3)mousePosition;

        isDraggingCoffee = true;

        Debug.Log(
            "COFFEE DUPLIKAT DIBUAT: " +
            draggedCoffee.name +
            " | Mug sumber: " + draggedSourceMug.name +
            " | Mesin sumber: " + draggedFromStation.name +
            " | ID: " + draggedCoffee.GetEntityId()
        );
    }

    // CEK CUSTOMER
    void CheckCoffeeCustomer()
    {
        if (draggedCoffee == null)
            return;

        // Pastikan mesin asal kopi masih tersedia dan kopinya siap.
        if (draggedFromStation == null ||
            !draggedFromStation.coffeeReady ||
            draggedSourceMug == null ||
            !draggedSourceMug.coffeeReady)
        {
            Debug.LogWarning(
                "Kopi belum siap atau mesin asal tidak ditemukan."
            );
            ClearDraggedCoffee();
            return;
        }

        // Cari customer terdekat yang sedang menunggu pesanan.
        MonoBehaviour closestCustomer = null;
        float closestDistance = Mathf.Infinity;

        CustomerMovement1[] c1 =
            FindObjectsByType<CustomerMovement1>(FindObjectsSortMode.None);
        CustomerMovement2[] c2 =
            FindObjectsByType<CustomerMovement2>(FindObjectsSortMode.None);
        CustomerMovement3[] c3 =
            FindObjectsByType<CustomerMovement3>(FindObjectsSortMode.None);
        CustomerMovement4[] c4 =
            FindObjectsByType<CustomerMovement4>(FindObjectsSortMode.None);
        CustomerMovement5[] c5 =
            FindObjectsByType<CustomerMovement5>(FindObjectsSortMode.None);
        CustomerMovement6[] c6 =
            FindObjectsByType<CustomerMovement6>(FindObjectsSortMode.None);

        System.Action<MonoBehaviour> checkCustomer = customer =>
        {
            if (customer == null)
                return;

            bool waiting = false;

            if (customer is CustomerMovement1 a)
                waiting = a.IsWaitingForOrder();
            else if (customer is CustomerMovement2 b)
                waiting = b.IsWaitingForOrder();
            else if (customer is CustomerMovement3 c)
                waiting = c.IsWaitingForOrder();
            else if (customer is CustomerMovement4 d)
                waiting = d.IsWaitingForOrder();
            else if (customer is CustomerMovement5 e)
                waiting = e.IsWaitingForOrder();
            else if (customer is CustomerMovement6 f)
                waiting = f.IsWaitingForOrder();

            if (!waiting)
                return;

            float distance = Vector2.Distance(
                draggedCoffee.transform.position,
                customer.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCustomer = customer;
            }
        };

        foreach (var customer in c1) checkCustomer(customer);
        foreach (var customer in c2) checkCustomer(customer);
        foreach (var customer in c3) checkCustomer(customer);
        foreach (var customer in c4) checkCustomer(customer);
        foreach (var customer in c5) checkCustomer(customer);
        foreach (var customer in c6) checkCustomer(customer);

        // Batasi jarak penyerahan.
        const float maxDropDistance = 2.5f;

        if (closestCustomer == null ||
            closestDistance > maxDropDistance)
        {
            Debug.Log(
                "Tidak ada customer yang cukup dekat untuk menerima kopi."
            );
            ClearDraggedCoffee();
            return;
        }

        // Serahkan kopi ke customer.
        if (closestCustomer is CustomerMovement1 customer1)
            customer1.ReceiveCoffee();
        else if (closestCustomer is CustomerMovement2 customer2)
            customer2.ReceiveCoffee();
        else if (closestCustomer is CustomerMovement3 customer3)
            customer3.ReceiveCoffee();
        else if (closestCustomer is CustomerMovement4 customer4)
            customer4.ReceiveCoffee();
        else if (closestCustomer is CustomerMovement5 customer5)
            customer5.ReceiveCoffee();
        else if (closestCustomer is CustomerMovement6 customer6)
            customer6.ReceiveCoffee();

        // Kosongkan HANYA mesin asal kopi yang sedang diserahkan.
        bool coffeeTaken = draggedFromStation.TakeCoffee();

        if (!coffeeTaken)
        {
            Debug.LogWarning("Kopi gagal diambil dari mesin.");
            ClearDraggedCoffee();
            return;
        }

        Debug.Log(
            "Kopi dari " + draggedFromStation.name +
            " diserahkan ke " + closestCustomer.name
        );

        DestroyDraggedCoffeeSafely();
        ResetDragState();
    }

    // HAPUS GELAS DRAG DENGAN PEMERIKSAAN
    void DestroyDraggedCoffeeSafely()
    {
        if (draggedCoffee == null)
            return;

        // Jangan pernah menghapus mug utama.
        if (draggedSourceMug != null &&
            draggedCoffee == draggedSourceMug.gameObject)
        {
            Debug.LogError(
                "BUG: draggedCoffee adalah GELAS UTAMA! " +
                "Penghapusan dibatalkan."
            );
            return;
        }

        Debug.Log(
            "MENGHAPUS GELAS DRAG: " +
            draggedCoffee.name +
            " | ID: " + draggedCoffee.GetEntityId()
        );

        Destroy(draggedCoffee);
    }

    // RESET STATUS DRAG
    void ResetDragState()
    {
        draggedCoffee = null;
        draggedSourceMug = null;
        draggedFromStation = null;
        isDraggingCoffee = false;
        dragOffset = Vector3.zero;
    }

    // RESET DRAG SAAT PENYERAHAN GAGAL
    void ClearDraggedCoffee()
    {
        if (draggedCoffee != null)
            DestroyDraggedCoffeeSafely();

        ResetDragState();
    }
}