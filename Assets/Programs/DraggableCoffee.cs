using UnityEngine;

public class DraggableCoffee : MonoBehaviour
{
    private bool isDragging = false;
    private Camera mainCamera;
    private Vector3 offset;

    void Start()
    {
        mainCamera = Camera.main;
    }

    public void StartDragging()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(
                Input.mousePosition
            );

        mousePosition.z = transform.position.z;

        offset =
            transform.position - mousePosition;

        isDragging = true;

        Debug.Log("COFFEE MULAI DI-DRAG!");
    }

    void OnMouseDown()
    {
        StartDragging();
    }

    void OnMouseDrag()
    {
        if (!isDragging)
            return;

        Vector3 mousePosition =
            mainCamera.ScreenToWorldPoint(
                Input.mousePosition
            );

        mousePosition.z = transform.position.z;

        transform.position =
            mousePosition + offset;
    }

    void OnMouseUp()
    {
        if (!isDragging)
            return;

        isDragging = false;

        Collider2D[] hits =
            Physics2D.OverlapPointAll(
                transform.position
            );

        foreach (Collider2D hit in hits)
        {
            CustomerMovement1 customer1 =
                hit.GetComponentInParent<CustomerMovement1>();

            if (customer1 != null &&
                customer1.IsWaitingForOrder())
            {
                customer1.ReceiveCoffee();
                Debug.Log("COFFEE DIBERIKAN KE CUSTOMER 1!");
                Destroy(gameObject);
                return;
            }

            CustomerMovement2 customer2 =
                hit.GetComponentInParent<CustomerMovement2>();

            if (customer2 != null &&
                customer2.IsWaitingForOrder())
            {
                customer2.ReceiveCoffee();
                Debug.Log("COFFEE DIBERIKAN KE CUSTOMER 2!");
                Destroy(gameObject);
                return;
            }

            CustomerMovement3 customer3 =
                hit.GetComponentInParent<CustomerMovement3>();

            if (customer3 != null &&
                customer3.IsWaitingForOrder())
            {
                customer3.ReceiveCoffee();
                Debug.Log("COFFEE DIBERIKAN KE CUSTOMER 3!");
                Destroy(gameObject);
                return;
            }

            CustomerMovement4 customer4 =
                hit.GetComponentInParent<CustomerMovement4>();

            if (customer4 != null &&
                customer4.IsWaitingForOrder())
            {
                customer4.ReceiveCoffee();
                Debug.Log("COFFEE DIBERIKAN KE CUSTOMER 4!");
                Destroy(gameObject);
                return;
            }
        }

        Destroy(gameObject);
    }
}