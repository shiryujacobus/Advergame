using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject coffeeStatusText;

    [Header("Coffee Drag")]
    public GameObject draggableCoffeePrefab;

    [Header("Coffee Station")]
    public CoffeeStation coffeeStation;

    private GameObject draggedCoffee;
    private bool isDraggingCoffee = false;
    private Vector3 dragOffset;

    void Update()
    {
        if (isDraggingCoffee && draggedCoffee != null)
        {
            Vector3 mousePosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mousePosition.z =
                draggedCoffee.transform.position.z;

            draggedCoffee.transform.position =
                mousePosition + dragOffset;

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
            Vector2 mousePosition =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] hits =
                Physics2D.OverlapPointAll(mousePosition);

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

                StartCoffeeDrag(
                    colliderHit.gameObject,
                    mousePosition
                );

                return;
            }

            // =========================
            // PRIORITAS 2: CEK ESPRESSO MACHINE
            // =========================

            foreach (Collider2D colliderHit in hits)
            {
                CoffeeStation clickedStation =
                    colliderHit.GetComponentInParent<CoffeeStation>();

                if (clickedStation == null)
                    continue;

                Debug.Log(
                    "========== MESIN KOPI DIKLIK =========="
                );

                Debug.Log(
                    "Coffee Ready: " +
                    clickedStation.coffeeReady
                );

                Debug.Log(
                    "Is Brewing: " +
                    clickedStation.isBrewing
                );

                clickedStation.MakeCoffee();

                Debug.Log(
                    "========== SELESAI MAKE COFFEE =========="
                );

                return;
            }

            // =========================
            // TIDAK MENGENAI OBJEK
            // =========================

            if (hits.Length == 0)
            {
                Debug.Log(
                    "KLIK TIDAK MENGENAI OBJEK"
                );
            }
        }
    }

    // =========================
    // MULAI DRAG COFFEE
    // =========================

    void StartCoffeeDrag(
        GameObject mug,
        Vector2 mousePosition
    )
    {
        if (draggedCoffee != null)
            return;

        if (draggableCoffeePrefab == null)
        {
            Debug.LogError(
                "Draggable Coffee Prefab BELUM DIISI!"
            );

            return;
        }

        draggedCoffee =
            Instantiate(
                draggableCoffeePrefab,
                (Vector3)mousePosition,
                Quaternion.identity
            );

        dragOffset =
            draggedCoffee.transform.position -
            (Vector3)mousePosition;

        isDraggingCoffee = true;

        Debug.Log(
            "COFFEE LANGSUNG SIAP DI-DRAG!"
        );
    }

    // =========================
    // CEK CUSTOMER
    // =========================

    void CheckCoffeeCustomer()
    {
        if (draggedCoffee == null)
            return;

        // =========================
        // CEK COFFEE STATION
        // =========================

        if (coffeeStation == null)
        {
            Debug.LogError(
                "Coffee Station belum di-assign di GameManager!"
            );

            ClearDraggedCoffee();
            return;
        }

        // =========================
        // CEK COFFEE READY
        // =========================

        if (!coffeeStation.coffeeReady)
        {
            Debug.Log(
                "COFFEE SUDAH KOSONG! CUSTOMER TIDAK MENERIMA."
            );

            ClearDraggedCoffee();
            return;
        }

        // =========================
        // CARI CUSTOMER TERDEKAT
        // =========================

        float closestDistance =
            Mathf.Infinity;

        CustomerMovement1 closestC1 = null;
        CustomerMovement2 closestC2 = null;
        CustomerMovement3 closestC3 = null;
        CustomerMovement4 closestC4 = null;

        // =========================
        // CUSTOMER 1
        // =========================

        CustomerMovement1[] customers1 =
            FindObjectsByType<CustomerMovement1>(
                FindObjectsSortMode.None
            );

        foreach (CustomerMovement1 customer in customers1)
        {
            if (!customer.IsWaitingForOrder())
                continue;

            float distance =
                Vector2.Distance(
                    draggedCoffee.transform.position,
                    customer.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestC1 = customer;
                closestC2 = null;
                closestC3 = null;
                closestC4 = null;
            }
        }

        // =========================
        // CUSTOMER 2
        // =========================

        CustomerMovement2[] customers2 =
            FindObjectsByType<CustomerMovement2>(
                FindObjectsSortMode.None
            );

        foreach (CustomerMovement2 customer in customers2)
        {
            if (!customer.IsWaitingForOrder())
                continue;

            float distance =
                Vector2.Distance(
                    draggedCoffee.transform.position,
                    customer.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestC1 = null;
                closestC2 = customer;
                closestC3 = null;
                closestC4 = null;
            }
        }

        // =========================
        // CUSTOMER 3
        // =========================

        CustomerMovement3[] customers3 =
            FindObjectsByType<CustomerMovement3>(
                FindObjectsSortMode.None
            );

        foreach (CustomerMovement3 customer in customers3)
        {
            if (!customer.IsWaitingForOrder())
                continue;

            float distance =
                Vector2.Distance(
                    draggedCoffee.transform.position,
                    customer.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestC1 = null;
                closestC2 = null;
                closestC3 = customer;
                closestC4 = null;
            }
        }

        // =========================
        // CUSTOMER 4
        // =========================

        CustomerMovement4[] customers4 =
            FindObjectsByType<CustomerMovement4>(
                FindObjectsSortMode.None
            );

        foreach (CustomerMovement4 customer in customers4)
        {
            if (!customer.IsWaitingForOrder())
                continue;

            float distance =
                Vector2.Distance(
                    draggedCoffee.transform.position,
                    customer.transform.position
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;

                closestC1 = null;
                closestC2 = null;
                closestC3 = null;
                closestC4 = customer;
            }
        }

        // =========================
        // TIDAK ADA CUSTOMER
        // =========================

        if (closestC1 == null &&
            closestC2 == null &&
            closestC3 == null &&
            closestC4 == null)
        {
            Debug.Log(
                "TIDAK ADA CUSTOMER YANG SEDANG MENUNGGU."
            );

            ClearDraggedCoffee();
            return;
        }

        // =========================
        // CEK JARAK
        // =========================

        float maxDropDistance = 2.5f;

        if (closestDistance > maxDropDistance)
        {
            Debug.Log(
                "COFFEE DILEPAS TERLALU JAUH DARI CUSTOMER!"
            );

            ClearDraggedCoffee();
            return;
        }

        // =========================
        // CUSTOMER DITEMUKAN
        // =========================

        Debug.Log(
            "CUSTOMER DITEMUKAN. JARAK: " +
            closestDistance
        );

        // =========================
        // BERIKAN COFFEE KE CUSTOMER
        // =========================
        // Coffee baru dikosongkan setelah customer
        // berhasil menerima coffee.

        if (closestC1 != null)
        {
            closestC1.ReceiveCoffee();

            Debug.Log(
                "COFFEE DIBERIKAN KE CUSTOMER 1!"
            );
        }
        else if (closestC2 != null)
        {
            closestC2.ReceiveCoffee();

            Debug.Log(
                "COFFEE DIBERIKAN KE CUSTOMER 2!"
            );
        }
        else if (closestC3 != null)
        {
            closestC3.ReceiveCoffee();

            Debug.Log(
                "COFFEE DIBERIKAN KE CUSTOMER 3!"
            );
        }
        else if (closestC4 != null)
        {
            closestC4.ReceiveCoffee();

            Debug.Log(
                "COFFEE DIBERIKAN KE CUSTOMER 4!"
            );
        }

        // =========================
        // AMBIL COFFEE DARI STATION
        // =========================

        bool coffeeTaken =
            coffeeStation.TakeCoffee();

        if (!coffeeTaken)
        {
            Debug.LogError(
                "COFFEE GAGAL DIAMBIL DARI COFFEE STATION!"
            );

            ClearDraggedCoffee();
            return;
        }

        Debug.Log(
            "COFFEE BERHASIL DIAMBIL DARI STATION."
        );

        // =========================
        // HAPUS MUG DRAG
        // =========================

        Destroy(draggedCoffee);

        draggedCoffee = null;
        isDraggingCoffee = false;
        dragOffset = Vector3.zero;

        Debug.Log(
            "COFFEE DRAG SYSTEM SUDAH DI-RESET!"
        );
    }

    // =========================
    // HAPUS COFFEE DRAG
    // =========================

    void ClearDraggedCoffee()
    {
        if (draggedCoffee != null)
        {
            Destroy(draggedCoffee);

            draggedCoffee = null;
        }

        isDraggingCoffee = false;
        dragOffset = Vector3.zero;
    }
}