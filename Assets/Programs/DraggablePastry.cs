
using UnityEngine;

public class DraggablePastry : MonoBehaviour
{
    private Vector3 originalPosition;
    private bool isDragging;

    private Camera mainCamera;
    private Collider2D bagCollider;

    private void Awake()
    {
        mainCamera = Camera.main;
        bagCollider = GetComponent<Collider2D>();
    }

    private void OnMouseDown()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null)
            return;

        originalPosition = transform.position;
        isDragging = true;

        Debug.Log("BUNGKUS MULAI DI-DRAG");
    }

    private void Update()
    {
        if (!isDragging || mainCamera == null)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = transform.position.z;
        transform.position = mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            DropBag();
        }
    }

    private void DropBag()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            0.7f
        );

        foreach (Collider2D hit in hits)
        {
            CustomerMovement1 c1 =
                hit.GetComponentInParent<CustomerMovement1>();

            if (c1 != null && c1.IsWaitingForOrder())
            {
                DeliverToCustomer(c1, "CUSTOMER 1");
                return;
            }

            CustomerMovement2 c2 =
                hit.GetComponentInParent<CustomerMovement2>();

            if (c2 != null && c2.IsWaitingForOrder())
            {
                DeliverToCustomer(c2, "CUSTOMER 2");
                return;
            }

            CustomerMovement3 c3 =
                hit.GetComponentInParent<CustomerMovement3>();

            if (c3 != null && c3.IsWaitingForOrder())
            {
                DeliverToCustomer(c3, "CUSTOMER 3");
                return;
            }

            CustomerMovement4 c4 =
                hit.GetComponentInParent<CustomerMovement4>();

            if (c4 != null && c4.IsWaitingForOrder())
            {
                DeliverToCustomer(c4, "CUSTOMER 4");
                return;
            }

            CustomerMovement5 c5 =
                hit.GetComponentInParent<CustomerMovement5>();

            if (c5 != null && c5.IsWaitingForOrder())
            {
                DeliverToCustomer(c5, "CUSTOMER 5");
                return;
            }

            CustomerMovement6 c6 =
                hit.GetComponentInParent<CustomerMovement6>();

            if (c6 != null && c6.IsWaitingForOrder())
            {
                DeliverToCustomer(c6, "CUSTOMER 6");
                return;
            }
        }

        Debug.Log("BUNGKUS TIDAK MENGENAI CUSTOMER.");
        transform.position = originalPosition;
    }

    private void DeliverToCustomer(
        MonoBehaviour customerComponent,
        string customerName)
    {
        if (FoodBagSystem.Instance == null)
        {
            Debug.LogError("FoodBagSystem tidak ditemukan!");
            transform.position = originalPosition;
            return;
        }

        ICustomerOrder customer =
            customerComponent as ICustomerOrder;

        if (customer == null)
        {
            Debug.LogError(
                customerName + " belum mengimplementasikan ICustomerOrder."
            );

            transform.position = originalPosition;
            return;
        }

        FoodBagSystem.Instance.TryDeliverBagToCustomer(customer);

        Debug.Log("BUNGKUS DIARAHKAN KE " + customerName);

        transform.position = originalPosition;
    }
}
